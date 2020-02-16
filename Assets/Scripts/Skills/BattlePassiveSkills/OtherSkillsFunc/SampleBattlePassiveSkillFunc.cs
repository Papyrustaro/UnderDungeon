using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleBattlePassiveSkillFunc : OtherBattlePassiveSkillsFunc
{
    public override void SkillFunc(BattleCharacter target, BattlePassiveSkill skill)
    {
        //Debug.Log("SampleBattlePassiveSkill発動/" + skill.EffectName);
        Debug.Log("スキルの説明:" + skill.Description);
        Debug.Log("効果対象:" + target.CharaClass.CharaName);
    }
}
