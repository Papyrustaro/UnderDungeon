using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAS_IncreaseBsp : DungeonActiveSkill
{
    [SerializeField] private E_TargetType targetTypeToAlly;
    [SerializeField] private int increaseValue;

    public override E_DungeonActiveEffectType EffectType => E_DungeonActiveEffectType.Bsp回復;

    public override void EffectFunc(DungeonManager dm)
    {
        //処理
    }
}
