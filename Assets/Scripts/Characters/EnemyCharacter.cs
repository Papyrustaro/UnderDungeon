using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField]
    private double hpRate, atkRate, defRate, spAtkRate, spDefRate, spdRate; //各ステータス倍率
    [SerializeField]
    private bool haveSpecialAI; //特別な行動AIがあるかどうか
    [SerializeField]
    private E_ActiveSkillID[] activeSkillID; //基本行動の敵の使用スキル
    [SerializeField]
    private int dropRate; //ドロップ率(%)

    public EnemyCharacter(string name, int id, int maxHp, int maxAtk, int maxDef, int maxSpAtk,
        int maxSpDef, int maxSpd, E_Element element, int rarity, string description, bool haveSpecialAI, int dropRate)
        : base(name, id, maxHp, maxAtk, maxDef, maxSpAtk, maxSpDef, maxSpd, element, rarity, description)
    {
        this.haveSpecialAI = haveSpecialAI; this.dropRate = dropRate;
    }

    public bool HaveSpecialAI => haveSpecialAI;
    public int DropRate => dropRate;
}
