using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAI_IncreaseDsp : DungeonActiveItem
{
    [SerializeField] private int increaseValue;
    [SerializeField] private E_TargetTypeToAlly targetTypeToAlly;

    public override E_DungeonActiveEffectType EffectType => E_DungeonActiveEffectType.Bsp回復;

    public override E_DungeonActiveEffectTargetType DungeonActiveEffectTargetType => EnumManager.GetDungeonActiveEffectTargetType(this.targetTypeToAlly);

    public override void EffectFunc(DungeonManager dm)
    {
        dm.IncreaseDsp(this.increaseValue);
    }
}
