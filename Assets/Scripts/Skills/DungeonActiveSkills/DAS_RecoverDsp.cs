using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAS_RecoverDsp : DungeonActiveSkill
{
    [SerializeField] private int recoverValue;
    [SerializeField] private E_TargetType targetTypeToAlly;
    public override void EffectFunc(DungeonManager dm)
    {
        dm.ChangeAllDsp(recoverValue);
    }
}
