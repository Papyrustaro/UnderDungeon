using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPS_IncreaseEnemyDropRate : DungeonPassiveSkill
{
    [SerializeField] private double increaseRateValue = 0.1;

    public override E_DungeonPassiveEffectType EffectType => E_DungeonPassiveEffectType.敵ドロップ率増加;

    public override void EffectFunc(DungeonManager dm)
    {
        dm.IncreaseEnemyDropRate(this.increaseRateValue);
    }
}
