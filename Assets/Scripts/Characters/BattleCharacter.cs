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
    private List<BuffEffect> toNormalAttackRate = new List<BuffEffect>(); //通常攻撃の与ダメージ倍率
    private List<BuffEffect> fromNormalAttackRate = new List<BuffEffect>();

    private int haveSkillPoint = 0;
    //private bool isEnemy = false;

    private List<E_BattleActiveSkill> battleActiveSkillID = new List<E_BattleActiveSkill>();
    private Character charaClass;
    //private double hp;
    private PlayerCharacter pc;
    private EnemyCharacter ec;
    

    /* passiveのみ考慮したプロパティ */
    public double PassiveMaxHp { get; set; } //passive考慮したステータス charaClass.maxHP * battlePassiveSkill * itemPassiveSkill
    public double PassiveAtk { get; set; }
    public double PassiveSpd { get; set; }
    public Dictionary<E_Element, double> PassiveToDamageRate { get; set; }
    public Dictionary<E_Element, double> PassiveFromDamageRate { get; set; }
    public double PassiveToNormalAttackRate { get; set; } = 1;
    public double PassiveFromNormalAttackRate { get; set; } = 1;


    /*  passiveとactiveを反映したプロパティ */
    public double MaxHp => PassiveMaxHp * GetRate(hpRate); //現在の(全て考慮した最終的な)最大HP
    public double Atk => PassiveAtk * GetRate(atkRate);
    public double Spd => PassiveSpd * GetRate(spdRate);
    public Dictionary<E_Element, double> ToDamageRate => GetRate(PassiveToDamageRate, toDamageRate);
    public Dictionary<E_Element, double> FromDamageRate => GetRate(PassiveFromDamageRate, fromDamageRate);
    public double ToNormalAttackRate => PassiveToNormalAttackRate * GetRate(this.toNormalAttackRate);
    public double FromNormalAttackRate => PassiveFromNormalAttackRate * GetRate(this.fromNormalAttackRate);



    /* その他プロパティ */
    public List<E_BattleActiveSkill> BattleActiveSkillID => this.battleActiveSkillID;
    public bool StatusChange { get; set; } = false;
    public double Hp { get; set; } //現在のHP
    public Character CharaClass
    {
        get
        {
            if (charaClass == null) SetCharacter();
            return charaClass;
        }
    }
    //private int[] skillTurnFromActivate = new int[4]; //ActiveSkillのスキル発動までのターン
    public double CanReborn { get; set; } = 0; //復活できる状態か(0で復活しない。0.1など復活したときのHP割合を保持)
    public bool Reborned { get; set; } //復活効果を使ったか
    public int AttractingAffectTurn { get; set; } = 0; //敵の攻撃を自分に集めているターン
    public int NormalAttackToAllTurn { get; set; } = 0; //通常攻撃が全体攻撃になるターン
    public double HaveDamageThisTurn { get; set; } //1ターンで喰らったダメージ量
    public bool CanWholeAttack { get; set; } //全体攻撃効果が付与されているか
    public bool IsEnemy { get; set; } = false;
    public int HaveSkillPoint => this.haveSkillPoint;
    public E_Element Element { get { if (this.elementChange == null) return CharaClass.Element; else return this.elementChange.Element; } }
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
        SetActiveSkill();
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
            this.pc = GetComponent<PlayerCharacter>();
            this.charaClass = pc.CharaClass;
        }
        else
        {
            this.ec = GetComponent<EnemyCharacter>();
            this.charaClass = ec.CharaClass;
            this.IsEnemy = true;
        }
    }
    private void SetActiveSkill()
    {
        if (!this.IsEnemy)
        {
            for(int i = 0; i < CharacterConstValue.MAX_HAVE_ACTIVE_SKILL; i++)
            {
                if (pc.LV >= pc.UseAbleBattleActiveSkillLV[i]) this.battleActiveSkillID.Add(pc.HaveBattleActiveSkillID[i]);
                else break;
            }
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
        Debug.Log(CharaClass.CharaName + "の体力" + rate + "倍");
        StatusChange = true;
    }
    public void AddAtkRate(double rate, int effectTurn)
    {
        this.atkRate.Add(new BuffEffect(rate, effectTurn));
        Debug.Log(CharaClass.CharaName + "の攻撃" + rate + "倍");
        StatusChange = true;
    }
    public void AddSpdRate(double rate, int effectTurn)
    {
        this.spdRate.Add(new BuffEffect(rate, effectTurn));
        Debug.Log(CharaClass.CharaName + "の素早さ" + rate + "倍");
        StatusChange = true;
    }
    public void AddToNormalAttackRate(double rate, int effectTurn)
    {
        this.toNormalAttackRate.Add(new BuffEffect(rate, effectTurn));
        Debug.Log(CharaClass.CharaName + "の通常攻撃の与ダメージ" + rate + "倍");
    }
    public void AddFromNormalAttackRate(double rate, int effectTurn)
    {
        this.fromNormalAttackRate.Add(new BuffEffect(rate, effectTurn));
        Debug.Log(CharaClass.CharaName + "の通常攻撃被ダメージ" + rate + "倍");
    }
    public void AddNormalAttackNum(int addNum, int effectTurn)
    {
        this.normalAttackNum.Add(new BuffEffect((double)addNum, effectTurn));
        Debug.Log(CharaClass.CharaName + "の通常攻撃回数+" + addNum);
    }
    public void AddToDamageRate(E_Element element, double rate, int effectTurn)
    {
        this.toDamageRate.Add(new BuffEffect(element, rate, effectTurn));
        Debug.Log(CharaClass.CharaName + "の" + element.ToString() + "に与ダメージ" + rate + "倍");
    }
    public void AddFromDamageRate(E_Element element, double rate, int effectTurn)
    {
        this.fromDamageRate.Add(new BuffEffect(element, rate, effectTurn));
        Debug.Log(CharaClass.CharaName + "の" + element.ToString() + "からの被ダメージ" + rate + "倍");
    }
    public void AddNoGetDamaged(E_Element element, int effectTurn)
    {
        this.noGetDamaged.Add(new BuffEffect(element, 0, effectTurn));
        Debug.Log(CharaClass.CharaName + "が" + effectTurn + "無敵");
    }
    public void SetElementChanged(E_Element element, int effectTurn)
    {
        this.elementChange = new BuffEffect(element, 0, effectTurn);
        Debug.Log(CharaClass.CharaName + "の属性が" + effectTurn + "ターン" + element.ToString());
    }
    public void AddHaveSkillPoint(int addValue)
    {
        if(this.haveSkillPoint + addValue < 0)
        {
            Debug.Log(CharaClass.CharaName + "のスキルターンが" + this.haveSkillPoint + "遅延された");
            this.haveSkillPoint = 0;
        }else if(addValue < 0)
        {
            Debug.Log(CharaClass.CharaName + "のスキルターンが" + addValue + "遅延された");
            this.haveSkillPoint += addValue;
        }
        else
        {
            Debug.Log(CharaClass.CharaName + "のスキルターンが" + addValue + "短縮された");
            this.haveSkillPoint += addValue;
        }
    }
    public double RecoverHp(double value)
    {
        StatusChange = true;
        if(Hp + value > MaxHp)
        {
            double diff = MaxHp - Hp;
            Debug.Log(charaClass.CharaName + "の体力が満タンになった");
            Hp = MaxHp;
            return diff;
        }
        else
        {
            Debug.Log(charaClass.CharaName + "の体力が" + (int)value + "回復した");
            Hp += value;
            return value;
        }
    }

    public double DecreaseHp(double damage_value)
    {
        StatusChange = true;
        if(Hp - damage_value <= 0)
        {
            double diff = Hp;
            Debug.Log(charaClass.CharaName + "は" + (int)Hp + "のダメージを受けた");
            Hp = 0;
            return diff;
        }
        else
        {
            Debug.Log(charaClass.CharaName + "は" + (int)damage_value + "のダメージを受けた");
            Hp -= damage_value;
            return damage_value;
        }
    }
}
