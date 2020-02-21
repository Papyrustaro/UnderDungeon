using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattlePassiveEffectsFunc : MonoBehaviour
{
    [SerializeField]
    private List<BattlePassiveSkill> skillList = new List<BattlePassiveSkill>();
    [SerializeField]
    private List<BattlePassiveItem> itemList = new List<BattlePassiveItem>();

    public void EffectFunc(E_BattlePassiveSkill skillID, List<BattleCharacter> target)
    {
        EffectFunc(GetBattlePassiveSkill(skillID), target);
    }

    /// <summary>
    /// PassiveEffectの効果適用
    /// </summary>
    /// <param name="effect">適用するBattlePassiveEffect</param>
    /// <param name="target">効果対象</param>
    public void EffectFunc(BattlePassiveEffect effect, List<BattleCharacter> target)
    {
        Debug.Log(effect.EffectName);
        switch (effect.EffectType)
        {
            case E_BattlePassiveEffectType.最大Hp増減:
                EffectToAllTarget(effect, target, AddMaxHpRate);
                break;
            case E_BattlePassiveEffectType.最大Atk増減:
                EffectToAllTarget(effect, target, AddMaxAtkRate);
                break;
            case E_BattlePassiveEffectType.最大Spd増減:
                EffectToAllTarget(effect, target, AddMaxSpdRate);
                break;
            case E_BattlePassiveEffectType.与ダメージ増減:
                EffectToAllTarget(effect, target, AddToDamageRate);
                break;
            case E_BattlePassiveEffectType.被ダメージ増減:
                EffectToAllTarget(effect, target, AddFromDamageRate);
                break;
            case E_BattlePassiveEffectType.通常攻撃与ダメージ増減:
                EffectToAllTarget(effect, target, AddToNormalAttackRate);
                break;
            case E_BattlePassiveEffectType.通常攻撃被ダメージ増減:
                EffectToAllTarget(effect, target, AddFromNormalAttackRate);
                break;
            case E_BattlePassiveEffectType.通常攻撃回数増加:
                EffectToAllTarget(effect, target, AddNormalAttackNum);
                break;
            case E_BattlePassiveEffectType.Sp回復量増加:
                EffectToAllTarget(effect, target, AddHealSpPower);
                break;
            case E_BattlePassiveEffectType.開始時Sp増加:
                EffectToAllTarget(effect, target, AddSpBeforeBattle);
                break;
            case E_BattlePassiveEffectType.防御時攻撃集中:
                EffectToAllTarget(effect, target, SetAttractInDefending);
                break;
            case E_BattlePassiveEffectType.防御時被ダメージ軽減:
                EffectToAllTarget(effect, target, AddFromDamageRateInDefending);
                break;

            case E_BattlePassiveEffectType.その他常時:
            case E_BattlePassiveEffectType.その他開始時:
                EffectToAllTarget(effect, target, effect.OtherFunc);
                break;
        }
    }

    public BattlePassiveSkill GetBattlePassiveSkill(E_BattlePassiveSkill id)
    {
        return this.skillList[(int)id];
    }

    private void AddMaxHpRate(BattleCharacter target, BattlePassiveEffect effect)
    {
        target.PassiveMaxHpRate *= effect.RateOrValue;
    }
    private void AddMaxAtkRate(BattleCharacter target, BattlePassiveEffect effect)
    {
        target.PassiveAtkRate *= effect.RateOrValue;
    }
    private void AddMaxSpdRate(BattleCharacter target, BattlePassiveEffect effect)
    {
        target.PassiveSpdRate *= effect.RateOrValue;
    }
    private void AddToDamageRate(BattleCharacter target, BattlePassiveEffect effect)
    {
        ElementClass.AddRate(target.PassiveToDamageRate, effect.EffectElement, effect.RateOrValue);
    }
    private void AddFromDamageRate(BattleCharacter target, BattlePassiveEffect effect)
    {
        ElementClass.AddRate(target.PassiveFromDamageRate, effect.EffectElement, effect.RateOrValue);
    }
    private void AddToNormalAttackRate(BattleCharacter target, BattlePassiveEffect effect)
    {
        target.PassiveToNormalAttackRate *= effect.RateOrValue;
    }
    private void AddFromNormalAttackRate(BattleCharacter target, BattlePassiveEffect effect)
    {
        target.PassiveFromNormalAttackRate *= effect.RateOrValue;
    }
    private void AddNormalAttackNum(BattleCharacter target, BattlePassiveEffect effect)
    {
        target.PassiveNormalAttackNum += (int)effect.RateOrValue;
    }
    private void AddHealSpPower(BattleCharacter target, BattlePassiveEffect effect)
    {
        target.PassiveHealSpInTurn += (int)effect.RateOrValue;
    }
    private void AddSpBeforeBattle(BattleCharacter target, BattlePassiveEffect effect)
    {
        target.AddHaveSkillPoint((int)effect.RateOrValue);
    }
    private void SetAttractInDefending(BattleCharacter target, BattlePassiveEffect effect)
    {
        ElementClass.SetTrueFlag(target.PassiveAttractInDefending, effect.EffectElement);
    }
    private void AddFromDamageRateInDefending(BattleCharacter target, BattlePassiveEffect effect)
    {
        target.PassiveFromDamageRateInDefending *= effect.RateOrValue;
    }

    /// <summary>
    /// passiveEffectをtargetListの中から条件を満たすtargetだけに反映させる
    /// </summary>
    /// <param name="effect">反映させるBattlePassiveEffect</param>
    /// <param name="targetList">効果対象</param>
    /// <param name="func">呼ぶ関数</param>
    private void EffectToAllTarget(BattlePassiveEffect effect, List<BattleCharacter> targetList, Action<BattleCharacter, BattlePassiveEffect> func)
    {
        foreach (BattleCharacter target in ElementClass.GetListInElementByCondition(targetList, effect.TargetElement)) //属性の条件
        {
            if (!target.IsAlive) continue; //倒れているキャラに効果は反映しない

            switch (effect.EffectCondition) //属性以外の条件
            {
                case E_BattlePassiveEffectCondition.AnyTime:
                    break;
                case E_BattlePassiveEffectCondition.HpHigher:
                    if (target.Condition.Hp / target.Condition.MaxHp < effect.ConditionValue) continue;
                    break;
                case E_BattlePassiveEffectCondition.HpLower:
                    if (target.Condition.Hp / target.Condition.MaxHp > effect.ConditionValue) continue;
                    break;
                case E_BattlePassiveEffectCondition.SpHigher:
                    if (target.Condition.Sp < effect.ConditionValue) continue;
                    break;
                case E_BattlePassiveEffectCondition.SpLower:
                    if (target.Condition.Sp > effect.ConditionValue) continue;
                    break;
                case E_BattlePassiveEffectCondition _:
                    Debug.Log("error");
                    continue;
            }
            func(target, effect);
        }
    }

    /// <summary>
    /// passiveEffectをtargetListの中から条件を満たすtargetだけに反映させる
    /// </summary>
    /// <param name="effect">反映させるBattlePassiveEffect</param>
    /// <param name="targetList">効果対象</param>
    /// <param name="func">呼ぶ関数</param>

    private void EffectToAllTarget(BattlePassiveEffect effect, List<BattleCharacter> targetList, Action<BattleCharacter> func)
    {
        foreach (BattleCharacter target in ElementClass.GetListInElementByCondition(targetList, effect.TargetElement)) //属性の条件
        {
            if (!target.IsAlive) continue; //倒れているキャラに効果は反映しない

            switch (effect.EffectCondition) //属性以外の条件
            {
                case E_BattlePassiveEffectCondition.AnyTime:
                    break;
                case E_BattlePassiveEffectCondition.HpHigher:
                    if (target.Condition.Hp / target.Condition.MaxHp < effect.ConditionValue) continue;
                    break;
                case E_BattlePassiveEffectCondition.HpLower:
                    if (target.Condition.Hp / target.Condition.MaxHp > effect.ConditionValue) continue;
                    break;
                case E_BattlePassiveEffectCondition.SpHigher:
                    if (target.Condition.Sp < effect.ConditionValue) continue;
                    break;
                case E_BattlePassiveEffectCondition.SpLower:
                    if (target.Condition.Sp > effect.ConditionValue) continue;
                    break;
                case E_BattlePassiveEffectCondition _:
                    Debug.Log("error");
                    continue;
            }
            func(target);
        }
    }
}
