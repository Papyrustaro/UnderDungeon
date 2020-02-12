using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActiveItem : BattleActiveEffect
{
    [SerializeField]
    private E_BattleActiveItem id;

    public E_BattleActiveItem ID => this.id;
    public override string EffectName => this.id.ToString();

    public override void OtherFunc(BattleCharacter invoker, List<BattleCharacter> target)
    {
        GetComponent<OtherBattleActiveItemsFunc>().ItemFunc(invoker, target, this);
    }
}
