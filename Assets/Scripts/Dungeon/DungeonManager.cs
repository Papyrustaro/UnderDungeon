﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    private List<BattleCharacter> allys; //battleCharacterとして持つべきか？

    /// <summary>
    /// ご褒美用アイテム
    /// </summary>
    [SerializeField] private BattlePassiveItem goodItem;

    /// <summary>
    /// 罰用アイテム
    /// </summary>
    [SerializeField] private BattlePassiveItem badItem;

    private List<DungeonActiveItem> haveDungeonActiveItems = new List<DungeonActiveItem>();
    private List<DungeonPassiveItem> haveDungeonPassiveItems = new List<DungeonPassiveItem>();
    private List<BattleActiveItem> haveBattleActiveItems = new List<BattleActiveItem>();
    private List<BattlePassiveItem> haveBattlePassiveItems = new List<BattlePassiveItem>();

    private List<PlayerCharacter> dropCharacters = new List<PlayerCharacter>(); //IDとして持ってもよい

    private E_DungeonSquareType[,] currentFloorDungeonSquares;
    private bool[,] understandDungeonSquareType; //マスイベントが何かわかるか(自分の周囲のマスのタイプを記憶する)

    private List<int> haveDsp = new List<int>(); //保持しているDsp(とりあえずこのクラスで保持)
    private List<int> needDsp = new List<int>(); //それぞれのスキルに必要なDsp(とりあえずこのクラスで保持)

    private MapManager mapManager;

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
    public int CurrentLocationRow { get; set; }

    /// <summary>
    /// マップの左から何列目か(0スタート)
    /// </summary>
    public int CurrentLocationColumn { get; set; }

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

    public List<DungeonActiveItem> HaveDungeonActiveItems => this.haveDungeonActiveItems;
    public List<DungeonPassiveItem> HaveDungeonPassiveItems => this.haveDungeonPassiveItems;
    public List<BattleActiveItem> HaveBattleActiveItems => this.haveBattleActiveItems;
    public List<BattlePassiveItem> HaveBattlePassiveItems => this.haveBattlePassiveItems;

    private void GenerateFloor(int rowSize, int columnSize)
    {
        //this.currentFloorDungeonSquares = new DungeonSquare[rowSize, columnSize];
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
}
