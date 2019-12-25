using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum E_BattleActiveSkill
{
    たいあたり,
    二段切り,
    ホイミ,
    とっしん,
    二度蹴り,
}

public class EnumBattleActiveSkillID
{
    public static int EnumSize => Enum.GetValues(typeof(E_BattleActiveSkill)).Length;
}