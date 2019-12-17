using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 全てのキャラクターの元となるクラス */
public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private new string name;
    [SerializeField]
    private int id;
    [SerializeField]
    private int maxHp, maxAtk, maxDef, maxSpAtk, maxSpDef, maxSpd;
    [SerializeField]
    private EnumElement element;
    [SerializeField]
    private int rarity;
    [SerializeField]
    private string description;

    public Character(string name, int id, int maxHp, int maxAtk, int maxDef, int maxSpAtk,
        int maxSpDef, int maxSpd, EnumElement element, int rarity, string description)
    {
        this.name = name;  this.id = id; this.maxHp = maxHp; this.maxAtk = maxAtk; this.maxDef = maxDef; this.maxSpAtk = maxSpAtk;
        this.maxSpDef = maxSpDef; this.maxSpd = maxSpd; this.element = element; this.rarity = rarity; this.description = description;
    }

    public int ID => id;
    public string Name => name;
    public int MaxHp => maxHp;
    public int MaxAtk => maxAtk;
    public int MaxDef => maxDef;
    public int MaxSpAtk => maxSpAtk;
    public int MaxSpDef => maxSpDef;
    public int MaxSpd => maxSpd;
    public EnumElement Element => element;
    public int Rarity => rarity;
    public string Description => description;
}
