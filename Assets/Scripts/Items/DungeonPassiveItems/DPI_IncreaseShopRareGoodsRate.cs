using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPI_IncreaseShopRareGoodsRate : DungeonPassiveItem
{
    [SerializeField] private double increaseRateValue = 0.1;

    public override E_DungeonPassiveEffectType EffectType => E_DungeonPassiveEffectType.店レア商品出現確率増加;

    public override void EffectFunc(DungeonManager dm)
    {
        dm.IncreaseShopRareGoodsRate(this.increaseRateValue);
    }
}
