using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 戦闘中のキャラクター状態保持クラス */
public class BattleCharacter : MonoBehaviour
{
    private List<BuffEffect> hpRate = new List<BuffEffect>();
    private List<BuffEffect> atkRate = new List<BuffEffect>();
    private List<BuffEffect> spdRate = new List<BuffEffect>();
    private List<BuffEffect> toDamageRate = new List<BuffEffect>();
    private List<BuffEffect> fromDamageRate = new List<BuffEffect>();
    private List<BuffEffect> noGetDamaged = new List<BuffEffect>();
    private BuffEffect elementChange;
    private List<BuffEffect> normalAttackNum = new List<BuffEffect>(); //通常攻撃回数
    private List<BuffEffect> normalAttackRate = new List<BuffEffect>(); //通常攻撃の倍率
    //private bool isEnemy = false;

    private E_BattleActiveSkill[] battleActiveSkillID;
    private Character charaClass;
    private double hp;
    

    /* passiveのみ考慮したプロパティ */
    public double PassiveMaxHp { get; set; } //passive考慮したステータス charaClass.maxHP * battlePassiveSkill * itemPassiveSkill
    public double PassiveAtk { get; set; }
    public double PassiveSpd { get; set; }
    public Dictionary<E_Element, double> PassiveToDamageRate { get; set; }
    public Dictionary<E_Element, double> PassiveFromDamageRate { get; set; }
    public double PassiveNormalAttackRate { get; set; } = 1;


    /*  passiveとactiveを反映したプロパティ */
    public double MaxHp => PassiveMaxHp * GetRate(hpRate); //現在の(全て考慮した最終的な)最大HP
    public double Atk => PassiveAtk * GetRate(atkRate);
    public double Spd => PassiveSpd * GetRate(spdRate);
    public Dictionary<E_Element, double> ToDamageRate => GetRate(PassiveToDamageRate, toDamageRate);
    public Dictionary<E_Element, double> FromDamageRate => GetRate(PassiveFromDamageRate, fromDamageRate);
    public double NormalAttackRate => PassiveNormalAttackRate * GetRate(this.normalAttackRate);



    /* その他プロパティ */
    public bool StatusChange { get; set; } = false;
    public double Hp { get { return this.hp; } set { this.hp = value; StatusChange = true; } } //現在のHP
    public Character CharaClass
    {
        get
        {
            if (charaClass == null) SetCharacter();
            return charaClass;
        }
    }
    //private int[] skillTurnFromActivate = new int[4]; //ActiveSkillのスキル発動までのターン
    public bool CanReborn { get; set; } //復活できる状態か
    public bool Reborned { get; set; } //復活効果を使ったか
    public bool IsAttractingAffect { get; set; } //敵の攻撃を自分に集めているか
    public double HaveDamageThisTurn { get; set; } //1ターンで喰らったダメージ量
    public bool CanWholeAttack { get; set; } //全体攻撃効果が付与されているか
    public bool IsEnemy { get; set; } = false;
    //public double NormalAttackRate { get; set; } = 1.0; //通常攻撃の倍率

    private void Awake()
    {
    }
    private void Start()
    {
        SetCharacter();
        SetBaseStatus();
        if (!IsEnemy)
        {
            SetPassiveEffect(); //Passiveスキルの効果を反映
        }
        StatusChange = true;
    }
    private void SetBaseStatus()
    {
        PassiveMaxHp = charaClass.MaxHp;
        PassiveAtk = charaClass.MaxAtk;
        PassiveSpd = charaClass.MaxSpd;
        PassiveToDamageRate = new Dictionary<E_Element, double>() { { E_Element.Fire, 1.0 }, { E_Element.Aqua, 1.0 }, { E_Element.Tree, 1.0 } };
        PassiveFromDamageRate = new Dictionary<E_Element, double>() { { E_Element.Fire, 1.0 }, { E_Element.Aqua, 1.0 }, { E_Element.Tree, 1.0 } };
        Hp = MaxHp;
    }
    private void SetPassiveEffect()
    {
        //本当はここで、BattlePassiveSkillとItemによる能力上昇がある
    }
    private void SetCharacter()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            this.charaClass = GetComponent<PlayerCharacter>().CharaClass;
        }
        else
        {
            this.charaClass = GetComponent<EnemyCharacter>().CharaClass;
            this.IsEnemy = true;
        }
    }
    
    public double GetRate(List<BuffEffect> bfList) //buffEffectのすべての倍率を計算して返す
    {
        double rate = 1.0;
        foreach(BuffEffect bf in bfList)
        {
            rate *= bf.Rate;
        }
        return rate;
    }
    public Dictionary<E_Element, double> GetRate(Dictionary<E_Element, double> passive, List<BuffEffect> activeList) //各属性ダメージのpassiveとactiveを計算して返す
    {
        double fireRate = passive[E_Element.Fire], aquaRate = passive[E_Element.Aqua], treeRate = passive[E_Element.Tree];

        foreach (BuffEffect bf in activeList)
        {
            if (ElementClass.IsFire(bf.Element)) fireRate *= bf.Rate;
            if (ElementClass.IsAqua(bf.Element)) aquaRate *= bf.Rate;
            if (ElementClass.IsTree(bf.Element)) treeRate *= bf.Rate;
        }

        return new Dictionary<E_Element, double>() { { E_Element.Fire, fireRate }, { E_Element.Aqua, aquaRate }, { E_Element.Tree, treeRate } };
    }
    public void AddHpRate(double rate, int effectTurn)
    {
        this.hpRate.Add(new BuffEffect(rate, effectTurn));
    }
    public void AddAtkRate(double rate, int effectTurn)
    {
        this.atkRate.Add(new BuffEffect(rate, effectTurn));
    }
    public void AddSpdRate(double rate, int effectTurn)
    {
        this.spdRate.Add(new BuffEffect(rate, effectTurn));
    }
    public void AddToDamageRate(E_Element element, double rate, int effectTurn)
    {
        this.toDamageRate.Add(new BuffEffect(element, rate, effectTurn));
    }
    public void AddFromDamageRate(E_Element element, double rate, int effectTurn)
    {
        this.fromDamageRate.Add(new BuffEffect(element, rate, effectTurn));
    }
    public void AddNoGetDamaged(E_Element element, int effectTurn)
    {
        this.noGetDamaged.Add(new BuffEffect(element, 0, effectTurn));
    }

    public double RecoverHp(double value)
    {
        if(Hp + value > MaxHp)
        {
            double diff = MaxHp - Hp;
            //Debug.Log(charaClass.CharaName + "の体力が満タンになった");
            Hp = MaxHp;
            return diff;
        }
        else
        {
            //Debug.Log(charaClass.CharaName + "の体力が" + value + "回復した");
            Hp += value;
            return value;
        }
    }

    public double DecreaseHp(double damage_value)
    {
        if(Hp - damage_value <= 0)
        {
            double diff = Hp;
            //Debug.Log(charaClass.CharaName + "は" + Hp + "のダメージを受けた");
            Hp = 0;
            return diff;
        }
        else
        {
            //Debug.Log(charaClass.CharaName + "は" + damage_value + "のダメージを受けた");
            Hp -= damage_value;
            return damage_value;
        }
    }
}
