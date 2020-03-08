using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    private List<PlayerCharacter> allys; //battleCharacterとして持つべきか？

    private List<DungeonActiveItem> haveDungeonActiveItems = new List<DungeonActiveItem>();
    private List<DungeonPassiveItem> haveDungeonPassiveItems = new List<DungeonPassiveItem>();
    private List<BattleActiveItem> haveBattleActiveItems = new List<BattleActiveItem>();
    private List<BattlePassiveItem> haveBattlePassiveItems = new List<BattlePassiveItem>();

    private List<PlayerCharacter> dropCharacters = new List<PlayerCharacter>(); //IDとして持ってもよい

    private DungeonSquare[,] currentFloorDungeonSquares;

    private int fullness = 100;

    /// <summary>
    /// 満腹度(マス移動する度に減少?)
    /// </summary>
    public int FullNess => this.fullness;

    /// <summary>
    /// 最大満腹度
    /// </summary>
    public int MaxFullNess { get; set; }

    public List<PlayerCharacter> Allys => this.allys;

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

    private void GenerateFloor(int rowSize, int columnSize)
    {
        this.currentFloorDungeonSquares = new DungeonSquare[rowSize, columnSize];
    }
}
