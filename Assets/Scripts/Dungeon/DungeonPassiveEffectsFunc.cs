using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPassiveEffectsFunc : MonoBehaviour
{
    [SerializeField] private List<DungeonPassiveItem> dungeonPassiveItems = new List<DungeonPassiveItem>();

    public DungeonPassiveItem GetItem(E_DungeonPassiveItem itemId)
    {
        return this.dungeonPassiveItems[(int)itemId];
    }
}
