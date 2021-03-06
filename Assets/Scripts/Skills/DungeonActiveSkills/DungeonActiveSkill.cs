﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DungeonActiveSkill : DungeonActiveEffect
{
    [SerializeField]
    private E_DungeonActiveSkill id;
    [SerializeField] private int needDsp;

    /// <summary>
    /// スキルID
    /// </summary>
    public E_DungeonActiveSkill Id => id;

    /// <summary>
    /// スキル名
    /// </summary>
    public override string EffectName => id.ToString();

    public int NeedDsp => this.needDsp;
}
