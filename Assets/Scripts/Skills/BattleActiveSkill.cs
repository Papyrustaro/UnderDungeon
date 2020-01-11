using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BattleActiveSkill : MonoBehaviour
{
    [SerializeField]
    private E_BattleActiveSkill id;
    [SerializeField]
    private E_SkillType skillType;
    [SerializeField]
    private double rateOrValue = 0; //攻撃の場合は倍率、回復の場合は回復量、その他の場合は0
    [SerializeField]
    private int effectTurn = 0; //効果ターン(必要のないものは0)
    [SerializeField]
    private string description; //スキルの説明
    [SerializeField]
    private int needTurn; //必要なターン
    [SerializeField]
    private E_Element skillElement; //技の属性
    [SerializeField]
    private E_Element targetElement = E_Element.FireAquaTree; //対象とする属性
    [SerializeField]
    private E_TargetType targetType;

    public E_BattleActiveSkill ID => this.id;
    public string SkillName => this.id.ToString();
    //public string Description => description;
    public int NeedTurn => this.needTurn;
    public E_Element SkillElement => this.skillElement;
    public E_Element TargetElement => this.targetElement;
    public E_TargetType TargetType => this.targetType;
    public bool NeedTarget => this.targetType == E_TargetType.OneAlly; //プレイヤーにターゲットをきくかどうか
    public double RateOrValue => this.rateOrValue;
    public E_SkillType SkillType => this.skillType;
    public int EffectTurn => this.effectTurn;
    public string Description
    {
        get
        {
            if (this.description != "" || this.skillType == E_SkillType.その他) return this.description;

            
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

            switch (this.skillType)
            {
                case E_SkillType.ATKバフ:
                    return targetText + "の攻撃力を" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                case E_SkillType.HPバフ:
                    return targetText + "の体力を" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                case E_SkillType.SPDバフ:
                    return targetText + "の素早さを" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                case E_SkillType.カウンター:
                    return "このターン受けたダメージの" + this.RateOrValue + "倍の" + ElementClass.GetStringElement(this.skillElement) + "属性ダメージを" + targetText + "に与える";
                case E_SkillType.スキルポイント増減:
                    if (this.rateOrValue < 0) return targetText + "のスキルポイントを" + (int)this.rateOrValue + "減らす";
                    else return targetText + "のスキルポイントを" + (int)this.rateOrValue + "増やす";
                case E_SkillType.与ダメージ増減バフ:
                    if (this.skillElement == E_Element.FireAquaTree) return targetText + "の与えるダメージを" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                    else return targetText + "の" + ElementClass.GetStringElement(this.skillElement) + "属性に与えるダメージを" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                case E_SkillType.属性変化:
                    return targetText + "の属性を" + this.effectTurn + "ターン" + ElementClass.GetStringElement(this.skillElement) + "属性に変化させる";
                case E_SkillType.復活付与:
                    return targetText + "に最大体力の" + (int)(this.rateOrValue * 100) + "%で復活する効果を付与させる";
                case E_SkillType.攻撃:
                    return targetText + "に攻撃力の" + this.rateOrValue + "倍の" + ElementClass.GetStringElement(this.skillElement) + "属性攻撃をする";
                case E_SkillType.攻撃集中:
                    if (this.skillElement == E_Element.FireAquaTree) return targetText + "に" + this.effectTurn + "ターン攻撃を集中させる";
                    else return targetText + "に" + this.effectTurn + "ターン" + ElementClass.GetStringElement(this.skillElement) + "属性攻撃を集中させる";
                case E_SkillType.攻撃集中被ダメ減:
                    if (this.skillElement == E_Element.FireAquaTree) return targetText + "に" + this.effectTurn + "ターン攻撃を集中させ、被ダメージを" + this.rateOrValue + "倍にする";
                    else return targetText + "に" + effectTurn + "ターン" + ElementClass.GetStringElement(skillElement) + "属性攻撃を集中させ、" + ElementClass.GetStringElement(skillElement) + "属性からの被ダメ－ジを" + this.rateOrValue + "倍にする";
                case E_SkillType.無敵付与:
                    if (this.skillElement == E_Element.FireAquaTree) return targetText + "に" + this.effectTurn + "ターン攻撃を無効にする効果を付与する";
                    else return targetText + "に" + this.effectTurn + "ターン" + ElementClass.GetStringElement(skillElement) + "属性攻撃を無効にする効果を付与する";
                case E_SkillType.被ダメージ増減バフ:
                    if (this.skillElement == E_Element.FireAquaTree) return targetText + "の被ダメージを" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                    else return targetText + "の" + ElementClass.GetStringElement(this.skillElement) + "属性から受けるダメージを" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                case E_SkillType.通常攻撃与ダメージ増減:
                    return targetText + "の通常攻撃で与えるダメージを" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                case E_SkillType.通常攻撃全体攻撃化:
                    return targetText + "の通常攻撃を" + this.effectTurn + "ターン全体攻撃にする";
                case E_SkillType.通常攻撃回数追加:
                    return targetText + "の通常攻撃回数を" + this.effectTurn + "ターン" + this.rateOrValue + "回増やす";
                case E_SkillType.通常攻撃被ダメージ増減:
                    return targetText + "の通常攻撃で受けるダメージを" + this.effectTurn + "ターン" + this.rateOrValue + "倍にする";
                case E_SkillType.固定ダメージ:
                    return targetText + "に" + (int)this.rateOrValue + "の固定ダメージを与える";
                case E_SkillType _:
                    return "エラー";
                
            }
        }
    }
}
