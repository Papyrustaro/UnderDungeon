using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPS_IncreaseShopGoodsNum : DungeonPassiveSkill
{
    [SerializeField] private int increaseValue = 1;

    public override E_DungeonPassiveEffectType EffectType => E_DungeonPassiveEffectType.店商品増加;

    public override void EffectFunc(DungeonManager dm)
    {
        dm.IncreaseShopGoodsNum(this.increaseValue);
    }
}
