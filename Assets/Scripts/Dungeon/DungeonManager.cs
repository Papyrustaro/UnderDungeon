using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private List<BattleCharacter> allys; //battleCharacterとして持つべきか？

    /// <summary>
    /// ご褒美用アイテム
    /// </summary>
    [SerializeField] private BattlePassiveItem goodItem;

    /// <summary>
    /// 罰用アイテム
    /// </summary>
    [SerializeField] private BattlePassiveItem badItem;

    [SerializeField] private DungeonSquaresFunc dungeonSquaresFunc;

    [SerializeField] private DungeonActiveEffectsFunc dungeonActiveEffectsFunc;
    [SerializeField] private DungeonPassiveEffectsFunc dungeonPassiveEffectsFunc;
    [SerializeField] private BattleActiveEffectsFunc battleActiveEffectsFunc;
    [SerializeField] private BattlePassiveEffectsFunc battlePassiveEffectsFunc;

    [SerializeField] private DungeonUIManager dungeonUIManager;

    private List<DungeonActiveItem> haveDungeonActiveItems = new List<DungeonActiveItem>();
    private List<DungeonPassiveItem> haveDungeonPassiveItems = new List<DungeonPassiveItem>();
    private List<BattleActiveItem> haveBattleActiveItems = new List<BattleActiveItem>();
    private List<BattlePassiveItem> haveBattlePassiveItems = new List<BattlePassiveItem>();
    private int[] baseDice = new int[6] { 1, 1, 2, 2, 3, 3 };
    private int[] changedDice = null;
    private E_DungeonScene currentScene = E_DungeonScene.WaitSetFirstData;


    private List<PlayerCharacter> dropCharacters = new List<PlayerCharacter>(); //IDとして持ってもよい

    private E_DungeonSquareType[,] currentFloorDungeonSquares;
    private bool[,] understandDungeonSquareType; //マスイベントが何かわかるか(自分の周囲のマスのタイプを記憶する)

    private List<int> haveDsp = new List<int>(); //保持しているDsp(とりあえずこのクラスで保持)
    private List<int> needDsp = new List<int>(); //それぞれのスキルに必要なDsp(とりあえずこのクラスで保持)

    [SerializeField] private MapManager mapManager;

    private List<BattleCharacter> targetAllys = null; //効果対象味方(とりあえず)
    private List<DungeonSquare> targetDungeonSquares = null; //効果対象マス(とりあえず)

    /// <summary>
    /// 満腹度(マス移動する度に減少?)
    /// </summary>
    public int Fullness { get; set; } = 100;

    /// <summary>
    /// 最大満腹度
    /// </summary>
    public int MaxFullNess { get; set; }

    public List<BattleCharacter> Allys => this.allys;

    /// <summary>
    /// マップの上から何行目か(0スタート)
    /// </summary>
    public int CurrentLocationRow { get; set; } = 0;

    /// <summary>
    /// マップの左から何列目か(0スタート)
    /// </summary>
    public int CurrentLocationColumn { get; set; } = 0;

    /// <summary>
    /// ダンジョン潜入してからの経過ターン(階層降りる毎にリセット?)
    /// </summary>
    public int TurnFromInfiltration { get; set; } = 0;

    /// <summary>
    /// 祈祷回数(ダンジョン潜入時0)
    /// </summary>
    public int CountOfPray { get; set; } = 0;

    /// <summary>
    /// 何マス先まで見えるか→通常と狭いの2パターンのみにするか？
    /// </summary>
    public int FieldOfVision { get; set; }

    /// <summary>
    /// 可視範囲減少効果の残りターン
    /// </summary>
    public int FieldOfVisionNarrowTurn { get; set; } = 0;

    /// <summary>
    /// 現在の階層
    /// </summary>
    public int CurrentFloor { get; set; }

    /// <summary>
    /// 潜入しているダンジョンID
    /// </summary>
    public E_Dungeon CurrentDungeon { get; set; }

    /// <summary>
    /// 所持ゴールド量
    /// </summary>
    public int HaveGold { get; set; } = 0;

    /// <summary>
    /// DungeonActiveSkillの発動を封じている残りターン数
    /// </summary>
    public int EnclosedUseDungeonActiveSkillTurn { get; set; } = 0;

    /// <summary>
    /// 基本のサイコロの目
    /// </summary>
    public int[] BaseDice => this.baseDice;

    /// <summary>
    /// 変化したサイコロの目(無いときはnull)
    /// </summary>
    public int[] ChangedDice => this.changedDice;

    /// <summary>
    /// マス移動量増加量(基本0)
    /// </summary>
    public int MovementIncreaseValue { get; set; } = 0;

    /// <summary>
    /// マス移動量の倍率(基本1倍)
    /// </summary>
    public int MovementRate { get; set; } = 1;

    public List<DungeonActiveItem> HaveDungeonActiveItems => this.haveDungeonActiveItems;
    public List<DungeonPassiveItem> HaveDungeonPassiveItems => this.haveDungeonPassiveItems;
    public List<BattleActiveItem> HaveBattleActiveItems => this.haveBattleActiveItems;
    public List<BattlePassiveItem> HaveBattlePassiveItems => this.haveBattlePassiveItems;


    /// <summary>
    /// DungeonSquareイベントの処理待ちかどうか
    /// </summary>
    public bool WaitDungeonSquareEvent { get; set; } = false;

    private void Start()
    {
        //this.mapManager.GenerateFloor(this.currentFloorDungeonSquares);
        this.GenerateFloor(this.mapManager.MapWidth, this.mapManager.MapHeight);
        this.dungeonSquaresFunc.SetMayApeearDungeonSquares(this.mapManager.MayApeearDungeonSquares);
        Debug.Log(this.currentFloorDungeonSquares[0, 0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) MoveDungeonSquare(E_Direction.Up);
        else if (Input.GetKeyDown(KeyCode.D)) MoveDungeonSquare(E_Direction.Right);
        else if (Input.GetKeyDown(KeyCode.S)) MoveDungeonSquare(E_Direction.Down);
        else if (Input.GetKeyDown(KeyCode.A)) MoveDungeonSquare(E_Direction.Left);

        if (this.WaitDungeonSquareEvent)
        {
            this.dungeonSquaresFunc.DungeonSquareEvent(this, this.currentFloorDungeonSquares[CurrentLocationRow, CurrentLocationColumn]);
            this.WaitDungeonSquareEvent = false;
        }
    }

    /// <summary>
    /// 現在の状態に合わせてプレイヤーの入力などの分岐処理(Updateで呼ばれる)
    /// </summary>
    private void ActionInCurrentScene()
    {
        switch (this.currentScene)
        {
            case E_DungeonScene.WaitSetFirstData:
                break;
            case E_DungeonScene.SelectAction:
                InputSelectAction();
                break;
            case E_DungeonScene.SelectDAI:
                InputUseDungeonActiveItem();
                break;
            case E_DungeonScene.SelectDAITarget:
                break;
            case E_DungeonScene.SelectDAS:
                InputInvokeDungeonActiveSkill();
                break;
            case E_DungeonScene.SelectDASTarget:
                break;
            case E_DungeonScene.SelectMoveDirection:
                break;
            case E_DungeonScene.ViewAllyStatus:
                break;
            case E_DungeonScene.ViewMap:
                break;
            case E_DungeonScene.WaitDungeonSquareEvent:
                break;
        }
    }

    /// <summary>
    /// 行動選択の入力受付(アイテム・スキル使用、サイコロ投げる、マップ見るなど)
    /// </summary>
    private void SelectAction()
    {
        this.dungeonUIManager.AnnounceByText("0.サイコロを投げる, 1.アイテム使用, 2.スキル使用, 3.マップ確認, 4.パーティ確認,");
    }

    

    /// <summary>
    /// 行動選択の入力受付(0:サイコロ, 1:アイテム使用, 2:DAS発動, 3:マップ確認, 4:パーティ確認)
    /// </summary>
    private void InputSelectAction()
    {
        if (Input.GetKeyDown(E_DungeonPlayerSelect.RollDice.ToString()))
        {
            //サイコロ投げに遷移
        }else if (Input.GetKeyDown(E_DungeonPlayerSelect.UseDungeonActiveItem.ToString()))
        {
            //所持アイテム確認に遷移
        }else if (Input.GetKeyDown(E_DungeonPlayerSelect.InvokeDungeonActiveSkill.ToString()))
        {
            //DAS発動に遷移
        }else if (Input.GetKeyDown(E_DungeonPlayerSelect.VerificateMap.ToString()))
        {
            //マップ確認に遷移
        }else if (Input.GetKeyDown(E_DungeonPlayerSelect.VerificateAlly.ToString()))
        {
            //パーティ確認に遷移
        }
    }

    private int RollDice()
    {
        int diceEye;
        string s = "使用するダイス[";
        if (this.changedDice == null)
        {
            for(int i = 0; i < 6; i++)
            {
                s += this.baseDice[i].ToString() + ",";
            }
            diceEye = this.baseDice[UnityEngine.Random.Range(0, 6)];
        }
        else
        {
            for(int i = 0; i < this.changedDice.Length; i++)
            {
                s += this.changedDice[i].ToString() + ",";
            }
            diceEye = this.changedDice[UnityEngine.Random.Range(0, this.changedDice.Length)];
        }
        s += "]";
        this.dungeonUIManager.AnnounceByText(s);

        this.dungeonUIManager.AnnounceByText("出た目:" + diceEye.ToString());
        return diceEye;
    }

    /// <summary>
    /// 所持DAIの使用(今は0~9までキーボード入力)
    /// </summary>
    private void InputUseDungeonActiveItem()
    {
        int itemNum = this.haveDungeonActiveItems.Count;
        for(int i = 0; i < itemNum; i++) //とりあえず9以下まで(10以上の入力ができないため)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                this.haveDungeonActiveItems[i].EffectFunc(this);
            }
        }
    }

    /// <summary>
    /// 所持しているDAIを先頭から番号づけて表示
    /// </summary>
    private void ShowHaveDungeonActiveItem()
    {
        string s = "所持しているDAI\n";
        int itemNum = this.haveDungeonActiveItems.Count;
        for(int i = 0; i < itemNum; i++)
        {
            s += i.ToString() + ":" + this.haveDungeonActiveItems[i].EffectName;
        }
        this.dungeonUIManager.AnnounceByText(s);
    }

    /// <summary>
    /// 所持しているDASを先頭から番号づけて表示
    /// </summary>
    private void ShowHaveDungeonActiveSkill()
    {
        string s = "使用するDAS\n";
        for(int i = 0; i < this.allys.Count; i++)
        {
            s += i.ToString() + ":(" + this.allys[i].CharaClass.CharaName + "):" + this.allys[i].PC.HaveDungeonActiveSkillID.ToString() + "\n";
        }
        this.dungeonUIManager.AnnounceByText(s);
    }

    /// <summary>
    /// 番号入力により、BAS発動
    /// </summary>
    private void InputInvokeDungeonActiveSkill()
    {
        for(int i = 0; i < this.allys.Count; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                this.dungeonActiveEffectsFunc.GetSkill(this.allys[i].PC.HaveDungeonActiveSkillID).EffectFunc(this);
            }
        }
    }
    private void GenerateFloor(int rowSize, int columnSize)
    {
        this.currentFloorDungeonSquares = new E_DungeonSquareType[rowSize, columnSize];
        int countOfDungeonSquaresKind = this.mapManager.MayApeearDungeonSquares.Count;
        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < columnSize; j++)
            {
                this.currentFloorDungeonSquares[i, j] = this.mapManager.MayApeearDungeonSquareTypes[UnityEngine.Random.Range(0, countOfDungeonSquaresKind)];
            }
        }
    }

    /// <summary>
    /// ランダムでひとつ悪い効果適用
    /// </summary>
    public void ActivateRandomBadEffect()
    {
        switch(UnityEngine.Random.Range(0, 5))
        {
            case 0:
                Debug.Log("チームHp半減");
                foreach(BattleCharacter bc in this.allys)
                {
                    bc.Hp /= 2;
                }
                break;
            case 1:
                Debug.Log("マップ忘却");
                this.mapManager.SetFlagUnderstandDungeonSquareType(this.understandDungeonSquareType, false);
                break;
            case 2:
                Debug.Log("満腹度半減");
                this.Fullness /= 2;
                break;
            case 3:
                Debug.Log("悪いアイテム獲得");
                this.haveBattlePassiveItems.Add(this.badItem); //参照渡しだからこれはだめか？
                break;
            case 4:
                Debug.Log("Dsp3減少");
                ChangeAllDsp(-3);
                break;
        }
    }

    /// <summary>
    /// ランダムでひとつ良い効果発動
    /// </summary>
    public void ActivateRandomGoodEffect()
    {
        switch (UnityEngine.Random.Range(0, 5))
        {
            case 0:
                Debug.Log("チームHp全回復");
                foreach(BattleCharacter bc in this.allys)
                {
                    bc.RecoverHpByRate(1);
                }
                break;
            case 1:
                Debug.Log("マップ全体可視化");
                this.mapManager.SetFlagUnderstandDungeonSquareType(this.understandDungeonSquareType, true);
                break;
            case 2:
                Debug.Log("満腹度全回復");
                RecoverFullnessByRate(1);
                break;
            case 3:
                Debug.Log("良いアイテム獲得");
                this.haveBattlePassiveItems.Add(this.goodItem); //参照渡しだから、これではだめか？
                break;
            case 4:
                Debug.Log("全キャラDungeonActiveSkill使用可能");
                SetUseableAllDungeonActiveSkill();
                break;
        }
    }

    /// <summary>
    /// 満腹度を固定値回復。
    /// </summary>
    /// <param name="recoverValue">回復する値</param>
    public void RecoverFullness(int recoverValue)
    {
        if(this.Fullness + recoverValue > this.MaxFullNess)
        {
            Debug.Log("満腹度が最大になった");
            this.Fullness = this.MaxFullNess;
        }
        else
        {
            Debug.Log("満腹度が" + recoverValue + "回復した");
            this.Fullness += recoverValue;
        }
    }
    
    /// <summary>
    /// 満腹度を割合回復
    /// </summary>
    /// <param name="recoverRate">最大満腹度に対して、回復する割合(最大1)</param>
    public void RecoverFullnessByRate(double recoverRate)
    {
        RecoverFullness((int)(MaxFullNess * recoverRate));
    }

    /// <summary>
    /// 全てのDungeonActiveSkillを使用可能にする
    /// </summary>
    public void SetUseableAllDungeonActiveSkill()
    {
        for(int i = 0; i < this.haveDsp.Count; i++)
        {
            this.haveDsp[i] = this.needDsp[i];
        }
    }

    /// <summary>
    /// 全キャラのDspを変更する
    /// </summary>
    /// <param name="addValue">加える値</param>
    public void ChangeAllDsp(int addValue)
    {
        for(int i = 0; i < this.haveDsp.Count; i++)
        {
            if (this.haveDsp[i] + addValue > this.needDsp[i]) this.haveDsp[i] = this.needDsp[i];
            else if (this.haveDsp[i] + addValue < 0) this.haveDsp[i] = 0;
            else this.haveDsp[i] += addValue;
        }
    }

    /// <summary>
    /// 罠にかかったときの処理(とりあえず全種類からランダムで)
    /// </summary>
    public void GetTrapped()
    {
        switch(UnityEngine.Random.Range(1, 12))
        {
            case 1:
                Debug.Log("小ダメージ");
                DamageAllAllyByRate(0.25); //1/4ダメージ;
                break;
            case 2:
                Debug.Log("中ダメージ");
                DamageAllAllyByRate(0.5); //1/2ダメージ;
                break;
            case 3:
                Debug.Log("大ダメージ");
                foreach(BattleCharacter bc in this.allys)
                {
                    bc.Hp = 1;
                }
                break;
            case 4:
                Debug.Log("アイテムをひとつ失った");
                LoseRandomOneItem();
                break;
            case 5:
                Debug.Log("満腹度半減");
                this.Fullness /= 2;
                break;
            case 6:
                Debug.Log("ワープ");
                WarpToRandomPosition();
                break;
            case 7:
                Debug.Log("Dsp減少");
                ChangeAllDsp(-3);
                break;
            case 8:
                Debug.Log("マップ可視範囲3ターン減少");
                this.FieldOfVisionNarrowTurn = 3;
                break;
            case 9:
                Debug.Log("マップ忘却");
                this.mapManager.SetFlagUnderstandDungeonSquareType(this.understandDungeonSquareType, false);
                break;
            case 10:
                Debug.Log("デメリットアイテム獲得");
                this.haveBattlePassiveItems.Add(this.badItem); //参照渡しだからだめかも
                break;
            case 11:
                Debug.Log("DungeonActiveSkillが3ターン使用不可");
                this.EnclosedUseDungeonActiveSkillTurn = 3;
                break;

        }
    }

    /// <summary>
    /// 全ての味方キャラの現在Hpに対する割合ダメージ
    /// </summary>
    /// <param name="rateOfHp">現在Hpに対するダメージ割合(最大1)</param>
    public void DamageAllAllyByRate(double rateOfHp)
    {
        foreach(BattleCharacter bc in this.allys)
        {
            bc.Hp -= bc.Hp * rateOfHp;
        }
    }

    /// <summary>
    /// ランダムでアイテムひとつ失う(4リスト最低1アイテム保持前提)
    /// </summary>
    public void LoseRandomOneItem()
    {
        switch(UnityEngine.Random.Range(0, 4))
        {
            case 0:
                this.haveDungeonActiveItems.RemoveAt(UnityEngine.Random.Range(0, this.haveDungeonActiveItems.Count));
                break;
            case 1:
                this.haveDungeonPassiveItems.RemoveAt(UnityEngine.Random.Range(0, this.haveDungeonPassiveItems.Count));
                break;
            case 2:
                this.haveBattleActiveItems.RemoveAt(UnityEngine.Random.Range(0, this.haveBattleActiveItems.Count));
                break;
            case 3:
                this.haveBattlePassiveItems.RemoveAt(UnityEngine.Random.Range(0, this.haveBattlePassiveItems.Count));
                break;
        }
    }

    /// <summary>
    /// マップ上のランダムなマスにワープ(現在位置移動)。壁かどうかの判断はまだしていない
    /// </summary>
    public void WarpToRandomPosition()
    {
        this.CurrentLocationRow = UnityEngine.Random.Range(0, this.mapManager.MapWidth);
        this.CurrentLocationColumn = UnityEngine.Random.Range(0, this.mapManager.MapHeight);
    }

    /// <summary>
    /// マス移動処理(とりあえず、自分の入力で上下左右にマス移動できるようにする)
    /// </summary>
    public void MoveDungeonSquare(E_Direction direction)
    {
        switch (direction)
        {
            case E_Direction.Up:
                if (CurrentLocationRow < 1) return;
                else CurrentLocationRow--;
                break;
            case E_Direction.Right:
                if (CurrentLocationColumn > this.mapManager.MapWidth - 2) return;
                else CurrentLocationColumn++;
                break;
            case E_Direction.Down:
                if (CurrentLocationRow > this.mapManager.MapHeight - 2) return;
                else CurrentLocationRow++;
                break;
            case E_Direction.Left:
                if (CurrentLocationColumn < 1) return;
                else CurrentLocationColumn--;
                break;
        }
        this.WaitDungeonSquareEvent = true;
        Debug.Log("row/column = " + CurrentLocationRow + "/" + CurrentLocationColumn);
    }

    public void SetChangedDiceEyes(int[] diceEyesToChange)
    {
        this.changedDice = diceEyesToChange;
        string s = "";
        for (int i = 0; i < 6; i++)
        {
            s += diceEyesToChange[i].ToString() + ",";
        }
        Debug.Log("次のサイコロの目を" + s + "に変更");
    }

    public void ChangeDungeonSquareType(List<E_DungeonSquareType> targetType, E_DungeonSquareType afterChangeDungeonSquareType, int effectRange)
    {
        //targetTypeに一致するマスからひとつ選択する処理(または戻る)
        //選択したマスをafterChangeのマスに変化させる処理
    }

    public void AddHaveItem(E_DungeonActiveItem itemId)
    {
        this.haveDungeonActiveItems.Add(this.dungeonActiveEffectsFunc.GetItem(itemId));
    }
    public void AddHaveItem(E_DungeonPassiveItem itemId)
    {
        this.haveDungeonPassiveItems.Add(this.dungeonPassiveEffectsFunc.GetItem(itemId));
    }
    public void AddHaveItem(E_BattleActiveItem itemId)
    {
        this.haveBattleActiveItems.Add(this.battleActiveEffectsFunc.GetItem(itemId));
    }
    public void AddHaveItem(E_BattlePassiveItem itemId)
    {
        this.haveBattlePassiveItems.Add(this.battlePassiveEffectsFunc.GetItem(itemId));
    }

    /// <summary>
    /// targetAllysのBspを増加させる
    /// </summary>
    /// <param name="increaseValue">Bsp増加量</param>
    public void IncreaseBsp(int increaseValue)
    {
        foreach(BattleCharacter bc in this.targetAllys)
        {
            bc.AddBsp(increaseValue);
        }
    }

    /// <summary>
    /// targetAllysのDspを増加させる
    /// </summary>
    /// <param name="increaseValue">Dsp増加量</param>
    public void IncreaseDsp(int increaseValue)
    {
        foreach(BattleCharacter bc in this.targetAllys)
        {
            bc.Dsp += increaseValue;
        }
    }

    /// <summary>
    /// targetAllysのHpを回復させる
    /// </summary>
    /// <param name="recoverRateOrValue">回復量(1以下で割合回復)</param>
    public void RecoverHp(double recoverRateOrValue)
    {
        if(recoverRateOrValue > 1)
        {
            foreach(BattleCharacter bc in this.targetAllys)
            {
                bc.RecoverHp(recoverRateOrValue);
            }
        }
        else
        {
            foreach(BattleCharacter bc in this.targetAllys)
            {
                bc.RecoverHpByRate(recoverRateOrValue);
            }
        }
    }

    public void SetFlagUnderstandDungeonSquareType(bool flag)
    {
        this.mapManager.SetFlagUnderstandDungeonSquareType(this.understandDungeonSquareType, flag);
    }
}

public enum E_Direction
{
    Up,
    Right,
    Down,
    Left
}