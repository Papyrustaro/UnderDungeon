using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePassiveItem : BattlePassiveEffect
{
    [SerializeField]
    private E_BattlePassiveItem id;

    public E_BattlePassiveItem ID => id;
    public override string EffectName => this.id.ToString();
    public override void OtherFunc(BattleCharacter invoker, List<BattleCharacter> target)
    {
        //GetComponent<OtherBattleActiveSkillsFunc>().SkillFunc(invoker, target, this);
    }

}
