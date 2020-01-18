using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSkillFunc : OtherBattleActiveSkillsFunc
{
    public override void SkillFunc(BattleCharacter invoker, List<BattleCharacter> target, BattleActiveSkill skill)
    {
        Debug.Log(invoker.CharaClass.CharaName + "のその他のスキル発動");
        Debug.Log("スキルの説明:" + skill.Description);
    }
}
