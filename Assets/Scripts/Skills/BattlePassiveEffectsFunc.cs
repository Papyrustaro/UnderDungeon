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

    public void EffectFunc(BattlePassiveEffect effect, BattleCharacter invoker, List<BattleCharacter> target)
    {
        Debug.Log(invoker.CharaClass.CharaName + "の" + effect.EffectName);
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
            case E_BattlePassiveEffectType.開始時Sp増減:
                EffectToAllTarget(effect, target, AddSpBeforeBattle);
                break;
            case E_BattlePassiveEffectType.防御時攻撃集中:
                EffectToAllTarget(effect, target, SetAttractInDefending);
                break;
            case E_BattlePassiveEffectType.防御時被ダメージ軽減:
                EffectToAllTarget(effect, target, AddFromDamageRateInDefending);
                break;
            case E_BattlePassiveEffectType.開始時ActiveSkill発動:
                //GetComposeでBattleActiveSkillを読み取って発動?
                break;

            case E_BattlePassiveEffectType.その他: //ActiveSkillのほうで全部やって、こちらはいらない?
                break;
        }
    }

    private void AddMaxHpRate(BattleCharacter target, BattlePassiveEffect effect)
    {

    }
    private void AddMaxAtkRate(BattleCharacter target, BattlePassiveEffect effect)
    {

    }
    private void AddMaxSpdRate(BattleCharacter target, BattlePassiveEffect effect)
    {

    }
    private void AddToDamageRate(BattleCharacter target, BattlePassiveEffect effect)
    {

    }
    private void AddFromDamageRate(BattleCharacter target, BattlePassiveEffect effect)
    {

    }
    private void AddToNormalAttackRate(BattleCharacter target, BattlePassiveEffect effect)
    {

    }
    private void AddFromNormalAttackRate(BattleCharacter target, BattlePassiveEffect effect)
    {

    }
    private void AddNormalAttackNum(BattleCharacter target, BattlePassiveEffect effect)
    {

    }
    private void AddHealSpPower(BattleCharacter target, BattlePassiveEffect effect)
    {

    }
    private void AddSpBeforeBattle(BattleCharacter target, BattlePassiveEffect effect)
    {

    }
    private void SetAttractInDefending(BattleCharacter target, BattlePassiveEffect effect)
    {

    }
    private void AddFromDamageRateInDefending(BattleCharacter target, BattlePassiveEffect effect)
    {

    }

    private void EffectToAllTarget(BattlePassiveEffect effect, BattleCharacter invoker, List<BattleCharacter> targetList, Action<BattleCharacter, BattleCharacter, BattlePassiveEffect> func)
    {
        foreach (BattleCharacter target in ElementClass.GetListInElement(targetList, effect.TargetElement))
        {
            if (!target.IsAlive) continue; //とりあえず倒れているキャラに効果は付与しないことにする
            func(invoker, target, effect);
        }
    }
    private void EffectToAllTarget(BattlePassiveEffect effect, List<BattleCharacter> targetList, Action<BattleCharacter, BattlePassiveEffect> func)
    {
        foreach (BattleCharacter target in ElementClass.GetListInElement(targetList, effect.TargetElement))
        {
            if (!target.IsAlive) continue; //とりあえず倒れているキャラに効果は付与しないことにする
            func(target, effect);
        }
    }
}
