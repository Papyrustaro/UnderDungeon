using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 全てのキャラクターの元となるクラス */
public class Character : MonoBehaviour
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string charaName;
    [SerializeField]
    private int maxHp, maxAtk, maxDef, maxSpAtk, maxSpDef, maxSpd;
    [SerializeField]
    private E_Element element;
    [SerializeField]
    private int rarity;
    [SerializeField]
    private string description;
    //private Image image;

    public int ID => id;
    public string CharaName => charaName;
    public int MaxHp => maxHp;
    public int MaxAtk => maxAtk;
    public int MaxDef => maxDef;
    public int MaxSpAtk => maxSpAtk;
    public int MaxSpDef => maxSpDef;
    public int MaxSpd => maxSpd;
    public E_Element Element => element;
    public int Rarity => rarity;
    public string Description => description;
}
