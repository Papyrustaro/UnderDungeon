using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField]
    private int exp;
    [SerializeField]
    private int exceedLimitNum; //限界突破数
    [SerializeField]
    private E_ActiveSkillID[] haveActiveSkillID = default; 
    [SerializeField]
    private int[] useAbleActiveSkillLV = default;
    [SerializeField]
    private E_PassiveSkillID[] havePassiveSkillID = default;
    [SerializeField]
    private int[] useAblePassiveSkillLV = default;
    [SerializeField]
    private E_CompositeExp compositeExp;
    [SerializeField]
    private E_SellPrice sellPrice;

    public PlayerCharacter(string name, int id, int maxHp, int maxAtk, int maxDef, int maxSpAtk, 
        int maxSpDef, int maxSpd, E_Element element, int rarity, string description, int exp)
        : base(name, id, maxHp, maxAtk, maxDef, maxSpAtk, maxSpDef, maxSpd, element, rarity, description)
    {
        this.exp = exp;
    }

    public int Exp => exp;

    public bool AddExp(int getExp) //MaxExpを超える:false, 超えない:true
    {
        int maxExp = LVTable.GetMaxExp(LVTable.GetMaxLV(Rarity, exceedLimitNum));
        if (exp + getExp > maxExp)
        {
            exp = maxExp;
            return false;
        }
        else
        {
            exp += getExp;
            return true;
        }
    }
}
