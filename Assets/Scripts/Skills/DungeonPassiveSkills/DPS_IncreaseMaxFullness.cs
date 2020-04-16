using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPS_IncreaseMaxFullness : DungeonPassiveSkill
{
    [SerializeField] private int increaseValue = 10;

    public override E_DungeonPassiveEffectType EffectType => E_DungeonPassiveEffectType.最大満腹度増加;

    public override void EffectFunc(DungeonManager dm)
    {
        dm.IncreaseMaxFullness(this.increaseValue);
    }
}
