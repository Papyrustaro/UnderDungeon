﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAS_ChangeDice : DungeonActiveSkill
{
    [SerializeField] private int[] diceEyesToChange = new int[6];
    public override E_DungeonActiveEffectType EffectType => E_DungeonActiveEffectType.サイコロ変化;

    public override void EffectFunc(DungeonManager dm)
    {
        dm.SetChangedDiceEyes(this.diceEyesToChange);
    }
}
