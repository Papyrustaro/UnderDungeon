using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField]
    private int exp;
    [SerializeField]
    private int maxExp;

    public PlayerCharacter(int race, string name, int id, int maxHp, int maxAtk, int maxDef, int maxSpAtk, 
        int maxSpDef, int maxSpd, int element, int rarity, string description, int exp, int maxExp)
        : base(race, name, id, maxHp, maxAtk, maxDef, maxSpAtk, maxSpDef, maxSpd, element, rarity, description)
    {
        this.exp = exp; this.maxExp = maxExp;
    }

    public int Exp => exp;
    public int MaxExp => maxExp;
}
