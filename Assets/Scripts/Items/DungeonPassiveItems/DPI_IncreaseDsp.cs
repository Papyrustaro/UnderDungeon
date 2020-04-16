using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPI_IncreaseDsp : DungeonPassiveItem
{
    [SerializeField] private int increaseValue;
    public override E_DungeonPassiveEffectType EffectType => E_DungeonPassiveEffectType.開始時Dsp増加;

    public override void EffectFunc(DungeonManager dm)
    {
        dm.ChangeAllDsp(this.increaseValue);
    }
}
