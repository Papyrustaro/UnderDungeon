using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    private List<PlayerCharacter> players; //battleCharacterとして持つべきか？

    private List<DungeonActiveItem> haveDungeonActiveItems = new List<DungeonActiveItem>();
    private List<DungeonPassiveItem> haveDungeonPassiveItems = new List<DungeonPassiveItem>();
    private List<BattleActiveItem> haveBattleActiveItems = new List<BattleActiveItem>();
    private List<BattlePassiveItem> haveBattlePassiveItems = new List<BattlePassiveItem>();

    private List<PlayerCharacter> dropCharacters = new List<PlayerCharacter>();

    public List<PlayerCharacter> Players => this.players;

    /// <summary>
    /// マップの上から何行目か(0スタート)
    /// </summary>
    public int CurrentLocationRow { get; set; }

    /// <summary>
    /// マップの左から何列目か(0スタート)
    /// </summary>
    public int CurrentLocationColumn { get; set; }
}
