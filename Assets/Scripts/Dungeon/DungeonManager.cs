using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private GameObject allysRootObject;
    [SerializeField] private GameObject enemysRootObject;

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

    private List<BattleCharacter> targetAllys = null; //効果対象にする味方記憶用
    private int currentIndexOfTargetableAllys = 0; //現在選んでいるtargetableAllysの番地
    private List<PositionXY> targetableDungeonSquares = null; //効果対象にできるマスの座標
    private List<PositionXY> targetDungeonSquares = null; //効果対象にするマスの座標
    private int currentIndexOfTargetableDungeonSquares = 0; //現在選んでいるtargetableDungeonSquaresの番地
    private bool isFinishFirstSet = false; //ダンジョン潜入直後のセットが終わったかどうか

    /// <summary>
    /// ActiveEffect発動前記憶用
    /// </summary>
    private DungeonActiveEffect waitActiveEffect;

    /// <summary>
    /// 満腹度(マス移動する度に減少?)
    /// </summary>
    public int Fullness { get; private set; } = 100;

    /// <summary>
    /// 最大満腹度(最大150???)
    /// </summary>
    public int MaxFullNess { get; private set; } = 100;

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
    /// 現在乗っているマスのタイプ
    /// </summary>
    public E_DungeonSquareType CurrentOnDungeonSquareType => this.currentFloorDungeonSquares[this.CurrentLocationRow, this.CurrentLocationColumn];

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
    public int FieldOfVision { get; set; } = 3;

    /// <summary>
    /// 可視範囲減少効果の残りターン
    /// </summary>
    public int FieldOfVisionNarrowTurn { get; set; } = 0;

    /// <summary>
    /// 獲得経験値の倍率(最大2倍)
    /// </summary>
    public double GetExpRate { get; private set; } = 1;

    /// <summary>
    /// 獲得Goldの倍率(最大2倍)
    /// </summary>
    public double GetGoldRate { get; private set; } = 1;

    /// <summary>
    /// 敵ドロップ率(最大2倍)
    /// </summary>
    public double EnemyDropRate { get; private set; } = 1;

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
    /// レア敵出現率。Max0.5???
    /// </summary>
    public double AppearanceRareEnemyRate { get; private set; } = 0.1;

    /// <summary>
    /// レアアイテム販売率。Max0.5???
    /// </summary>
    public double ShopRareGoodsRate { get; private set; } = 0.1;

    /// <summary>
    /// 店の商品数(最大8???)
    /// </summary>
    public int ShopGoodsNum { get; private set; } = 4;

    /// <summary>
    /// マスイベントの出現率(初期値1、最大2?)
    /// </summary>
    public Dictionary<E_DungeonSquareType, double> SquareEventAppearanceRate { get; private set; }

    /// <summary>
    /// 宝箱解除成功率(初期値0.5, 最大1?)
    /// </summary>
    public double SuccessRateOfUnlockTreasureChest { get; private set; } = 0.5;

    /// <summary>
    /// 罠回避率。Max0.9???
    /// </summary>
    public double EvadeTrapRate { get; private set; } = 0.2;

    /// <summary>
    /// マス移動量増加量(基本0)
    /// </summary>
    public int MovementIncreaseValue { get; set; } = 0;

    /// <summary>
    /// マス移動量の倍率(基本1倍)
    /// </summary>
    public int MovementRate { get; set; } = 1;

    /// <summary>
    /// 残りの移動量(一次記憶用)
    /// </summary>
    public int RemainingAmountOfMovement { get; set; } = 0;

    /// <summary>
    /// enemyオブジェクトをシーン間記憶用親オブジェクト
    /// </summary>
    public GameObject EnemysRootObject => this.enemysRootObject;


    public List<DungeonActiveItem> HaveDungeonActiveItems => this.haveDungeonActiveItems;
    public List<DungeonPassiveItem> HaveDungeonPassiveItems => this.haveDungeonPassiveItems;
    public List<BattleActiveItem> HaveBattleActiveItems => this.haveBattleActiveItems;
    public List<BattlePassiveItem> HaveBattlePassiveItems => this.haveBattlePassiveItems;


    /// <summary>
    /// DungeonSquareイベントの処理待ちかどうか
    /// </summary>
    public bool WaitDungeonSquareEvent { get; set; } = false;

    /// <summary>
    /// 移動している方向
    /// </summary>
    public E_Direction CurrentMovingDirection { get; set; } = E_Direction.None;

    /// <summary>
    /// アナウンスが必要か(必要ならtrue)
    /// </summary>
    public bool NeedAnnounce { get; set; } = true;

    /// <summary>
    /// 出現した敵保存用
    /// </summary>
    public List<BattleCharacter> Enemys { get; set; } = new List<BattleCharacter>();

    public static DungeonManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);


    }

    private void Start()
    {
        //this.mapManager.GenerateFloor(this.currentFloorDungeonSquares);
        if (!isFinishFirstSet)
        {
            Debug.Log(this.mapManager.MapHeight + ":" + this.mapManager.MapWidth);
            this.GenerateFloor(this.mapManager.MapWidth, this.mapManager.MapHeight);
            this.dungeonSquaresFunc.SetMayApeearDungeonSquares(this.mapManager.MayApeearDungeonSquares);
            this.MoveScene(E_DungeonScene.SelectAction);
            FirstSet();
            SetFlagUnderstandDungeonSquareType(true);

            HaveGold = 5000000;
            this.isFinishFirstSet = true;
        }
    }

    private void Update()
    {
        if (this.currentScene == E_DungeonScene.Battle) return;
        ActionInCurrentScene();
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            InputBack();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ViewMap();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("現在いる座標: [" + this.CurrentLocationRow + "," + this.CurrentLocationColumn + "]");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("所持G: " + HaveGold);
        }
    }

    

    /// <summary>
    /// 各データの初期化。Startで呼ぶ.
    /// </summary>
    private void FirstSet()
    {
        this.understandDungeonSquareType = new bool[this.mapManager.MapWidth, this.mapManager.MapHeight];
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
                if (NeedAnnounce) AnnounceByText("0.サイコロを投げる, 1.アイテム使用, 2.スキル使用, 3.マップ確認, 4.パーティ確認");
                InputSelectAction();
                break;
            case E_DungeonScene.SelectDAI:
                if (NeedAnnounce) ShowHaveDungeonActiveItem();
                InputUseDungeonActiveItem();
                break;
            case E_DungeonScene.SelectDAS:
                if (NeedAnnounce) ShowHaveDungeonActiveSkill();
                InputInvokeDungeonActiveSkill();
                break;
            case E_DungeonScene.SelectDAITargetToAlly:
            case E_DungeonScene.SelectDASTargetToAlly:
                if (NeedAnnounce) AnnounceByText("現在選択中の味方: " + allys[currentIndexOfTargetableAllys].CharaClass.CharaName);
                InputTargetToAlly();
                break;
            case E_DungeonScene.SelectDAITargetToDungeonSquare:
            case E_DungeonScene.SelectDASTargetToDungeonSquare:
                if (NeedAnnounce)
                {
                    ShowTargetableDungeonSquares();
                    AnnounceByText("現在選択中のマス:[" + targetableDungeonSquares[0].Row + "," + targetableDungeonSquares[0].Column + "]" + DungeonManager.GetStringDungeonSquareType(currentFloorDungeonSquares[targetableDungeonSquares[0].Row, targetableDungeonSquares[0].Column]));
                }
                InputTargetToDungeonSquare();
                break;
            case E_DungeonScene.InvokeActiveEffect:
                this.waitActiveEffect.EffectFunc(this);
                this.waitActiveEffect = null;
                this.MoveScene(E_DungeonScene.SelectAction);
                break;
            case E_DungeonScene.MovingDungeonSquare:
                MoveDungeonSquare();
                break;
            case E_DungeonScene.SelectMoveDirection:
                InputMoveDirection();
                break;
            case E_DungeonScene.ViewAllyStatus:
                ShowAllysStatus();
                this.MoveScene(E_DungeonScene.SelectAction);
                break;
            case E_DungeonScene.ViewMap:
                ViewMap();
                this.MoveScene(E_DungeonScene.SelectAction);
                break;
            case E_DungeonScene.WaitDungeonSquareEvent:
                //マスイベント処理
                this.dungeonSquaresFunc.DungeonSquareEvent(this, this.currentFloorDungeonSquares[CurrentLocationRow, CurrentLocationColumn]);
                //this.MoveScene(E_DungeonScene.SelectAction);
                break;
        }
    }

    /// <summary>
    /// 現在の状態を変化させる
    /// </summary>
    /// <param name="toMoveScene">移動先の状態</param>
    public void MoveScene(E_DungeonScene toMoveScene)
    {
        this.currentScene = toMoveScene;
        switch (toMoveScene)
        {
            case E_DungeonScene.SelectAction:
            case E_DungeonScene.SelectDAI:
            case E_DungeonScene.SelectDAITargetToDungeonSquare:
            case E_DungeonScene.SelectDAS:
            case E_DungeonScene.SelectDASTargetToDungeonSquare:
            case E_DungeonScene.SelectMoveDirection:
                this.NeedAnnounce = true;
                break;
            case E_DungeonScene.SelectDASTargetToAlly:
            case E_DungeonScene.SelectDAITargetToAlly:
                this.NeedAnnounce = true;
                this.currentIndexOfTargetableAllys = 0;
                break;
        }
    }
    public void AnnounceByText(string announceText)
    {
        this.dungeonUIManager.AnnounceByText(announceText);
        this.NeedAnnounce = false;
    }

    /// <summary>
    /// 戦闘終了後の初期化処理、シーン遷移
    /// </summary>
    public void FinishBattle()
    {
        //敵オブジェクト削除
        foreach(Transform child in this.enemysRootObject.transform)
        {
            Destroy(child.gameObject);
        }
        this.Enemys = new List<BattleCharacter>();

        StartCoroutine(DelayMethod(1, () =>
        {
            this.dungeonUIManager.LoadDungeonScene();
            this.MoveScene(E_DungeonScene.SelectAction);
        }));
    }

    /// <summary>
    /// 行動選択の入力受付(0:サイコロ, 1:アイテム使用, 2:DAS発動, 3:マップ確認, 4:パーティ確認)
    /// </summary>
    private void InputSelectAction()
    {
        if (Input.GetKeyDown(((int)E_DungeonPlayerSelect.RollDice).ToString()))
        {
            //サイコロ投げに遷移
            RollDice();
        }else if (Input.GetKeyDown(((int)E_DungeonPlayerSelect.UseDungeonActiveItem).ToString()))
        {
            //DAI使用に遷移
            MoveScene(E_DungeonScene.SelectDAI);
            this.NeedAnnounce = true;
        }else if (Input.GetKeyDown(((int)E_DungeonPlayerSelect.InvokeDungeonActiveSkill).ToString()))
        {
            //DAS発動に遷移
            this.MoveScene(E_DungeonScene.SelectDAS);
            this.NeedAnnounce = true;
        }else if (Input.GetKeyDown(((int)E_DungeonPlayerSelect.VerificateMap).ToString()))
        {
            //マップ確認に遷移
            this.MoveScene(E_DungeonScene.ViewMap);
        }else if (Input.GetKeyDown(((int)E_DungeonPlayerSelect.VerificateAlly).ToString()))
        {
            //パーティ確認に遷移
            this.MoveScene(E_DungeonScene.ViewAllyStatus);
        }
    }

    /// <summary>
    /// 戻るボタン押したときの処理
    /// </summary>
    public void InputBack()
    {
        switch (this.currentScene)
        {
            case E_DungeonScene.SelectDAI:
            case E_DungeonScene.SelectDAS:
                this.MoveScene(E_DungeonScene.SelectAction);
                break;
            case E_DungeonScene.SelectDAITargetToAlly:
            case E_DungeonScene.SelectDAITargetToDungeonSquare:
                this.MoveScene(E_DungeonScene.SelectDAI);
                break;
            case E_DungeonScene.SelectDASTargetToAlly:
            case E_DungeonScene.SelectDASTargetToDungeonSquare:
                this.MoveScene(E_DungeonScene.SelectDAS);
                break;
        }
    }
    /// <summary>
    /// 味方パーティの状態確認
    /// </summary>
    private void ShowAllysStatus()
    {
        string s = "味方の状態\n";
        foreach(BattleCharacter bc in this.allys)
        {
            s += bc.CharaClass.CharaName + ". HP:" + bc.Hp + "/" + bc.MaxHp + " Dsp:" + bc.Dsp + "\n";
        }
        this.AnnounceByText(s);
    }

    /// <summary>
    /// 現在いるフロアのマップを確認
    /// </summary>
    private void ViewMap()
    {
        string s = "マップ状態\n";
        for(int i = 0; i < this.mapManager.MapHeight; i++)
        {
            for(int j = 0; j < this.mapManager.MapWidth; j++)
            {
                if (understandDungeonSquareType[i, j]) s += DungeonManager.GetStringDungeonSquareType(this.currentFloorDungeonSquares[i, j]) + " ";
                else s += "? ";
            }
            s += "\n";
        }
        //this.AnnounceByText(s);
        Debug.Log(s);
    }

    /// <summary>
    /// レア敵出現率を増加させる。とりあえず最大0.5
    /// </summary>
    /// <param name="increaseRate">増加割合(0.1など)</param>
    public void IncreaseAppearanceRareEnemyRate(double increaseRate)
    {
        Debug.Log("レア敵出現率" + increaseRate + "増加");
        this.AppearanceRareEnemyRate += increaseRate;
        if (this.AppearanceRareEnemyRate > 0.5) this.AppearanceRareEnemyRate = 0.5;
    }

    /// <summary>
    /// 罠回避率を増加させる。とりあえず最大0.9
    /// </summary>
    /// <param name="increaseRate">増加割合(0.1など)</param>
    public void IncreaseEvadeTrapRate(double increaseRate)
    {
        Debug.Log("罠回避率" + increaseRate + "増加");
        this.EvadeTrapRate += increaseRate;
        if (this.EvadeTrapRate > 0.9) this.EvadeTrapRate = 0.9;
    }

    /// <summary>
    /// 獲得経験値倍率を増加させるとりあえず最大2倍
    /// </summary>
    /// <param name="increaseRate">増加割合(0.1など)</param>
    public void IncreaseGetExpRate(double increaseRate)
    {
        Debug.Log("獲得経験値" + increaseRate + "増加");
        this.GetExpRate += increaseRate;
        if (this.GetExpRate > 2) this.GetExpRate = 2;
    }

    /// <summary>
    /// 獲得経験値倍率を増加させるとりあえず最大2倍
    /// </summary>
    /// <param name="increaseRate">増加割合(0.1など)</param>
    public void IncreaseGetGoldRate(double increaseRate)
    {
        Debug.Log("獲得経験値" + increaseRate + "増加");
        this.GetGoldRate += increaseRate;
        if (this.GetGoldRate > 2) this.GetGoldRate = 2;
    }

    /// <summary>
    /// 最大満腹度増加(とりあえず最大150)
    /// </summary>
    /// <param name="increaseValue">増加値</param>
    public void IncreaseMaxFullness(int increaseValue)
    {
        Debug.Log("最大満腹度" + increaseValue + "増加");
        this.MaxFullNess += increaseValue;
        if (this.MaxFullNess > 150) this.MaxFullNess = 150;
    }

    /// <summary>
    /// 敵ドロップ率増加(とりあえず最大2倍)
    /// </summary>
    /// <param name="increaseRateValue">増加割合(0.1など)</param>
    public void IncreaseEnemyDropRate(double increaseRateValue)
    {
        Debug.Log("敵ドロップ率" + increaseRateValue + "増加");
        this.EnemyDropRate += increaseRateValue;
        if (this.EnemyDropRate > 2) this.EnemyDropRate = 2;
    }

    /// <summary>
    /// 店レアアイテム販売率増加(とりあえず最大0,5)
    /// </summary>
    /// <param name="increaseRateValue">増加割合(0.1など)</param>
    public void IncreaseShopRareGoodsRate(double increaseRateValue)
    {
        Debug.Log("店レアアイテム販売率" + increaseRateValue + "増加");
        this.ShopRareGoodsRate += increaseRateValue;
        if (this.ShopRareGoodsRate > 0.5) this.ShopRareGoodsRate = 0.5;
    }

    /// <summary>
    /// 店商品数増加(とりあえず最大8)
    /// </summary>
    /// <param name="increaseValue">商品増加数(基本1)</param>
    public void IncreaseShopGoodsNum(int increaseValue)
    {
        Debug.Log("店商品数" + increaseValue + "増加");
        this.ShopGoodsNum += increaseValue;
        if (this.ShopGoodsNum > 8) this.ShopGoodsNum = 8;
    }

    /// <summary>
    /// 特定のマスイベント発生率増加(とりあえず最大2)
    /// </summary>
    /// <param name="squareType">発生率を増やすマスイベント</param>
    /// <param name="increaseRateValue">増加割合(0.1など)</param>
    public void IncreaseSquareEventAppearanceRate(E_DungeonSquareType squareType, double increaseRateValue)
    {
        Debug.Log(squareType.ToString() + "イベント発生率" + increaseRateValue + "増加");
        this.SquareEventAppearanceRate[squareType] += increaseRateValue;
        if (this.SquareEventAppearanceRate[squareType] > 2) this.SquareEventAppearanceRate[squareType] = 2;
    }

    /// <summary>
    /// 宝箱解除率を増加させる(とりあえず最大2)
    /// </summary>
    /// <param name="increaseRateValue">増加割合(0.1など)</param>
    public void IncreaseSuccessRateOfUnlockTreasureChest(double increaseRateValue)
    {
        Debug.Log("宝箱解除率" + increaseRateValue + "増加");
        this.SuccessRateOfUnlockTreasureChest += increaseRateValue;
        if (this.SuccessRateOfUnlockTreasureChest > 2) this.SuccessRateOfUnlockTreasureChest = 2;
    }
    /// <summary>
    /// マスタイプを簡略化した文字を返す
    /// </summary>
    /// <param name="dungeonSquareType">文字を得るマスタイプ</param>
    /// <returns>マスタイプを簡略化した文字</returns>
    public static string GetStringDungeonSquareType(E_DungeonSquareType dungeonSquareType)
    {
        switch (dungeonSquareType)
        {
            case E_DungeonSquareType.なにもなし:
                return "無";
            case E_DungeonSquareType.ボス戦:
                return "ボ";
            case E_DungeonSquareType.ランダム:
                return "ラ";
            case E_DungeonSquareType.呪いの宝箱:
                return "呪";
            case E_DungeonSquareType.回復の泉:
                return "泉";
            case E_DungeonSquareType.壁:
                return "壁";
            case E_DungeonSquareType.宝箱:
                return "宝";
            case E_DungeonSquareType.店:
                return "店";
            case E_DungeonSquareType.祈祷:
                return "祈";
            case E_DungeonSquareType.罠:
                return "罠";
            case E_DungeonSquareType.通常戦闘:
                return "戦";
            case E_DungeonSquareType.闇商人:
                return "闇";
            case E_DungeonSquareType.階段:
                return "階";
            case E_DungeonSquareType _:
                return "error";
        }
    }
    private void RollDice()
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
        this.AnnounceByText(s);
        
        this.AnnounceByText("出た目:" + diceEye.ToString());
        if(MovementIncreaseValue > 0 || MovementRate > 1)
        {
            this.RemainingAmountOfMovement = (diceEye + MovementIncreaseValue) * MovementRate;
            this.AnnounceByText("(効果適用)目の数: " + RemainingAmountOfMovement);
        }
        else
        {
            this.RemainingAmountOfMovement = diceEye;
        }
        this.MovementRate = 1;
        this.MovementIncreaseValue = 0;
        this.changedDice = null;
        this.MoveScene(E_DungeonScene.MovingDungeonSquare); //移動に遷移
    }

    

    /// <summary>
    /// 所持DAIの使用(今は0~9までキーボード入力)
    /// </summary>
    private void InputUseDungeonActiveItem()
    {
        int itemNum = this.haveDungeonActiveItems.Count;
        for(int i = 0; i < itemNum && i < 9; i++) //とりあえず9以下まで(10以上の入力ができないため)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                UseDungeonActiveItem(this.haveDungeonActiveItems[i]);
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
        this.AnnounceByText(s);
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
        this.AnnounceByText(s);
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
                DungeonActiveSkill skill = this.dungeonActiveEffectsFunc.GetSkill(this.allys[i].PC.HaveDungeonActiveSkillID);
                if (skill.NeedDsp > this.allys[i].Dsp) continue;
                InvokeDungeonActiveSkill(skill, this.allys[i]);
            }
        }
    }

    /// <summary>
    /// DAIを使用する。対象選択があれば対象選択へ移動。なければ発動。
    /// </summary>
    /// <param name="item">使用するDAI</param>
    private void UseDungeonActiveItem(DungeonActiveItem item)
    {
        switch (item.DungeonActiveEffectTargetType)
        {
            case E_DungeonActiveEffectTargetType.Other:
                item.EffectFunc(this);
                MoveScene(E_DungeonScene.SelectAction);
                break;
            case E_DungeonActiveEffectTargetType.AllAlly:
                this.targetAllys = this.allys;
                item.EffectFunc(this);
                MoveScene(E_DungeonScene.SelectAction);
                break;
            case E_DungeonActiveEffectTargetType.AllDungeonSquare:
                SetTargetableDungeonSquare(item.TargetDungeonSquareTypes, item.EffectRange);
                this.targetDungeonSquares = this.targetableDungeonSquares;
                item.EffectFunc(this);
                MoveScene(E_DungeonScene.SelectAction);
                break;
            case E_DungeonActiveEffectTargetType.OneAlly:
                this.waitActiveEffect = item;
                MoveScene(E_DungeonScene.SelectDAITargetToAlly);
                break;
            case E_DungeonActiveEffectTargetType.OneDungeonSquare:
                this.waitActiveEffect = item;
                SetTargetableDungeonSquare(item.TargetDungeonSquareTypes, item.EffectRange);
                MoveScene(E_DungeonScene.SelectDAITargetToDungeonSquare);
                break;
            case E_DungeonActiveEffectTargetType.Error:
                throw new Exception();

        }
    }

    /// <summary>
    /// DASをinvokerが発動する。対象選択があれば対象選択へ移動。なければ発動
    /// </summary>
    /// <param name="skill">発動するDAS</param>
    /// <param name="invoker">発動者</param>
    private void InvokeDungeonActiveSkill(DungeonActiveSkill skill, BattleCharacter invoker)
    {
        switch (skill.DungeonActiveEffectTargetType)
        {
            case E_DungeonActiveEffectTargetType.Other:
                skill.EffectFunc(this);
                MoveScene(E_DungeonScene.SelectAction);
                break;
            case E_DungeonActiveEffectTargetType.AllAlly:
                this.targetAllys = this.allys;
                skill.EffectFunc(this);
                MoveScene(E_DungeonScene.SelectAction);
                break;
            case E_DungeonActiveEffectTargetType.AllDungeonSquare:
                SetTargetableDungeonSquare(skill.TargetDungeonSquareTypes, skill.EffectRange);
                this.targetDungeonSquares = this.targetableDungeonSquares;
                skill.EffectFunc(this);
                MoveScene(E_DungeonScene.SelectAction);
                break;
            case E_DungeonActiveEffectTargetType.OneAlly:
                this.waitActiveEffect = skill;
                MoveScene(E_DungeonScene.SelectDASTargetToAlly);
                break;
            case E_DungeonActiveEffectTargetType.OneDungeonSquare:
                this.waitActiveEffect = skill;
                SetTargetableDungeonSquare(skill.TargetDungeonSquareTypes, skill.EffectRange);
                MoveScene(E_DungeonScene.SelectDASTargetToDungeonSquare);
                break;
            case E_DungeonActiveEffectTargetType.SelfAlly:
                this.targetAllys = new List<BattleCharacter>() { invoker };
                skill.EffectFunc(this);
                MoveScene(E_DungeonScene.SelectAction);
                break;
            case E_DungeonActiveEffectTargetType.Error:
                throw new Exception();
             
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

    public void MoveBattleScene()
    {
        this.currentScene = E_DungeonScene.Battle;
        this.dungeonUIManager.AnnounceText.text = "";
        SceneManager.LoadScene("TestBattle");
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
                this.mapManager.SetFlagUnderstandDungeonSquareType(ref this.understandDungeonSquareType, false);
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
                //マップ全体可視化
                this.mapManager.SetFlagUnderstandDungeonSquareType(ref this.understandDungeonSquareType, true);
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
                Debug.Log("アイテム消失の罠");
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
                this.mapManager.SetFlagUnderstandDungeonSquareType(ref this.understandDungeonSquareType, false);
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
    /// ランダムでアイテムひとつ失う
    /// </summary>
    public void LoseRandomOneItem()
    {
        switch(UnityEngine.Random.Range(0, 4))
        {
            case 0:
                try
                {
                    this.haveDungeonActiveItems.RemoveAt(UnityEngine.Random.Range(0, this.haveDungeonActiveItems.Count));
                }
                catch (ArgumentOutOfRangeException)
                {
                    Debug.Log("DAIをひとつも持っていなかった");
                }
                break;
            case 1:
                try
                {
                    this.haveDungeonPassiveItems.RemoveAt(UnityEngine.Random.Range(0, this.haveDungeonPassiveItems.Count));
                }
                catch (ArgumentOutOfRangeException)
                {
                    Debug.Log("DPIをひとつも持っていなかった");
                }
                break;
            case 2:
                try
                {
                    this.haveBattleActiveItems.RemoveAt(UnityEngine.Random.Range(0, this.haveBattleActiveItems.Count));
                }
                catch (ArgumentOutOfRangeException)
                {
                    Debug.Log("BAIをひとつも持っていなかった");
                }
                break;
            case 3:
                try
                {
                    this.haveBattlePassiveItems.RemoveAt(UnityEngine.Random.Range(0, this.haveBattlePassiveItems.Count));
                }
                catch (ArgumentOutOfRangeException)
                {
                    Debug.Log("BPIをひとつも持っていなかった");
                }
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
        this.CurrentMovingDirection = E_Direction.None;
    }

    /// <summary>
    /// 移動先が分岐しているかどうか(現在の進行方向から左右に曲がる道があるかどうか)
    /// </summary>
    /// <param name="currentMoveDirection">現在の進行方向</param>
    /// <returns>分岐しているならtrue、していないならfalse</returns>
    private bool IsJunction(E_Direction currentMoveDirection)
    {
        try
        {
            switch (currentMoveDirection)
            {
                case E_Direction.Up:
                case E_Direction.Down:
                    return AbleMoveDirection(E_Direction.Right) || AbleMoveDirection(E_Direction.Left);
                case E_Direction.Right:
                case E_Direction.Left:
                    return AbleMoveDirection(E_Direction.Up) || AbleMoveDirection(E_Direction.Down);
                case E_Direction.None:
                    return true;
                case E_Direction _:
                    return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// 現在いるマスからsearchDirection方向に移動できるかどうか
    /// </summary>
    /// <param name="searchDirection">移動できるか確かめる方向</param>
    /// <returns>searchDirection方向に移動できたらtrue</returns>
    private bool AbleMoveDirection(E_Direction searchDirection)
    {
        try
        {
            switch (searchDirection)
            {
                case E_Direction.Up:
                    return CurrentLocationRow > 0 && currentFloorDungeonSquares[CurrentLocationRow - 1, CurrentLocationColumn] != E_DungeonSquareType.壁;
                case E_Direction.Right:
                    return CurrentLocationColumn < mapManager.MapWidth - 1 && currentFloorDungeonSquares[CurrentLocationRow, CurrentLocationColumn + 1] != E_DungeonSquareType.壁;
                case E_Direction.Left:
                    return CurrentLocationColumn > 0 && currentFloorDungeonSquares[CurrentLocationRow, CurrentLocationColumn - 1] != E_DungeonSquareType.壁;
                case E_Direction.Down:
                    return CurrentLocationRow < mapManager.MapHeight - 1 && currentFloorDungeonSquares[CurrentLocationRow + 1, CurrentLocationColumn] != E_DungeonSquareType.壁;
                case E_Direction _:
                    return false;
            }
        }
        catch(Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// プレイヤーの移動方向入力受付
    /// </summary>
    private void InputMoveDirection()
    {
        if (Input.GetKeyDown(KeyCode.W) && CurrentMovingDirection != E_Direction.Down && AbleMoveDirection(E_Direction.Up))
        {
            this.CurrentMovingDirection = E_Direction.Up;
            MoveDungeonSquare(E_Direction.Up);
            this.MoveScene(E_DungeonScene.MovingDungeonSquare);
        }
        else if (Input.GetKeyDown(KeyCode.D) && CurrentMovingDirection != E_Direction.Left && AbleMoveDirection(E_Direction.Right))
        {
            this.CurrentMovingDirection = E_Direction.Right;
            MoveDungeonSquare(E_Direction.Right);
            this.MoveScene(E_DungeonScene.MovingDungeonSquare);
        }
        else if (Input.GetKeyDown(KeyCode.S) && CurrentMovingDirection != E_Direction.Up && AbleMoveDirection(E_Direction.Down))
        {
            this.CurrentMovingDirection = E_Direction.Down;
            MoveDungeonSquare(E_Direction.Down);
            this.MoveScene(E_DungeonScene.MovingDungeonSquare);
        }
        else if (Input.GetKeyDown(KeyCode.A) && CurrentMovingDirection != E_Direction.Right && AbleMoveDirection(E_Direction.Left))
        {
            this.CurrentMovingDirection = E_Direction.Left;
            MoveDungeonSquare(E_Direction.Left);
            this.MoveScene(E_DungeonScene.MovingDungeonSquare);
        }
    }

    /// <summary>
    /// プレイヤーの移動処理。分岐点に着いたら方向入力へ切り替え
    /// </summary>
    private void MoveDungeonSquare()
    {
        if(RemainingAmountOfMovement < 1)
        {
            //初期化処理?
            this.RemainingAmountOfMovement = 0;
            this.MoveScene(E_DungeonScene.WaitDungeonSquareEvent);
        }else if (IsJunction(this.CurrentMovingDirection))
        {
            Debug.Log("進行方向を選択してください(現在の進行方向: " + this.CurrentMovingDirection + ")"); ;
            this.MoveScene(E_DungeonScene.SelectMoveDirection);
        }
        else
        {
            MoveDungeonSquare(this.CurrentMovingDirection);
        }
    }
    /// <summary>
    /// directionの方向に1マス移動する(残り移動量-1)
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
        //this.WaitDungeonSquareEvent = true;
        this.RemainingAmountOfMovement--;
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

    /// <summary>
    /// targetDungeonSquaresに格納されているマスのタイプをafterに変化させる
    /// </summary>
    /// <param name="afterChangeDungeonSquareType">変化先のマスタイプ</param>
    public void ChangeDungeonSquareType(E_DungeonSquareType afterChangeDungeonSquareType)
    {
        foreach(PositionXY position in this.targetDungeonSquares)
        {
            this.currentFloorDungeonSquares[position.Row, position.Column] = afterChangeDungeonSquareType;
        }
    }

    /// <summary>
    /// マスのターゲットタイプと効果範囲から、効果を適用できるマス(tragetableDungeonSquares)をセット
    /// </summary>
    /// <param name="targetTypes">対象にできるマスの種類</param>
    /// <param name="effectRange">効果範囲</param>
    private void SetTargetableDungeonSquare(List<E_DungeonSquareType> targetTypes, int effectRange)
    {
        this.targetableDungeonSquares = new List<PositionXY>();
        for(int i = CurrentLocationRow - effectRange; i < CurrentLocationRow + effectRange; i++)
        {
            for(int j = CurrentLocationColumn - effectRange + i; Math.Abs(CurrentLocationColumn - j) + Math.Abs(CurrentLocationRow - i) <= effectRange; j++)
            {
                foreach(E_DungeonSquareType dsType in targetTypes)
                {
                    try
                    {
                        if (this.currentFloorDungeonSquares[i, j] == dsType) this.targetableDungeonSquares.Add(new PositionXY(i, j));
                    }
                    catch (IndexOutOfRangeException)
                    {
                        break;
                    }
                }
            }
        }

        if(targetableDungeonSquares.Count < 1)
        {
            AnnounceByText("対象にできるマスがありません\n");
            this.waitActiveEffect = null;
            MoveScene(E_DungeonScene.SelectDAS);
        }
        else
        {
            this.currentIndexOfTargetableDungeonSquares = 0;
        }
    }

    private void ShowTargetableDungeonSquares()
    {
        string s = "対象にできるマス\n";
        foreach(PositionXY position in this.targetableDungeonSquares)
        {
            s += "[" + position.Row + "," +  position.Column + "]" +  DungeonManager.GetStringDungeonSquareType(this.currentFloorDungeonSquares[position.Row, position.Column]) + "\n";
        }
        this.AnnounceByText(s);
    }

    /// <summary>
    /// 対象にする味方決定の入力
    /// </summary>
    private void InputTargetToAlly()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (this.currentIndexOfTargetableAllys > this.allys.Count - 2) this.currentIndexOfTargetableAllys = 0;
            else this.currentIndexOfTargetableAllys++;
            AnnounceByText("現在選択中の味方:" + this.allys[this.currentIndexOfTargetableAllys]);
        }else if (Input.GetKeyDown(KeyCode.A))
        {
            if (this.currentIndexOfTargetableAllys <= 0) this.currentIndexOfTargetableAllys = this.allys.Count - 1;
            else this.currentIndexOfTargetableAllys--;
            AnnounceByText("現在選択中の味方:" + this.allys[this.currentIndexOfTargetableAllys]);
        }else if (Input.GetKeyDown(KeyCode.Return))
        {
            this.targetAllys = new List<BattleCharacter>() { this.allys[this.currentIndexOfTargetableAllys] };
            this.MoveScene(E_DungeonScene.InvokeActiveEffect);
        }
    }

    /// <summary>
    /// 対象にするマス決定の入力
    /// </summary>
    private void InputTargetToDungeonSquare()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (this.currentIndexOfTargetableDungeonSquares > this.targetableDungeonSquares.Count - 2) this.currentIndexOfTargetableDungeonSquares = 0;
            else this.currentIndexOfTargetableDungeonSquares++;
            AnnounceByText("現在選択中のマス:[" + targetableDungeonSquares[currentIndexOfTargetableDungeonSquares].Row + "," + targetableDungeonSquares[currentIndexOfTargetableDungeonSquares].Column + "]" + 
                DungeonManager.GetStringDungeonSquareType(this.currentFloorDungeonSquares[this.targetableDungeonSquares[this.currentIndexOfTargetableDungeonSquares].Row, this.targetableDungeonSquares[this.currentIndexOfTargetableDungeonSquares].Column]));
        }else if (Input.GetKeyDown(KeyCode.A))
        {
            if (this.currentIndexOfTargetableDungeonSquares <= 0) this.currentIndexOfTargetableDungeonSquares = this.targetableDungeonSquares.Count - 1;
            else this.currentIndexOfTargetableDungeonSquares--;
            AnnounceByText("現在選択中のマス:[" + targetableDungeonSquares[currentIndexOfTargetableDungeonSquares].Row + "," + targetableDungeonSquares[currentIndexOfTargetableDungeonSquares].Column + "]" +
                DungeonManager.GetStringDungeonSquareType(this.currentFloorDungeonSquares[this.targetableDungeonSquares[this.currentIndexOfTargetableDungeonSquares].Row, this.targetableDungeonSquares[this.currentIndexOfTargetableDungeonSquares].Column]));
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            //ターゲット決定処理
            this.targetDungeonSquares = new List<PositionXY>() { this.targetableDungeonSquares[this.currentIndexOfTargetableDungeonSquares] };
            this.MoveScene(E_DungeonScene.InvokeActiveEffect);
        }
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
        this.mapManager.SetFlagUnderstandDungeonSquareType(ref this.understandDungeonSquareType, flag);
    }

    IEnumerator DelayMethod(int delayFrameCount, Action action)
    {
        for (int i = 0; i < delayFrameCount; i++)
        {
            yield return null;
        }
        action();
    }
}

public enum E_Direction
{
    Up,
    Right,
    Down,
    Left,
    None
}

public class PositionXY
{
    public int Row { get; set; }
    public int Column { get; set; }
    public PositionXY(int row, int column)
    {
        this.Row = row; this.Column = column;
    }
}