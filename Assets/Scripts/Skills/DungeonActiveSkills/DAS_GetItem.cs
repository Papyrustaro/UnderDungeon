using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAS_GetItem : DungeonActiveSkill
{
    //獲得するアイテムは固定(リストのどれか1つ)
    [SerializeField] private List<E_DungeonActiveItem> dungeonActiveItemId = null;
    [SerializeField] private List<E_DungeonPassiveItem> dungeonPassiveItemId = null;
    [SerializeField] private List<E_BattleActiveItem> battleActiveItemId = null;
    [SerializeField] private List<E_BattlePassiveItem> battlePassiveItemId = null;
    public override E_DungeonActiveEffectType EffectType => E_DungeonActiveEffectType.アイテム獲得;

    public override void EffectFunc(DungeonManager dm)
    {
        if (this.dungeonActiveItemId != null) dm.AddHaveItem(this.dungeonActiveItemId[0]);
        else if (this.dungeonPassiveItemId != null) dm.AddHaveItem(this.dungeonPassiveItemId[0]);
        else if (this.battleActiveItemId != null) dm.AddHaveItem(this.battleActiveItemId[0]);
        else dm.AddHaveItem(this.battlePassiveItemId[0]);
    }
}
