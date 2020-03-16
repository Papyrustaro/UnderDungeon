using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAI_RecoverFullness : DungeonActiveItem
{
    [SerializeField] private double recoverRateOrValue;
    public override E_DungeonActiveEffectType EffectType => E_DungeonActiveEffectType.満腹度回復;

    public override void EffectFunc(DungeonManager dm)
    {
        if (this.recoverRateOrValue > 1)
        {
            dm.RecoverFullness((int)this.recoverRateOrValue);
        }
        else
        {
            dm.RecoverFullnessByRate(this.recoverRateOrValue);
        }
    }
}
