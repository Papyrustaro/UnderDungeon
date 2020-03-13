using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAS_IncreaseDsp : DungeonActiveSkill
{
    [SerializeField] private int increaseValue;
    [SerializeField] private E_TargetType targetTypeToAlly;

    public override E_DungeonActiveEffectType EffectType => E_DungeonActiveEffectType.Bsp回復;
    public override void EffectFunc(DungeonManager dm)
    {
        dm.ChangeAllDsp(increaseValue); //とりあえず対象は味方全体のみで
    }
}
