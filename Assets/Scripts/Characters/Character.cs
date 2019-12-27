using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 全てのキャラクターの元となるクラス */
public class Character : MonoBehaviour
{
    [SerializeField]
    private E_CharacterID id;
    [SerializeField]
    private int maxHp, maxAtk, maxSpd;
    [SerializeField]
    private E_Element element;
    [SerializeField]
    private int rarity;
    [SerializeField]
    private string description;
    //private Image image;

    public E_CharacterID ID => id;
    public string CharaName => this.id.ToString();
    public int MaxHp { get { return this.maxHp; } set { this.maxHp = value; } }
    public int MaxAtk { get { return this.maxAtk; } set { this.maxAtk = value; } }
    public int MaxSpd { get { return this.maxSpd; } set { this.maxSpd = value; } }
    public E_Element Element => element;
    public int Rarity => rarity;
    public string Description => description;
}
