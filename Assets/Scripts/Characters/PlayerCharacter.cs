using System.Collections;
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
    private E_ActiveSkillID[] haveActiveSkillID; 
    [SerializeField]
    private int[] useAbleActiveSkillLV;
    [SerializeField]
    private E_PassiveSkillID[] havePassiveSkillID;
    [SerializeField]
    private int[] useAblePassiveSkillLV;
    [SerializeField]
    private E_CompositeExp compositeExp;
    [SerializeField]
    private E_SellPrice sellPrice;

    private void Awake()
    {
        this.charaClass = GetComponent<Character>();
    }
    private void Start()
    {
        Debug.Log("rarity: " + this.Rarity);
    }
    public int ID => charaClass.ID;
    public string CharaName => charaClass.CharaName;
    public int MaxHp => charaClass.MaxHp;
    public int MaxAtk => charaClass.MaxAtk;
    public int MaxDef => charaClass.MaxDef;
    public int MaxSpAtk => charaClass.MaxSpAtk;
    public int MaxSpDef => charaClass.MaxSpDef;
    public E_Element Element => charaClass.Element;
    public int Rarity => charaClass.Rarity;
    public string Description => charaClass.Description;
    public int HaveExp => this.haveExp;
    public int ExceedLimitNum => this.exceedLimitNum;
    public E_ActiveSkillID[] HaveActiveSkillID => this.haveActiveSkillID;
    public int[] UseAbleActiveSkillLV => this.useAbleActiveSkillLV;
    public E_PassiveSkillID[] HavePassiveSkillID => this.havePassiveSkillID;
    public int[] UseAblePassiveSkillLV => this.useAblePassiveSkillLV;
    public E_CompositeExp CompositeExp => this.compositeExp;
    public E_SellPrice SellPrice => this.sellPrice;


    public int GetMaxExp()
    {
        return LVTable.GetMaxExp(LVTable.GetMaxLV(this.Rarity, this.exceedLimitNum));
    }
    public int GetMaxLV()
    {
        return LVTable.GetMaxLV(this.Rarity, this.exceedLimitNum);
    }
    public int GetLV()
    {
        return LVTable.GetLV(this.haveExp, LVTable.GetMaxLV(this.Rarity, this.exceedLimitNum));
    }

    public bool AddExp(int getExp) //MaxExpを超える:false, 超えない:true
    {
        int maxExp = LVTable.GetMaxExp(LVTable.GetMaxLV(Rarity, exceedLimitNum));
        if (haveExp + getExp > maxExp)
        {
            haveExp = maxExp;
            return false;
        }
        else
        {
            haveExp += getExp;
            return true;
        }
    }
}
