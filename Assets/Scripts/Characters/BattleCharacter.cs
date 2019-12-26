﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 戦闘中のキャラクター状態保持クラス */
public class BattleCharacter : MonoBehaviour
{
    private List<BuffEffect> hpRate = new List<BuffEffect>();
    private List<BuffEffect> atkRate = new List<BuffEffect>();
    private List<BuffEffect> spdRate = new List<BuffEffect>();
    private List<BuffEffect> toDamageRate = new List<BuffEffect>();
    private List<BuffEffect> fromDamageRate = new List<BuffEffect>();
    private List<BuffEffect> noGetDamaged = new List<BuffEffect>();
    private BuffEffect elementChange;
    private List<BuffEffect> normalAttackNum = new List<BuffEffect>(); //通常攻撃回数
    private List<BuffEffect> normalAttackRate = new List<BuffEffect>(); //通常攻撃の倍率
    private bool isEnemy = false;
    private PlayerCharacter pc;
    private EnemyCharacter ec;

    public List<BuffEffect> HpRate => this.hpRate;
    public List<BuffEffect> SpdRate => this.spdRate;

    //private int[] skillTurnFromActivate = new int[4]; //ActiveSkillのスキル発動までのターン
    public bool CanReborn { get; set; } //復活できる状態か
    public bool Reborned { get; set; } //復活効果を使ったか
    public bool IsAttractingAffect { get; set; } //敵の攻撃を自分に集めているか
    public double HaveDamageThisTurn { get; set; } //1ターンで喰らったダメージ量
    public bool CanWholeAttack { get; set; } //全体攻撃効果が付与されているか
    public double NormalAttackRate { get; set; } = 1.0; //通常攻撃の倍率

    private void Awake()
    {
        SetCharacter();
    }
    public int Spd { get
        {
            if (this.isEnemy) return ec.MaxSpd;
            else return pc.MaxSpd;
        } }
    public void SetCharacter()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            pc = GetComponent<PlayerCharacter>();
        }
        else
        {
            ec = GetComponent<EnemyCharacter>();
            this.isEnemy = true;
        }
    }
    public void AddHpRate(int rate, int effectTurn)
    {
        this.hpRate.Add(new BuffEffect(rate, effectTurn));
    }
    public void AddAtkRate(int rate, int effectTurn)
    {
        this.atkRate.Add(new BuffEffect(rate, effectTurn));
    }
    public void AddSpdRate(int rate, int effectTurn)
    {
        this.spdRate.Add(new BuffEffect(rate, effectTurn));
    }
    public void AddToDamageRate(E_Element element, int rate, int effectTurn)
    {
        this.toDamageRate.Add(new BuffEffect(element, rate, effectTurn));
    }
    public void AddFromDamageRate(E_Element element, int rate, int effectTurn)
    {
        this.fromDamageRate.Add(new BuffEffect(element, rate, effectTurn));
    }
    public void AddNoGetDamaged(E_Element element, int effectTurn)
    {
        this.noGetDamaged.Add(new BuffEffect(element, 0, effectTurn));
    }
}
