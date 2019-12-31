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
    private E_Element element;
    [SerializeField]
    private E_TargetType targetType;

    public E_BattleActiveSkill ID => id;
    public string SkillName => id.ToString();
    public string Description => description;
    public int NeedTurn => needTurn;
    public E_Element Element => element;
    public E_TargetType TargetType => this.targetType;
    public bool NeedTarget => this.targetType == E_TargetType.OneAlly; //プレイヤーにターゲットをきくかどうか
    public double RateOrValue => this.rateOrValue;
    public E_SkillType SkillType => this.skillType;
    public int EffectTurn => this.effectTurn;
}
