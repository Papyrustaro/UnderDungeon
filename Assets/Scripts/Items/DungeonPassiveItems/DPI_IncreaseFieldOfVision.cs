using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPI_IncreaseFieldOfVision : DungeonPassiveItem
{
    [SerializeField] private int increaseValue = 1;

    public override E_DungeonPassiveEffectType EffectType => E_DungeonPassiveEffectType.マス可視範囲増加;

    public override void EffectFunc(DungeonManager dm)
    {
        Debug.Log("マップ可視範囲" + this.increaseValue + "増加");
        dm.FieldOfVision += this.increaseValue;
    }
}
