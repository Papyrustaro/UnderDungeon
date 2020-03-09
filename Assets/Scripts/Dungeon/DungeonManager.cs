using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    private List<BattleCharacter> allys; //battleCharacterとして持つべきか？

    private List<DungeonActiveItem> haveDungeonActiveItems = new List<DungeonActiveItem>();
    private List<DungeonPassiveItem> haveDungeonPassiveItems = new List<DungeonPassiveItem>();
    private List<BattleActiveItem> haveBattleActiveItems = new List<BattleActiveItem>();
    private List<BattlePassiveItem> haveBattlePassiveItems = new List<BattlePassiveItem>();

    private List<PlayerCharacter> dropCharacters = new List<PlayerCharacter>(); //IDとして持ってもよい

    private E_DungeonSquareType[,] currentFloorDungeonSquares;
    private bool[,] understandDungeonSquareType; //マスイベントが何かわかるか(自分の周囲のマスのタイプを記憶する)

    private int fullness = 100;

    /// <summary>
    /// 満腹度(マス移動する度に減少?)
    /// </summary>
    public int FullNess => this.fullness;

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
    /// 何マス先まで見えるか
    /// </summary>
    public int FieldOfVision { get; set; }

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

    public List<DungeonActiveItem> HaveDungeonActiveItems => this.haveDungeonActiveItems;
    public List<DungeonPassiveItem> HaveDungoenPassiveItems => this.haveDungeonPassiveItems;
    public List<BattleActiveItem> HaveBattleActiveItems => this.haveBattleActiveItems;
    public List<BattlePassiveItem> HaveBattlePassiveItems => this.haveBattlePassiveItems;

    private void GenerateFloor(int rowSize, int columnSize)
    {
        //this.currentFloorDungeonSquares = new DungeonSquare[rowSize, columnSize];
    }

    /// <summary>
    /// ランダムでひとつ悪い効果適用
    /// </summary>
    private void ActivateRandomBadEffect()
    {
        switch(UnityEngine.Random.Range(0, 5))
        {
            case 0:
                Debug.Log("チームHp全回復");

                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    /// <summary>
    /// ランダムでひとつ良い効果発動
    /// </summary>
    private void ActivateRandomGoodEffect()
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

                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
}
