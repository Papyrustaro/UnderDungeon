using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAI_IncreaseMovementByRate : DungeonActiveItem
{
    [SerializeField] private int movementRate;
    public override E_DungeonActiveEffectType EffectType => E_DungeonActiveEffectType.移動量倍増;

    public override void EffectFunc(DungeonManager dm)
    {
        Debug.Log("移動量が" + this.movementRate + "倍");
        dm.MovementRate = this.movementRate;
    }
}
