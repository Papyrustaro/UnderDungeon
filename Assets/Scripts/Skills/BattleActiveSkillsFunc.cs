using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleActiveSkillsFunc : MonoBehaviour
{
    public static void SkillFunc(E_BattleActiveSkill id)
    {
        if((int)id > EnumBattleActiveSkillID.EnumSize)
        {
            Debug.Log("idがBattleActiveSkillIDの最大値を超えています");
            throw new Exception();
        }
        switch (id)
        {
            case E_BattleActiveSkill.たいあたり:
                Debug.Log("たいあたり");
                break;
            case E_BattleActiveSkill.二段切り:
                Debug.Log("二段切り");
                break;
        }
    }
}
