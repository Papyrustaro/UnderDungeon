using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSkillFunc : OtherBattleActiveSkillsFunc
{
    public override void SkillFunc(BattleCharacter invoker, List<BattleCharacter> target, BattleActiveSkill skill)
    {
        Debug.Log(invoker.CharaClass.CharaName + "のスキル発動");
        foreach(BattleCharacter bc in target)
        {
            Debug.Log("target:" + bc.CharaClass.CharaName);
        }
        Debug.Log("スキルの説明:" + skill.Description);
    }
}
