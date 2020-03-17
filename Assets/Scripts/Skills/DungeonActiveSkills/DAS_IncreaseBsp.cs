using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAS_IncreaseBsp : DungeonActiveSkill
{
    [SerializeField] private int increaseValue;
    [SerializeField] private E_TargetTypeToAlly targetTypeToAlly = E_TargetTypeToAlly.AllAlly;

    public override E_DungeonActiveEffectType EffectType => E_DungeonActiveEffectType.Bsp回復;

    public override E_DungeonActiveEffectTargetType DungeonActiveEffectTargetType => EnumManager.GetDungeonActiveEffectTargetType(this.targetTypeToAlly);

    public override void EffectFunc(DungeonManager dm)
    {
        dm.IncreaseBsp(this.increaseValue);
    }
}
