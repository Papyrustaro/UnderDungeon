using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPS_IncreaseShopRareGoodRate : DungeonPassiveSkill
{
    [SerializeField] private double increaseRateValue = 0.1;

    public override E_DungeonPassiveEffectType EffectType => E_DungeonPassiveEffectType.店レア商品出現確率増加;

    public override void EffectFunc(DungeonManager dm)
    {
        dm.IncreaseShopRareGoodsRate(this.increaseRateValue);
    }
}
