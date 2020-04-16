using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPI_IncreaseEvadeTrapRate : DungeonPassiveItem
{
    [SerializeField] private double increaseRateValue = 0.1;

    public override E_DungeonPassiveEffectType EffectType => E_DungeonPassiveEffectType.罠回避率増加;

    public override void EffectFunc(DungeonManager dm)
    {
        dm.IncreaseEvadeTrapRate(increaseRateValue);
    }
}