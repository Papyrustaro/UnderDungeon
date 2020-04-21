using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPassiveEffectsFunc : MonoBehaviour
{
    [SerializeField] private List<DungeonPassiveItem> dungeonPassiveItems = new List<DungeonPassiveItem>();
    [SerializeField] private List<DungeonPassiveSkill> dungeonPassiveSkills = new List<DungeonPassiveSkill>();

    public DungeonPassiveItem GetItem(E_DungeonPassiveItem itemId)
    {
        return this.dungeonPassiveItems[(int)itemId];
    }

    public DungeonPassiveSkill GetSkill(E_DungeonPassiveSkill skillId)
    {
        return this.dungeonPassiveSkills[(int)skillId];
    }
}
