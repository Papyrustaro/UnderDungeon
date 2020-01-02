using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleActiveSkillsFunc : MonoBehaviour
{
    //private BattleCharacter invoker;
    //private List<BattleCharacter> target;

    [SerializeField]
    private List<BattleActiveSkill> skillList = new List<BattleActiveSkill>();

    public void SkillFunc(E_BattleActiveSkill id, BattleCharacter invoker, List<BattleCharacter> target)
    {
        if((int)id > EnumBattleActiveSkillID.EnumSize)
        {
            Debug.Log("idがBattleActiveSkillIDの最大値を超えています");
            throw new Exception();
        }

        BattleActiveSkill skill = skillList[(int)id];
        Debug.Log(invoker.CharaClass.CharaName + "の" + skill.SkillName);

        switch (skill.SkillType)
        {
            case E_SkillType.攻撃:
                NormalElementAttack(invoker, target, skill.RateOrValue, skill.Element);
                break;
            case E_SkillType.HP回復:
                NormalRecoverHp(target, skill.RateOrValue);
                break;
            case E_SkillType.ATKバフ:
                BuffAttackStatus(target, skill.RateOrValue, skill.EffectTurn);
                break;







            case E_SkillType.その他:
                switch (skill.ID)
                {
                    case E_BattleActiveSkill.ファイアA:
                        //特別な処理
                        break;
                }
                break;
        }
    }

    private void NormalElementAttack(BattleCharacter attacker, List<BattleCharacter> target, double rate, E_Element element) //rateは通常攻撃を1としたときの威力,elementは技の属性
    {
        double damageValue = attacker.Atk * attacker.ToDamageRate[element] * rate;
        foreach(BattleCharacter bc in target)
        {
            double damage = bc.DecreaseHp(damageValue);
            Debug.Log(bc.CharaClass.CharaName + "は" + (int)damage + "のダメージを受けた");
        }
    }
    /*private void NormalElementAttack(BattleCharacter attacker, List<BattleCharacter> target, BattleActiveSkill skill) 
    {
        double damageValue = attacker.Atk * attacker.ToDamageRate[skill.Element] * skill.RateOrValue;
        damageValue = target.DecreaseHp(damageValue);
        Debug.Log(target.CharaClass.CharaName + "は" + (int)damageValue + "のダメージを受けた");
    }*/
    private void NormalRecoverHp(List<BattleCharacter> target, double value)
    {
        foreach (BattleCharacter bc in target)
        {
            double recoverValue = bc.RecoverHp(value);
            Debug.Log(bc.CharaClass.CharaName + "は" + (int)recoverValue + "体力を回復した");
        }
    }
    private void BuffAttackStatus(List<BattleCharacter> target, double rate, int effectTurn)
    {
        foreach(BattleCharacter bc in target)
        {
            bc.AddAtkRate(rate, effectTurn);
        }
    }
}
