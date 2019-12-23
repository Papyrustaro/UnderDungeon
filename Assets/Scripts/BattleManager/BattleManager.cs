using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacter[] player;
    [SerializeField]
    private EnemyCharacter[] enemy;

    private void Awake()
    {
        try
        {
            Debug.Log(EnumBattleActiveSkillID.EnumSize);
            BattleActiveSkillsFunc.SkillFunc(E_BattleActiveSkill.たいあたり);
            BattleActiveSkillsFunc.SkillFunc(E_BattleActiveSkill.二段切り);
        }catch(Exception e)
        {
            Debug.Log(e);
        }
    }
}
