using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* その他のBattleActiveSkillの各々のスキルにつなぐ抽象クラス */
public abstract class OtherBattleActiveSkillsFunc : MonoBehaviour
{
    public virtual void SkillFunc(BattleCharacter invoker, List<BattleCharacter> target, BattleActiveSkill skill)
    {
    }
}
