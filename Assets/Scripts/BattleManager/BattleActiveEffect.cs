using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_BattleActiveEffectType
{
    攻撃, //RateOrValueが1以下でHP割合ダメージ
    HP回復, //RateOrValueが1以下でHP割合回復
    被ダメージ増減バフ,
    与ダメージ増減バフ,
    HPバフ,
    ATKバフ,
    SPDバフ, //デバフも倍率と対象変えるだけ
    スキルポイント増減,
    攻撃集中,
    属性変化,
    復活付与,
    無敵付与,
    カウンター,
    通常攻撃全体攻撃化,
    通常攻撃被ダメージ増減,
    通常攻撃与ダメージ増減,
    通常攻撃回数追加,
    攻撃集中被ダメ減,
    固定ダメージ,
    HPリジェネ,
    SPリジェネ,
    その他,
}
public class BattleActiveEffect : MonoBehaviour
{
    [SerializeField]
    private E_BattleActiveEffectType effectType;
    [SerializeField]
    private double rateOrValue = 0; //攻撃の場合は倍率、回復の場合は回復量、その他の場合は0
    [SerializeField]
    private int effectTurn = 0; //効果ターン(必要のないものは0)
    [SerializeField]
    private string description; //スキルの説明
    [SerializeField]
    private E_Element effectElement; //技の属性
    [SerializeField]
    private E_Element targetElement = E_Element.FireAquaTree; //対象とする属性
    [SerializeField]
    private E_TargetType targetType;

    /// <summary>
    /// 技の属性
    /// </summary>
    public E_Element EffectElement => this.effectElement;

    /// <summary>
    /// 対象とする属性条件
    /// </summary>
    public E_Element TargetElement => this.targetElement;
    public E_TargetType TargetType => this.targetType;
    public double RateOrValue => this.rateOrValue;
    public E_BattleActiveEffectType EffectType => this.effectType;
    public int EffectTurn => this.effectTurn;
    public virtual string EffectName { get { return "error"; } }
    public virtual void OtherFunc(BattleCharacter invoker, List<BattleCharacter> target) { Debug.Log("error"); }
    public string Description
    {
        get
        {
            if (this.description != "" || this.effectType == E_BattleActiveEffectType.その他) return this.description;


            string targetText = "";
            if (this.targetElement != E_Element.FireAquaTree) targetText = ElementClass.GetStringElement(this.targetElement) + "属性の";

            switch (this.targetType)
            {
                case E_TargetType.AllAlly: targetText += "味方全体"; break;
                case E_TargetType.AllEnemy: targetText += "敵全体"; break;
                case E_TargetType.OneAlly: targetText = "味方一体"; break;
                case E_TargetType.OneEnemy: targetText = "敵一体"; break;
                case E_TargetType.Self: targetText = "自身"; break;
                case E_TargetType.All: targetText += "全体"; break;
                case E_TargetType.RandomAll: targetText += "全体からランダム"; break;
                case E_TargetType.RandomAlly: targetText += "味方からランダム"; break;
                case E_TargetType.RandomEnemy: targetText += "敵からランダム"; break;
                case E_TargetType _: targetText = "エラー"; break;
            }

            switch (this.effectType)
            {
                case E_BattleActiveEffectType.ATKバフ:
                    return targetText + "の攻撃力を" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                case E_BattleActiveEffectType.HPバフ:
                    return targetText + "の体力を" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                case E_BattleActiveEffectType.SPDバフ:
                    return targetText + "の素早さを" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                case E_BattleActiveEffectType.カウンター:
                    return "このターン受けたダメージの" + this.RateOrValue + "倍の" + ElementClass.GetStringElement(this.effectElement) + "属性ダメージを" + targetText + "に与える";
                case E_BattleActiveEffectType.スキルポイント増減:
                    if (this.rateOrValue < 0) return targetText + "のスキルポイントを" + (int)this.rateOrValue + "減らす";
                    else return targetText + "のスキルポイントを" + (int)this.rateOrValue + "増やす";
                case E_BattleActiveEffectType.与ダメージ増減バフ:
                    if (this.effectElement == E_Element.FireAquaTree) return targetText + "の与えるダメージを" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                    else return targetText + "の" + ElementClass.GetStringElement(this.effectElement) + "属性に与えるダメージを" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                case E_BattleActiveEffectType.属性変化:
                    return targetText + "の属性を" + this.effectTurn + "ターン" + ElementClass.GetStringElement(this.effectElement) + "属性に変化させる";
                case E_BattleActiveEffectType.復活付与:
                    return targetText + "に最大体力の" + (int)(this.rateOrValue * 100) + "%で復活する効果を付与させる";
                case E_BattleActiveEffectType.攻撃:
                    return targetText + "に攻撃力の" + this.rateOrValue + "倍の" + ElementClass.GetStringElement(this.effectElement) + "属性攻撃をする";
                case E_BattleActiveEffectType.攻撃集中:
                    if (this.effectElement == E_Element.FireAquaTree) return targetText + "に" + this.effectTurn + "ターン攻撃を集中させる";
                    else return targetText + "に" + this.effectTurn + "ターン" + ElementClass.GetStringElement(this.effectElement) + "属性攻撃を集中させる";
                case E_BattleActiveEffectType.攻撃集中被ダメ減:
                    if (this.effectElement == E_Element.FireAquaTree) return targetText + "に" + this.effectTurn + "ターン攻撃を集中させ、被ダメージを" + this.rateOrValue + "倍にする";
                    else return targetText + "に" + effectTurn + "ターン" + ElementClass.GetStringElement(effectElement) + "属性攻撃を集中させ、" + ElementClass.GetStringElement(effectElement) + "属性からの被ダメ－ジを" + this.rateOrValue + "倍にする";
                case E_BattleActiveEffectType.無敵付与:
                    if (this.effectElement == E_Element.FireAquaTree) return targetText + "に" + this.effectTurn + "ターン攻撃を無効にする効果を付与する";
                    else return targetText + "に" + this.effectTurn + "ターン" + ElementClass.GetStringElement(effectElement) + "属性攻撃を無効にする効果を付与する";
                case E_BattleActiveEffectType.被ダメージ増減バフ:
                    if (this.effectElement == E_Element.FireAquaTree) return targetText + "の被ダメージを" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                    else return targetText + "の" + ElementClass.GetStringElement(this.effectElement) + "属性から受けるダメージを" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                case E_BattleActiveEffectType.通常攻撃与ダメージ増減:
                    return targetText + "の通常攻撃で与えるダメージを" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                case E_BattleActiveEffectType.通常攻撃全体攻撃化:
                    return targetText + "の通常攻撃を" + this.effectTurn + "ターン全体攻撃にする";
                case E_BattleActiveEffectType.通常攻撃回数追加:
                    return targetText + "の通常攻撃回数を" + this.effectTurn + "ターン" + this.rateOrValue + "回増やす";
                case E_BattleActiveEffectType.通常攻撃被ダメージ増減:
                    return targetText + "の通常攻撃で受けるダメージを" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                case E_BattleActiveEffectType.固定ダメージ:
                    return targetText + "に" + (int)this.rateOrValue + "の固定ダメージを与える";
                case E_BattleActiveEffectType _:
                    return "エラー";

            }
        }
    }

    /// <summary>
    /// (発動者への効果ターンを1増やすため)EffectTurnの値を変化させる
    /// </summary>
    /// <param name="addTurnValue"></param>
    public void ChangeEffectTurn(int addTurnValue)
    {
        this.effectTurn += addTurnValue;
    }
}
