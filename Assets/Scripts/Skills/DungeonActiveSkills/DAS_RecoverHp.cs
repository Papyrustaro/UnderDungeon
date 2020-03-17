using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAS_RecoverHp : DungeonActiveSkill
{
    [SerializeField] private double recoverRateOrValue;
    [SerializeField] private E_TargetTypeToAlly targetTypeToAlly;

    public override E_DungeonActiveEffectType EffectType => E_DungeonActiveEffectType.Hp回復;

    public override E_DungeonActiveEffectTargetType DungeonActiveEffectTargetType => EnumManager.GetDungeonActiveEffectTargetType(this.targetTypeToAlly);
    public override void EffectFunc(DungeonManager dm)
    {
        dm.RecoverHp(this.recoverRateOrValue);
    }
}
