using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField]
    private bool haveSpecialAI; //特別な行動AIがあるかどうか
    [SerializeField]
    private int dropRate; //ドロップ率(%)
    [SerializeField]
    private int haveExp; //獲得できる経験値
    [SerializeField]
    private int haveGold; //獲得できるゴールド

    public EnemyCharacter(string name, int id, int maxHp, int maxAtk, int maxDef, int maxSpAtk,
        int maxSpDef, int maxSpd, EnumElement element, int rarity, string description, bool haveSpecialAI, int dropRate,
        int haveExp, int haveGold)
        : base(name, id, maxHp, maxAtk, maxDef, maxSpAtk, maxSpDef, maxSpd, element, rarity, description)
    {
        this.haveSpecialAI = haveSpecialAI; this.dropRate = dropRate;
        this.haveExp = haveExp; this.haveGold = haveGold;
    }

    public bool HaveSpecialAI => haveSpecialAI;
    public int DropRate => dropRate;
    public int HaveExp => haveExp;
    public int HaveGold => haveGold;
}
