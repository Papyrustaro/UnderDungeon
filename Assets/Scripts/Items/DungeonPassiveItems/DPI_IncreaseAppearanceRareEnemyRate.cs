using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPI_IncreaseAppearanceRareEnemyRate : DungeonPassiveItem
{
    [SerializeField] private double increaseRateValue = 0.1;
    public override E_DungeonPassiveEffectType EffectType => E_DungeonPassiveEffectType.レア敵出現率増加;

    public override void EffectFunc(DungeonManager dm)
    {
        dm.IncreaseAppearanceRareEnemyRate(this.increaseRateValue);
    }
}
