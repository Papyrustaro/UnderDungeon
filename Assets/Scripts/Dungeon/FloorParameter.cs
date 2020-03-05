using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各階層毎に設定するパラメータ保持クラス(例: マップの大きさ、出現する敵など)
/// </summary>
public class FloorParameter : MonoBehaviour
{
    [SerializeField] private E_Dungeon dungeonID;
    [SerializeField] private int floorNumber;

    [SerializeField] private List<EnemyCharacter> enemyList = new List<EnemyCharacter>();

    //アイテムはIDとして持つべきかな
    [SerializeField] private List<DungeonActiveItem> dungeonActiveItemList = new List<DungeonActiveItem>();
    [SerializeField] private List<DungeonPassiveItem> dungeonPassiveItemList = new List<DungeonPassiveItem>();
    [SerializeField] private List<BattleActiveItem> battleActiveItemList = new List<BattleActiveItem>();
    [SerializeField] private List<BattlePassiveItem> battlePassiveItemList = new List<BattlePassiveItem>();

    //マスもIDとして持つべき？
    [SerializeField] private List<DungeonSquare> dungeonSquareList = new List<DungeonSquare>();

    /// <summary>
    /// マップの大きさ。小中大などで分けてもいいかも。
    /// </summary>
    [SerializeField] private int mapScale = 10;

    /// <summary>
    /// 何マス先まで見えるか。(動的に変わるが、基礎となる値)
    /// </summary>
    [SerializeField] private int baseFieldOfVision = 5;

    /// <summary>
    /// 何のダンジョンか
    /// </summary>
    public E_Dungeon DungeonID => this.dungeonID;

    /// <summary>
    /// ダンジョンの地下何階か
    /// </summary>
    public int FloorNumber => this.floorNumber;

    /// <summary>
    /// この階層で出現しうる敵のリスト
    /// </summary>
    public List<EnemyCharacter> EnemyList => this.enemyList;

    /// <summary>
    /// この階層で出現しうるDungeonActiveItemのリスト
    /// </summary>
    public List<DungeonActiveItem> DungeonActiveItemList => this.dungeonActiveItemList;

    /// <summary>
    /// この階層で出現しうるDungeonPassiveItemのリスト
    /// </summary>
    public List<DungeonPassiveItem> DungeonPassiveItemList => this.dungeonPassiveItemList;

    /// <summary>
    /// この階層で出現しうるBattleActiveItemのリスト
    /// </summary>
    public List<BattleActiveItem> BattleActiveItemList => this.battleActiveItemList;

    /// <summary>
    /// この階層で出現しうるBattlePassiveItemのリスト
    /// </summary>
    public List<BattlePassiveItem> BattlePassiveItemList => this.battlePassiveItemList;

    /// <summary>
    /// この階層で出現しうるマスイベントのリスト
    /// </summary>
    public List<DungeonSquare> DungeonSquareList => this.dungeonSquareList;
}
