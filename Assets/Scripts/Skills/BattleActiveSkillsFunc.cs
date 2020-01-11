using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleActiveSkillsFunc : MonoBehaviour
{
    [SerializeField]
    private List<BattleActiveSkill> skillList = new List<BattleActiveSkill>();

    public void SkillFunc(E_BattleActiveSkill skillID, BattleCharacter invoker, List<BattleCharacter> target)
    {
        SkillFunc(skillList[(int)skillID], invoker, target);
    }

    public void SkillFunc(BattleActiveSkill skill, BattleCharacter invoker, List<BattleCharacter> target)
    {
        Debug.Log(invoker.CharaClass.CharaName + "の" + skill.SkillName);

        switch (skill.SkillType)
        {
            case E_SkillType.攻撃:
                SkillToAllTarget(skill, invoker, target, NormalElementAttack);
                break;
            case E_SkillType.固定ダメージ:
                SkillToAllTarget(skill, target, FixedDamageAttack);
                break;
            case E_SkillType.HP回復:
                SkillToAllTarget(skill, target, NormalRecoverHp);
                break;
            case E_SkillType.ATKバフ:
                SkillToAllTarget(skill, target, BuffAtkStatus);
                break;
            case E_SkillType.SPDバフ:
                SkillToAllTarget(skill, target, BuffSpdStatus);
                break;
            case E_SkillType.HPバフ:
                SkillToAllTarget(skill, target, BuffHpStatus);
                break;
            case E_SkillType.被ダメージ増減バフ:
                SkillToAllTarget(skill, target, BuffFromDamageRate);
                break;
            case E_SkillType.与ダメージ増減バフ:
                SkillToAllTarget(skill, target, BuffToDamageRate);
                break;
            case E_SkillType.スキルポイント増減:
                SkillToAllTarget(skill, target, AddHaveSkillPoint);
                break;
            case E_SkillType.属性変化:
                SkillToAllTarget(skill, target, SetElementChanged);
                break;
            case E_SkillType.復活付与:
                SkillToAllTarget(skill, target, SetRebornEffect);
                break;
            case E_SkillType.無敵付与:
                SkillToAllTarget(skill, target, SetNoGetDamaged);
                break;
            case E_SkillType.攻撃集中:
                SkillToAllTarget(skill, target, SetAttractingAffect);
                break;
            case E_SkillType.カウンター:
                SkillToAllTarget(skill, invoker, target, CounterAttack);
                break;
            case E_SkillType.通常攻撃全体攻撃化:
                SkillToAllTarget(skill, target, AddNormalAttackToAllTurn);
                break;
            case E_SkillType.通常攻撃被ダメージ増減:
                SkillToAllTarget(skill, target, AddFromNormalAttackRate);
                break;
            case E_SkillType.通常攻撃与ダメージ増減:
                SkillToAllTarget(skill, target, AddToNormalAttackRate);
                break;
            case E_SkillType.通常攻撃回数追加:
                SkillToAllTarget(skill, target, AddNormalAttackNum);
                break;
            case E_SkillType.攻撃集中被ダメ減:
                SkillToAllTarget(skill, target, SetAttractingAffect);
                SkillToAllTarget(skill, target, BuffFromDamageRate);
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
    public BattleActiveSkill GetBattleActiveSkill(E_BattleActiveSkill id)
    {
        return this.skillList[(int)id];
    }
    private void NormalElementAttack(BattleCharacter attacker, BattleCharacter target, BattleActiveSkill skill) 
    {
        target.DecreaseHp(attacker.Atk * attacker.ToDamageRate[skill.SkillElement] * skill.RateOrValue * target.FromDamageRate[skill.SkillElement]);
    }
    private void CounterAttack(BattleCharacter attacker, BattleCharacter target, BattleActiveSkill skill) 
    {
        target.DecreaseHp(attacker.HaveDamageThisTurn * attacker.ToDamageRate[skill.SkillElement] * skill.RateOrValue * target.FromDamageRate[skill.SkillElement]);
    }
    private void NormalRecoverHp(BattleCharacter target, BattleActiveSkill skill)
    {
        target.RecoverHp(skill.RateOrValue);
    }
    private void FixedDamageAttack(BattleCharacter target, BattleActiveSkill skill)
    {
        target.DecreaseHp(skill.RateOrValue);
    }
    private void BuffAtkStatus(BattleCharacter target, BattleActiveSkill skill)
    {
        target.AddAtkRate(skill.RateOrValue, skill.EffectTurn);
    }
    private void BuffHpStatus(BattleCharacter target, BattleActiveSkill skill)
    {
        target.AddHpRate(skill.RateOrValue, skill.EffectTurn);
    }
    private void BuffSpdStatus(BattleCharacter target, BattleActiveSkill skill)
    {
        target.AddSpdRate(skill.RateOrValue, skill.EffectTurn);
    }
    private void BuffToDamageRate(BattleCharacter target, BattleActiveSkill skill)
    {
        target.AddToDamageRate(skill.SkillElement, skill.RateOrValue, skill.EffectTurn);
    }
    private void BuffFromDamageRate(BattleCharacter target, BattleActiveSkill skill)
    {
        target.AddFromDamageRate(skill.SkillElement, skill.RateOrValue, skill.EffectTurn);
    }
    private void SetNoGetDamaged(BattleCharacter target, BattleActiveSkill skill)
    {
        target.AddNoGetDamaged(skill.SkillElement, skill.EffectTurn);
    }
    private void SetElementChanged(BattleCharacter target, BattleActiveSkill skill)
    {
        target.SetElementChanged(skill.SkillElement, skill.EffectTurn);
    }
    private void SetRebornEffect(BattleCharacter target, BattleActiveSkill skill)
    {
        target.CanReborn = skill.RateOrValue;
    }
    private void SetAttractingAffect(BattleCharacter target, BattleActiveSkill skill)
    {
        target.AddAttractEffectTurn(skill.SkillElement, skill.EffectTurn);
    }
    private void AddHaveSkillPoint(BattleCharacter target, BattleActiveSkill skill)
    {
        target.AddHaveSkillPoint((int)skill.RateOrValue);
    }
    private void AddNormalAttackToAllTurn(BattleCharacter target, BattleActiveSkill skill)
    {
        target.NormalAttackToAllTurn += skill.EffectTurn;
    }
    private void AddToNormalAttackRate(BattleCharacter target, BattleActiveSkill skill)
    {
        target.AddToNormalAttackRate(skill.RateOrValue, skill.EffectTurn);
    }
    private void AddFromNormalAttackRate(BattleCharacter target, BattleActiveSkill skill)
    {
        target.AddFromNormalAttackRate(skill.RateOrValue, skill.EffectTurn);
    }
    private void AddNormalAttackNum(BattleCharacter target, BattleActiveSkill skill)
    {
        target.AddNormalAttackNum((int)skill.RateOrValue, skill.EffectTurn);
    }

    private void SkillToAllTarget(BattleActiveSkill skill, BattleCharacter invoker, List<BattleCharacter> targetList, Action<BattleCharacter, BattleCharacter, BattleActiveSkill> func)
    {
        foreach (BattleCharacter target in ElementClass.GetListInElement(targetList, skill.TargetElement))
        {
            func(invoker, target, skill);
        }
    }
    private void SkillToAllTarget(BattleActiveSkill skill, List<BattleCharacter> targetList, Action<BattleCharacter, BattleActiveSkill> func)
    {
        foreach(BattleCharacter target in ElementClass.GetListInElement(targetList, skill.TargetElement))
        {
            func(target, skill);
        }
    }
}
