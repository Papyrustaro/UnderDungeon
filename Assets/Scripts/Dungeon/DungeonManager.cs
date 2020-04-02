﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    private List<BattleCharacter> targetAllys = null; //効果対象にする味方記憶用
    private int currentIndexOfTargetableAllys = 0; //現在選んでいるtargetableAllysの番地
    private List<PositionXY> targetableDungeonSquares = null; //効果対象にできるマスの座標
    private List<PositionXY> targetDungeonSquares = null; //効果対象にするマスの座標
    private int currentIndexOfTargetableDungeonSquares = 0; //現在選んでいるtargetableDungeonSquaresの番地

    /// <summary>
    /// ActiveEffect発動前記憶用
    /// </summary>
    private DungeonActiveEffect waitActiveEffect;

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

    /// <summary>
    /// 残りの移動量(一次記憶用)
    /// </summary>
    public int RemainingAmountOfMovement { get; set; } = 0;

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

    private void Start()
    {
        //this.mapManager.GenerateFloor(this.currentFloorDungeonSquares);
        this.GenerateFloor(this.mapManager.MapWidth, this.mapManager.MapHeight);
        this.dungeonSquaresFunc.SetMayApeearDungeonSquares(this.mapManager.MayApeearDungeonSquares);
        Debug.Log(this.currentFloorDungeonSquares[0, 0]);
        this.MoveScene(E_DungeonScene.SelectAction);
    }

    private void Update()
    {
        ActionInCurrentScene();
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
            case E_DungeonScene.SelectDAITargetToAlly:
                InputTargetToAlly();
                break;
            case E_DungeonScene.SelectDAITargetToDungeonSquare:
                InputTargetToDungeonSquare();
                break;
            case E_DungeonScene.SelectDAS:
                if (NeedAnnounce) ShowHaveDungeonActiveSkill();
                InputInvokeDungeonActiveSkill();
                break;
            case E_DungeonScene.SelectDASTargetToAlly:
                InputTargetToAlly();
                break;
            case E_DungeonScene.SelectDASTargetToDungeonSquare:
                InputTargetToDungeonSquare();
                break;
            case E_DungeonScene.InvokeActiveEffect:
                this.waitActiveEffect.EffectFunc(this);
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
                this.MoveScene(E_DungeonScene.SelectAction);
                this.NeedAnnounce = true;
                break;
        }
    }

    /// <summary>
    /// 現在の状態を変化させる
    /// </summary>
    /// <param name="toMoveScene">移動先の状態</param>
    private void MoveScene(E_DungeonScene toMoveScene)
    {
        this.currentScene = toMoveScene;
        switch (toMoveScene)
        {
            case E_DungeonScene.SelectAction:
            case E_DungeonScene.SelectDAI:
            case E_DungeonScene.SelectDAITargetToAlly:
            case E_DungeonScene.SelectDAITargetToDungeonSquare:
            case E_DungeonScene.SelectDAS:
            case E_DungeonScene.SelectDASTargetToAlly:
            case E_DungeonScene.SelectDASTargetToDungeonSquare:
            case E_DungeonScene.SelectMoveDirection:
                this.NeedAnnounce = true;
                break;
        }
    }
    private void AnnounceByText(string announceText)
    {
        this.dungeonUIManager.AnnounceByText(announceText);
        this.NeedAnnounce = false;
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
            ViewMap();
        }else if (Input.GetKeyDown(((int)E_DungeonPlayerSelect.VerificateAlly).ToString()))
        {
            //パーティ確認に遷移
            ShowAllysStatus();
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
        this.AnnounceByText(s);
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

        this.RemainingAmountOfMovement = diceEye;
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
                this.haveDungeonActiveItems[i].EffectFunc(this);
            }
        }
    }

    /// <summary>
    /// DAIの味方へのターゲットをセット
    /// </summary>
    /// <param name="targetType">DAIのターゲットタイプ</param>
    private void SetDungeonActiveItemTargetToAlly(E_DungeonActiveEffectTargetType targetType)
    {
        switch (targetType)
        {
            case E_DungeonActiveEffectTargetType.OneAlly:
                this.MoveScene(E_DungeonScene.SelectDAITargetToAlly);
                break;
            case E_DungeonActiveEffectTargetType.AllAlly:
                this.targetAllys = this.allys;
                this.MoveScene(E_DungeonScene.InvokeActiveEffect);
                break;
            case E_DungeonActiveEffectTargetType.AllDungeonSquare:
                SetTargetableDungeonSquare(this.waitActiveEffect.TargetDungeonSquareTypes, this.waitActiveEffect.EffectRange);
                this.targetDungeonSquares = this.targetableDungeonSquares;
                this.MoveScene(E_DungeonScene.InvokeActiveEffect);
                break;
            case E_DungeonActiveEffectTargetType.OneDungeonSquare:
                SetTargetableDungeonSquare(this.waitActiveEffect.TargetDungeonSquareTypes, this.waitActiveEffect.EffectRange);
                this.MoveScene(E_DungeonScene.SelectDAITargetToDungeonSquare);
                break;
            case E_DungeonActiveEffectTargetType _:
                throw new Exception();
        }
    }

    /// <summary>
    /// DASの味方へのターゲットセット
    /// </summary>
    /// <param name="targetType">DASのターゲットタイプ</param>
    /// <param name="invoker">スキル発動者</param>
    private void SetDungeonActiveSkillTargetToAlly(E_DungeonActiveEffectTargetType targetType, BattleCharacter invoker)
    {
        switch (targetType)
        {
            case E_DungeonActiveEffectTargetType.OneAlly:
                this.MoveScene(E_DungeonScene.SelectDASTargetToAlly);
                break;
            case E_DungeonActiveEffectTargetType.AllAlly:
                this.targetAllys = this.allys;
                break;
            case E_DungeonActiveEffectTargetType.SelfAlly:
                this.targetAllys = new List<BattleCharacter>() { invoker };
                break;
            case E_DungeonActiveEffectTargetType.AllDungeonSquare:
                SetTargetableDungeonSquare(this.waitActiveEffect.TargetDungeonSquareTypes, this.waitActiveEffect.EffectRange);
                this.targetDungeonSquares = this.targetableDungeonSquares;
                this.MoveScene(E_DungeonScene.InvokeActiveEffect);
                break;
            case E_DungeonActiveEffectTargetType.OneDungeonSquare:
                SetTargetableDungeonSquare(this.waitActiveEffect.TargetDungeonSquareTypes, this.waitActiveEffect.EffectRange);
                this.MoveScene(E_DungeonScene.SelectDASTargetToDungeonSquare);
                break;
            case E_DungeonActiveEffectTargetType _:
                throw new Exception();
        }
    }

    private void SetTargetOfDungeonActiveEffect(E_DungeonActiveEffectTargetType targetType)
    {

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
                this.dungeonActiveEffectsFunc.GetSkill(this.allys[i].PC.HaveDungeonActiveSkillID).EffectFunc(this);
            }
        }
    }

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
                MoveScene(E_DungeonScene.SelectDAITargetToDungeonSquare);
                break;
            case E_DungeonActiveEffectTargetType.Error:
                throw new Exception();
        }
    }

    
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
                //マップ全体可視化
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
            Debug.Log("進行方向を選択してください");
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

    public void ChangeDungeonSquareType(List<E_DungeonSquareType> targetType, E_DungeonSquareType afterChangeDungeonSquareType, int effectRange)
    {
        //targetTypeに一致するマスからひとつ選択する処理(または戻る)
        //選択したマスをafterChangeのマスに変化させる処理
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
                    if (this.currentFloorDungeonSquares[i, j] == dsType) this.targetableDungeonSquares.Add(new PositionXY(i, j));
                }
            }
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
            Debug.Log("現在選択中の味方:" + this.allys[this.currentIndexOfTargetableAllys]);
        }else if (Input.GetKeyDown(KeyCode.A))
        {
            if (this.currentIndexOfTargetableAllys <= 0) this.currentIndexOfTargetableAllys = this.allys.Count - 1;
            else this.currentIndexOfTargetableAllys--;
            Debug.Log("現在選択中の味方:" + this.allys[this.currentIndexOfTargetableAllys]);
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
            Debug.Log("現在選択中のマス:[" + "," + "]" + DungeonManager.GetStringDungeonSquareType(this.currentFloorDungeonSquares[this.targetableDungeonSquares[this.currentIndexOfTargetableDungeonSquares].Row, this.targetableDungeonSquares[this.currentIndexOfTargetableDungeonSquares].Column]));
        }else if (Input.GetKeyDown(KeyCode.A))
        {
            if (this.currentIndexOfTargetableDungeonSquares <= 0) this.currentIndexOfTargetableDungeonSquares = this.targetableDungeonSquares.Count - 1;
            else this.currentIndexOfTargetableDungeonSquares--;
            Debug.Log("現在選択中のマス:[" + "," + "]" + DungeonManager.GetStringDungeonSquareType(this.currentFloorDungeonSquares[this.targetableDungeonSquares[this.currentIndexOfTargetableDungeonSquares].Row, this.targetableDungeonSquares[this.currentIndexOfTargetableDungeonSquares].Column]));
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
        this.mapManager.SetFlagUnderstandDungeonSquareType(this.understandDungeonSquareType, flag);
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