using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPS_IncreaseGetExp : DungeonPassiveSkill
{
    [SerializeField] private double increaseRateValue = 0.1;

    public override E_DungeonPassiveEffectType EffectType => E_DungeonPassiveEffectType.獲得経験値増加;

    public override void EffectFunc(DungeonManager dm)
    {
        dm.IncreaseGetExpRate(this.increaseRateValue);
    }
}
