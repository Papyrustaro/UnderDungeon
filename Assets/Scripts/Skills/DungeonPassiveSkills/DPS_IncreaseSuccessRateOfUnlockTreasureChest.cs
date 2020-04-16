using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPS_IncreaseSuccessRateOfUnlockTreasureChest : DungeonPassiveSkill
{
    [SerializeField] private double increaseRateValue = 0.1;

    public override E_DungeonPassiveEffectType EffectType => E_DungeonPassiveEffectType.宝箱解除成功率増加;

    public override void EffectFunc(DungeonManager dm)
    {
        dm.IncreaseSuccessRateOfUnlockTreasureChest(this.increaseRateValue);
    }
}
