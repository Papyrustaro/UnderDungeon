using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleActiveEffectsFunc : MonoBehaviour
{
    [SerializeField]
    private List<BattleActiveSkill> skillList = new List<BattleActiveSkill>();
    [SerializeField]
    private List<BattleActiveItem> itemList = new List<BattleActiveItem>();

    public void SkillFunc(E_BattlePassiveSkill skillID, BattleCharacter invoker, List<BattleCharacter> target)
    {
        EffectFunc(skillList[(int)skillID], invoker, target);
    }
    public void ItemFunc(E_BattlePassiveItem itemID, BattleCharacter invoker, List<BattleCharacter> target)
    {
        EffectFunc(itemList[(int)itemID], invoker, target);
    }

    /// <summary>
    /// ActiveEffectの効果適用
    /// </summary>
    /// <param name="effect">適用するActiveEffect</param>
    /// <param name="invoker">(skill,item)発動者</param>
    /// <param name="target">効果対象</param>
    public void EffectFunc(BattleActiveEffect effect, BattleCharacter invoker, List<BattleCharacter> target)
    {
        Debug.Log(invoker.CharaClass.CharaName + "の" + effect.EffectName);

        switch (effect.EffectType)
        {
            case E_BattleActiveEffectType.攻撃:
                EffectToAllTarget(effect, invoker, target, Attack);
                break;
            case E_BattleActiveEffectType.固定ダメージ:
                EffectToAllTarget(effect, invoker, target, FixedDamageAttack);
                break;
            case E_BattleActiveEffectType.HP回復:
                EffectToAllTarget(effect, invoker, target, RecoverHp);
                break;
            case E_BattleActiveEffectType.ATKバフ:
                EffectToAllTarget(effect, invoker, target, BuffAtkStatus);
                break;
            case E_BattleActiveEffectType.SPDバフ:
                EffectToAllTarget(effect, invoker, target, BuffSpdStatus);
                break;
            case E_BattleActiveEffectType.HPバフ:
                EffectToAllTarget(effect, invoker, target, BuffHpStatus);
                break;
            case E_BattleActiveEffectType.被ダメージ増減バフ:
                EffectToAllTarget(effect, invoker, target, BuffFromDamageRate);
                break;
            case E_BattleActiveEffectType.与ダメージ増減バフ:
                EffectToAllTarget(effect, invoker, target, BuffToDamageRate);
                break;
            case E_BattleActiveEffectType.スキルポイント増減:
                EffectToAllTarget(effect, invoker, target, AddHaveSkillPoint);
                break;
            case E_BattleActiveEffectType.属性変化:
                EffectToAllTarget(effect, invoker, target, SetElementChanged);
                break;
            case E_BattleActiveEffectType.復活付与:
                EffectToAllTarget(effect, invoker, target, SetRebornEffect);
                break;
            case E_BattleActiveEffectType.無敵付与:
                EffectToAllTarget(effect, invoker, target, SetNoGetDamaged);
                break;
            case E_BattleActiveEffectType.攻撃集中:
                EffectToAllTarget(effect, invoker, target, SetAttractingAffect);
                break;
            case E_BattleActiveEffectType.カウンター:
                EffectToAllTarget(effect, invoker, target, CounterAttack);
                break;
            case E_BattleActiveEffectType.通常攻撃全体攻撃化:
                EffectToAllTarget(effect, invoker, target, AddNormalAttackToAllTurn);
                break;
            case E_BattleActiveEffectType.通常攻撃被ダメージ増減:
                EffectToAllTarget(effect, invoker, target, AddFromNormalAttackRate);
                break;
            case E_BattleActiveEffectType.通常攻撃与ダメージ増減:
                EffectToAllTarget(effect, invoker, target, AddToNormalAttackRate);
                break;
            case E_BattleActiveEffectType.通常攻撃回数追加:
                EffectToAllTarget(effect, invoker, target, AddNormalAttackNum);
                break;
            case E_BattleActiveEffectType.攻撃集中被ダメ減:
                EffectToAllTarget(effect, invoker, target, SetAttractingAffect);
                EffectToAllTarget(effect, invoker, target, BuffFromDamageRate);
                break;
            case E_BattleActiveEffectType.HPリジェネ:
                EffectToAllTarget(effect, invoker, target, AddHpRegeneration);
                break;
            case E_BattleActiveEffectType.SPリジェネ:
                EffectToAllTarget(effect, invoker, target, AddSpRegeneration);
                break;


            case E_BattleActiveEffectType.その他:
                effect.OtherFunc(invoker, target);
                break;
        }
    }

    /// <summary>
    /// idからBattleActiveSkillを取得
    /// </summary>
    /// <param name="id">取得するBattleActiveSkillのId</param>
    /// <returns>取得したBattleActiveSkill</returns>
    public BattleActiveSkill GetBattleActiveSkill(E_BattleActiveSkill id)
    {
        return this.skillList[(int)id];
    }
    private void Attack(BattleCharacter attacker, BattleCharacter target, BattleActiveEffect effect) 
    {
        target.DamagedByElementAttack(attacker.Atk * attacker.GetToDamageRate(effect.EffectElement) * effect.RateOrValue, effect.EffectElement);
    }

    /// <summary>
    /// 自身のこのターン受けた総ダメージ量*power(atk * toDamageRate * effectRate)のダメージ
    /// </summary>
    /// <param name="attacker">攻撃者</param>
    /// <param name="target">攻撃対象</param>
    /// <param name="effect">BattleActiveEffect</param>
    private void CounterAttack(BattleCharacter attacker, BattleCharacter target, BattleActiveEffect effect) 
    {
        target.DamagedByElementAttack(attacker.HaveDamageThisTurn * attacker.GetToDamageRate(effect.EffectElement) * effect.RateOrValue, effect.EffectElement);
    }

    /// <summary>
    /// 固定値or割合Hp回復(effect.RateOrValueが1未満で最大Hpの割合回復、1以上で固定値回復)
    /// </summary>
    /// <param name="target"></param>
    /// <param name="effect"></param>
    private void RecoverHp(BattleCharacter target, BattleActiveEffect effect)
    {
        if (effect.RateOrValue > 1) target.RecoverHp(effect.RateOrValue);
        else target.RecoverHpByRate(effect.RateOrValue);
    }

    /// <summary>
    /// 固定or割合ダメージ(effect.RateOrValueが1未満でtargetの最大Hpに対する割合ダメージ、1以上で固定ダメージ)
    /// </summary>
    /// <param name="target">攻撃対象</param>
    /// <param name="effect">BattleActiveEffect</param>
    private void FixedDamageAttack(BattleCharacter target, BattleActiveEffect effect) //固定ダメージ(RateOrValueが1以下で最大HPの割合ダメージ)
    {
        if (effect.RateOrValue > 1) target.DecreaseHp(effect.RateOrValue);
        else target.DecreaseHpByRate(effect.RateOrValue);
    }
    private void BuffAtkStatus(BattleCharacter target, BattleActiveEffect effect)
    {
        target.AddAtkRate(effect.RateOrValue, effect.EffectTurn);
    }
    private void BuffHpStatus(BattleCharacter target, BattleActiveEffect effect)
    {
        target.AddHpRate(effect.RateOrValue, effect.EffectTurn);
    }
    private void BuffSpdStatus(BattleCharacter target, BattleActiveEffect effect)
    {
        target.AddSpdRate(effect.RateOrValue, effect.EffectTurn);
    }
    private void BuffToDamageRate(BattleCharacter target, BattleActiveEffect effect)
    {
        target.AddToDamageRate(effect.EffectElement, effect.RateOrValue, effect.EffectTurn);
    }
    private void BuffFromDamageRate(BattleCharacter target, BattleActiveEffect effect)
    {
        target.AddFromDamageRate(effect.EffectElement, effect.RateOrValue, effect.EffectTurn);
    }
    private void SetNoGetDamaged(BattleCharacter target, BattleActiveEffect effect)
    {
        target.AddNoGetDamaged(effect.EffectElement, effect.EffectTurn);
    }
    private void SetElementChanged(BattleCharacter target, BattleActiveEffect effect)
    {
        target.SetElementChanged(effect.EffectElement, effect.EffectTurn);
    }
    private void SetRebornEffect(BattleCharacter target, BattleActiveEffect effect)
    {
        target.RebornHpRate = effect.RateOrValue;
    }
    private void SetAttractingAffect(BattleCharacter target, BattleActiveEffect effect)
    {
        target.AddAttractEffectTurn(effect.EffectElement, effect.EffectTurn);
    }
    private void AddHaveSkillPoint(BattleCharacter target, BattleActiveEffect effect)
    {
        target.AddHaveSkillPoint((int)effect.RateOrValue);
    }
    private void AddNormalAttackToAllTurn(BattleCharacter target, BattleActiveEffect effect)
    {
        target.NormalAttackToAllTurn += effect.EffectTurn;
    }
    private void AddToNormalAttackRate(BattleCharacter target, BattleActiveEffect effect)
    {
        target.AddToNormalAttackRate(effect.RateOrValue, effect.EffectTurn);
    }
    private void AddFromNormalAttackRate(BattleCharacter target, BattleActiveEffect effect)
    {
        target.AddFromNormalAttackRate(effect.RateOrValue, effect.EffectTurn);
    }
    private void AddNormalAttackNum(BattleCharacter target, BattleActiveEffect effect)
    {
        target.AddNormalAttackNum((int)effect.RateOrValue, effect.EffectTurn);
    }
    private void AddHpRegeneration(BattleCharacter target, BattleActiveEffect effect)
    {
        target.AddHpRegeneration(effect.RateOrValue, effect.EffectTurn);
    }
    private void AddSpRegeneration(BattleCharacter target, BattleActiveEffect effect)
    {
        target.AddSpRegeneration((int)effect.RateOrValue, effect.EffectTurn);
    }

    /// <summary>
    /// targetList全てに対しactiveEffectを発動する(倒れているtargetは除く)
    /// </summary>
    /// <param name="effect">発動するBattleActiveEffect</param>
    /// <param name="invoker">発動者</param>
    /// <param name="targetList">効果対象</param>
    /// <param name="func">呼ぶ関数</param>
    private void EffectToAllTarget(BattleActiveEffect effect, BattleCharacter invoker, List<BattleCharacter> targetList, Action<BattleCharacter, BattleCharacter, BattleActiveEffect> func)
    {
        foreach (BattleCharacter target in ElementClass.GetListInElement(targetList, effect.TargetElement))
        {
            if (!target.IsAlive) continue; //とりあえず倒れているキャラに効果は付与しないことにする
            if(invoker == target) //発動者へのバフは+1ターンする(行動終了後に全effectTurnを経過させるため)
            {
                effect.ChangeEffectTurn(1);
                Debug.Log("効果対象が発動者と同じです");
                func(invoker, target, effect);
                effect.ChangeEffectTurn(-1);
            }
            else
            {
                func(invoker, target, effect);
            }
        }
    }

    /// <summary>
    /// targetList全てに対しactiveEffectを発動する(倒れているtargetは除く)
    /// </summary>
    /// <param name="effect">発動するBattleActiveEffect</param>
    /// <param name="invoker">発動者</param>
    /// <param name="targetList">効果対象</param>
    /// <param name="func">呼ぶ関数</param>
    private void EffectToAllTarget(BattleActiveEffect effect, BattleCharacter invoker, List<BattleCharacter> targetList, Action<BattleCharacter, BattleActiveEffect> func)
    {
        foreach(BattleCharacter target in ElementClass.GetListInElement(targetList, effect.TargetElement))
        {
            if (!target.IsAlive) continue; //とりあえず倒れているキャラに効果は付与しないことにする

            if (invoker == target) //発動者へのバフは+1ターンする(行動終了後に全effectTurnを経過させるため)
            {
                effect.ChangeEffectTurn(1);
                func(target, effect);
                effect.ChangeEffectTurn(-1);
            }
            else
            {
                func(target, effect);
            }
        }
    }
}
