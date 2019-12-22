﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private Character charaClass;
    [SerializeField]
    private int haveExp; // 獲得している経験値
    [SerializeField]
    private int exceedLimitNum; //限界突破数
    [SerializeField]
    private E_BattleActiveSkill[] haveBattleActiveSkillID;
    [SerializeField]
    private int[] useAbleBattleActiveSkillLV;
    [SerializeField]
    private E_BattlePassiveSkill[] haveBattlePassiveSkillID;
    [SerializeField]
    private int[] useAbleBattlePassiveSkillLV;
    [SerializeField]
    private E_DungeonActiveSkill[] haveDungeonActiveSkillID;
    [SerializeField]
    private int[] useAbleDungeonActiveSkillLV;
    [SerializeField]
    private E_DungeonPassiveSkill[] haveDungeonPassiveSkillID;
    [SerializeField]
    private int[] useAbleDungeonPassiveSkillLV;
    [SerializeField]
    private E_CompositeExp compositeExp;
    [SerializeField]
    private E_SellPrice sellPrice;
    [SerializeField]
    private int cost; //パーティに入れる際のコスト

    private void Awake()
    {
        this.charaClass = GetComponent<Character>();
    }
    private void Start()
    {
        Debug.Log("MaxLV: " + this.MaxLV);
        Debug.Log("MaxExp: " + this.MaxExp);
    }
    public E_CharacterID ID => charaClass.ID;
    public string CharaName => charaClass.CharaName;
    public int MaxHp => charaClass.MaxHp;
    public int MaxAtk => charaClass.MaxAtk;
    public int MaxSpd => charaClass.MaxSpd;
    public E_Element Element => charaClass.Element;
    public int Rarity => charaClass.Rarity;
    public string Description => charaClass.Description;
    public int HaveExp => this.haveExp;
    public int ExceedLimitNum => this.exceedLimitNum;
    public E_BattleActiveSkill[] HaveBattleActiveSkillID => this.haveBattleActiveSkillID;
    public int[] UseAbleBattleActiveSkillLV => this.useAbleBattleActiveSkillLV;
    public E_BattlePassiveSkill[] HaveBattlePassiveSkillID => this.haveBattlePassiveSkillID;
    public int[] UseAbleBattlePassiveSkillLV => this.useAbleBattlePassiveSkillLV;
    public E_DungeonActiveSkill[] HaveDungeonActiveSkillID => this.haveDungeonActiveSkillID;
    public int[] UseAbleDungeonActiveSkillLV => this.useAbleDungeonActiveSkillLV;
    public E_DungeonPassiveSkill[] HaveDungeonPassiveSkillID => this.haveDungeonPassiveSkillID;
    public int[] UseAbleDungeonPassiveSkillLV => this.useAbleDungeonPassiveSkillLV;
    public E_CompositeExp CompositeExp => this.compositeExp;
    public E_SellPrice SellPrice => this.sellPrice;
    public int Cost => this.cost;
    public int MaxLV => LVTable.GetMaxLV(this.Rarity, this.exceedLimitNum);
    public int LV => LVTable.GetLV(this.haveExp, this.MaxLV);
    public int MaxExp => LVTable.GetMaxExp(this.MaxLV);

    public bool AddExp(int getExp) //MaxExpを超える:false, 超えない:true
    {
        if (this.haveExp + getExp > this.MaxLV)
        {
            this.haveExp = this.MaxLV;
            return false;
        }
        else
        {
            this.haveExp += getExp;
            return true;
        }
    }
}
