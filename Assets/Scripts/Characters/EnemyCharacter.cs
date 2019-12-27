using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    private Character charaClass;
    [SerializeField]
    private double hpRate = 1.0, atkRate = 1.0, spdRate = 1.0; //各ステータス倍率
    [SerializeField]
    private bool haveSpecialAI; //特別な行動AIがあるかどうか
    [SerializeField]
    private E_BattleActiveSkill[] haveBattleActiveSkillID; //基本行動の敵の使用スキル
    [SerializeField]
    private int dropRate = 50; //ドロップ率(%)

    private void Awake()
    {
        this.charaClass = GetComponent<Character>();
        SetStatusRate(); //ステータス倍率を反映
    }

    private void SetStatusRate()
    {
        charaClass.MaxHp = (int)(MaxHp * hpRate);
        charaClass.MaxAtk = (int)(MaxAtk * atkRate);
        charaClass.MaxSpd = (int)(MaxSpd * spdRate);
    }

    public E_CharacterID ID => charaClass.ID;
    public string CharaName => charaClass.CharaName;
    public int MaxHp => charaClass.MaxHp;
    public int MaxAtk => charaClass.MaxAtk;
    public int MaxSpd => charaClass.MaxSpd;
    public E_Element Element => charaClass.Element;
    public int Rarity => charaClass.Rarity;
    public string Description => charaClass.Description;
    public double HpRate => this.hpRate;
    public double AtkRate => this.atkRate;
    public double SpdRate => this.spdRate;
    public int DropRate => this.dropRate;
    public E_BattleActiveSkill[] HaveBattleActtiveSkillID => this.haveBattleActiveSkillID;
    public bool HaveSpecialAI => this.haveSpecialAI;
    public Character CharaClass => this.charaClass;
}
