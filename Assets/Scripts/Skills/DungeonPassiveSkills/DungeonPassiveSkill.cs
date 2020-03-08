using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonPassiveSkill : DungeonPassiveEffect
{
    [SerializeField]
    private E_DungeonPassiveSkill id;

    /// <summary>
    /// スキルID
    /// </summary>
    public E_DungeonPassiveSkill Id => id;

    /// <summary>
    /// スキル名
    /// </summary>
    public override string EffectName => id.ToString();
}
