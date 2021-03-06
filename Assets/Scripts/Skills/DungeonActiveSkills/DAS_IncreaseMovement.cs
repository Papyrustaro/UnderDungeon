﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAS_IncreaseMovement : DungeonActiveSkill
{
    [SerializeField] private int increaseValue;
    public override E_DungeonActiveEffectType EffectType => E_DungeonActiveEffectType.移動量増加;

    public override void EffectFunc(DungeonManager dm)
    {
        Debug.Log("移動量" + this.increaseValue + "増加");
        dm.MovementIncreaseValue = this.increaseValue;
    }
}
