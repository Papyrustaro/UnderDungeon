using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_BattlePassiveEffectType
{
    最大Hp増減,
    最大Atk増減,
    最大Spd増減,
    与ダメージ増減,
    被ダメージ増減,
    開始時Sp増加, //開始時だ、ということをどう判断するか→BattleManagerから直接呼ぶ？
    通常攻撃回数増加,
    通常攻撃与ダメージ増減,
    通常攻撃被ダメージ増減,
    Sp回復量増加, //強すぎる気もする。バランス崩れそう→厳しい条件ならアリ？
    防御時被ダメージ軽減,
    防御時攻撃集中,
    その他常時,
    その他開始時,
}

public enum E_BattlePassiveEffectCondition //PassiveEffect発動条件(ElementはTargetElementで管理)
{
    AnyTime,
    HpHigher,
    HpLower,
    SpHigher,
    SpLower,
}
public class BattlePassiveEffect : MonoBehaviour
{
    [SerializeField]
    private E_BattlePassiveEffectType effectType;
    [SerializeField]
    private double rateOrValue = 0;
    [SerializeField]
    private E_Element targetElement = E_Element.FireAquaTree; //対象とする属性
    [SerializeField]
    private E_TargetType targetType = E_TargetType.AllAlly;
    [SerializeField]
    private E_BattlePassiveEffectCondition effectCondition;
    [SerializeField]
    private E_Element effectElement = E_Element.FireAquaTree; //技の属性
    [SerializeField]
    private double conditionValue = 0; //条件の値(例: Hp50%以上→conditionValue=0.5)
    [SerializeField]
    private string description; //スキルの説明

    public E_Element EffectElement => this.effectElement;
    public E_Element TargetElement => this.targetElement;
    public E_TargetType TargetType => this.targetType;
    public double RateOrValue => this.rateOrValue;
    public E_BattlePassiveEffectType EffectType => this.effectType;
    public E_BattlePassiveEffectCondition EffectCondition => this.effectCondition;
    public double ConditionValue => this.conditionValue;
    public virtual string EffectName { get { return "error"; } }
    public virtual void OtherFunc(BattleCharacter target) { Debug.Log("error"); }
    public string Description
    {
        get
        {
            if (this.description != "" || this.effectType == E_BattlePassiveEffectType.その他常時 || this.effectType == E_BattlePassiveEffectType.その他開始時) return this.description;


            string targetText = "";
            switch (this.effectCondition)
            {
                case E_BattlePassiveEffectCondition.HpHigher:
                    targetText = "HP" + conditionValue * 100 + "%以上の";
                    break;
                case E_BattlePassiveEffectCondition.HpLower:
                    targetText = "HP" + conditionValue * 100 + "%以下の";
                    break;
                case E_BattlePassiveEffectCondition.SpHigher:
                    targetText = "SP" + conditionValue * 100 + "%以上の";
                    break;
                case E_BattlePassiveEffectCondition.SpLower:
                    targetText = "SP" + conditionValue * 100 + "%以下の";
                    break;
                case E_BattlePassiveEffectCondition _:
                    break;
            }

            if (this.targetElement != E_Element.FireAquaTree) targetText += ElementClass.GetStringElement(this.targetElement) + "属性の";

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
                case E_BattlePassiveEffectType.最大Hp増減:
                    return targetText + "の最大HP" + RateOrValue + "倍";
                case E_BattlePassiveEffectType.最大Atk増減:
                    return targetText + "の最大ATK" + RateOrValue + "倍";
                case E_BattlePassiveEffectType.最大Spd増減:
                    return targetText + "の最大SPD" + RateOrValue + "倍";
                case E_BattlePassiveEffectType.与ダメージ増減:
                    if (this.effectElement == E_Element.FireAquaTree) return targetText + "の与えるダメージ" + this.rateOrValue + "倍";
                    else return targetText + "の" + ElementClass.GetStringElement(this.effectElement) + "属性に与えるダメージ" + this.rateOrValue + "倍";
                case E_BattlePassiveEffectType.被ダメージ増減:
                    if (this.effectElement == E_Element.FireAquaTree) return targetText + "の被ダメージ" + this.rateOrValue + "倍";
                    else return targetText + "の" + ElementClass.GetStringElement(this.effectElement) + "属性から受けるダメージ" + this.rateOrValue + "倍";
                case E_BattlePassiveEffectType.通常攻撃与ダメージ増減:
                    return targetText + "の通常攻撃与ダメージ" + this.rateOrValue + "倍";
                case E_BattlePassiveEffectType.通常攻撃被ダメージ増減:
                    return targetText + "の通常攻撃被ダメージ" + this.rateOrValue + "倍";
                case E_BattlePassiveEffectType.通常攻撃回数増加:
                    return targetText + "の通常攻撃回数+" + this.rateOrValue;
                case E_BattlePassiveEffectType.Sp回復量増加:
                    return targetText + "のSP回復量+" + this.rateOrValue;
                case E_BattlePassiveEffectType.開始時Sp増加:
                    return targetText + "の開始時SP+" + this.rateOrValue;
                case E_BattlePassiveEffectType.防御時攻撃集中:
                    if (this.effectElement == E_Element.FireAquaTree) return targetText + "防御時攻撃集中";
                    else return targetText + "防御時" +  ElementClass.GetStringElement(this.effectElement) + "属性の攻撃集中";
                case E_BattlePassiveEffectType.防御時被ダメージ軽減:
                    return targetText + "の防御時被ダメージ" + this.rateOrValue + "倍";
                case E_BattlePassiveEffectType _:
                    return "エラー";

            }
        }
    }

    /// <summary>
    /// passiveEffectの効果適用が戦闘開始時のみか
    /// </summary>
    /// <returns></returns>
    public bool EffectIsOnlyFirst()
    {
        return this.effectType == E_BattlePassiveEffectType.開始時Sp増加 || this.effectType == E_BattlePassiveEffectType.その他開始時; //「その他」の判断はどうする？
    }
}
