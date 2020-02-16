using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePassiveSkill : BattlePassiveEffect
{
    [SerializeField]
    private E_BattlePassiveSkill id;

    public E_BattlePassiveSkill ID => id;
    public override string EffectName => this.id.ToString();
    public override void OtherFunc(BattleCharacter target)
    {
        GetComponent<OtherBattlePassiveSkillsFunc>().SkillFunc(target, this);
    }

}
