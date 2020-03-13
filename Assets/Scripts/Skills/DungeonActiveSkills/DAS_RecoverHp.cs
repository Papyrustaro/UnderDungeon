using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAS_RecoverHp : DungeonActiveSkill
{
    [SerializeField] private double recoverRateOrValue;
    [SerializeField] private E_TargetType targetTypeToAlly;

    public override E_DungeonActiveEffectType EffectType => E_DungeonActiveEffectType.Hp回復;
    public override void EffectFunc(DungeonManager dm)
    {
        //処理
        throw new System.NotImplementedException();
    }
}
