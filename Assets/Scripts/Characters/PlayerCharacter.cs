using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField]
    private int exp;
    [SerializeField]
    private int maxExp;
    [SerializeField]
    private int[] haveActiveSkillId = default; 
    [SerializeField]
    private int[] useAbleActiveSkillExp = default;
    [SerializeField]
    private int[] havePassiveSkillId = default;
    [SerializeField]
    private int[] useAblePassiveSkillExp = default;

    public PlayerCharacter(string name, int id, int maxHp, int maxAtk, int maxDef, int maxSpAtk, 
        int maxSpDef, int maxSpd, EnumElement element, int rarity, string description, int exp, int maxExp)
        : base(name, id, maxHp, maxAtk, maxDef, maxSpAtk, maxSpDef, maxSpd, element, rarity, description)
    {
        this.exp = exp; this.maxExp = maxExp;
    }

    public int Exp => exp;
    public int MaxExp => maxExp;

    public bool AddExp(int value) //MaxExpを超える:false, 超えない:true
    {
        if(exp + value > maxExp)
        {
            exp = value;
            return false;
        }
        else
        {
            exp += value;
            return true;
        }
    }
}
