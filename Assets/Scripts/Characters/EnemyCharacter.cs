using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemyクラス。Inspector上で操作するためCharacterを継承していない。ステータス倍率の書き換えは、処理をかえたほうがいいかも
/// </summary>
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

    private bool finishSetStatus = false;
    private BaseEnemyAI enemyAI;

    private void Awake()
    {
    }
    private void Start()
    {
        this.charaClass = GetComponent<Character>();
    }

    public E_CharacterID ID => CharaClass.ID;
    public string CharaName => CharaClass.CharaName;
    public int MaxHp => CharaClass.MaxHp;
    public int MaxAtk => CharaClass.MaxAtk;
    public int MaxSpd => CharaClass.MaxSpd;
    public E_Element Element => CharaClass.Element;
    public E_CharaRarity Rarity => CharaClass.Rarity;
    public string Description => CharaClass.Description;
    public double HpRate => this.hpRate;
    public double AtkRate => this.atkRate;
    public double SpdRate => this.spdRate;
    public int DropRate => this.dropRate;
    public E_BattleActiveSkill[] HaveBattleActtiveSkillID => this.haveBattleActiveSkillID;
    public bool HaveSpecialAI => this.haveSpecialAI;
    public Character CharaClass
    {
        get
        {
            if (charaClass == null) this.charaClass = GetComponent<Character>();
            if (!this.finishSetStatus) SetStatusRate();
            return charaClass;
        }
    }

    private void SetStatusRate()
    {
        this.finishSetStatus = true;
        charaClass.MaxHp = (int)(MaxHp * hpRate);
        charaClass.MaxAtk = (int)(MaxAtk * atkRate);
        charaClass.MaxSpd = (int)(MaxSpd * spdRate);
    }
    public BaseEnemyAI EnemyAI
    {
        get
        {
            if (this.enemyAI == null) this.enemyAI = GetComponent<BaseEnemyAI>();
            return this.enemyAI;
        }
    }
}
