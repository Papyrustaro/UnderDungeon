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
    private Dictionary<E_Element, int> noGetDamagedTurn = new Dictionary<E_Element, int>() { { E_Element.Fire, 0 },{ E_Element.Aqua, 0 },{E_Element.Tree, 0} };
    private BuffEffect elementChange;
    private List<BuffEffect> normalAttackNum = new List<BuffEffect>(); //通常攻撃回数
    private List<BuffEffect> toNormalAttackRate = new List<BuffEffect>(); //通常攻撃の与ダメージ倍率
    private List<BuffEffect> fromNormalAttackRate = new List<BuffEffect>();
    private Dictionary<E_Element, int> attractingEffectTurn = new Dictionary<E_Element, int>() { { E_Element.Fire, 0 }, { E_Element.Aqua, 0 }, { E_Element.Tree, 0 } };
    private List<BuffEffect> hpRegeneration = new List<BuffEffect>();
    private List<BuffEffect> spRegeneration = new List<BuffEffect>();

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
    private Dictionary<E_Element, double> ToDamageRate => GetRate(PassiveToDamageRate, toDamageRate);
    private Dictionary<E_Element, double> FromDamageRate => GetRate(PassiveFromDamageRate, fromDamageRate);
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
    public PlayerCharacter PC => this.pc;
    public EnemyCharacter EC => this.ec;
    //private int[] skillTurnFromActivate = new int[4]; //ActiveSkillのスキル発動までのターン
    public double RebornHpRate { get; set; } = 0; //復活できる状態か(0で復活しない。0.1など復活したときのHP割合を保持)
    public bool Reborned { get; set; } //復活効果を使ったか
    public int NormalAttackToAllTurn { get; set; } = 0; //通常攻撃が全体攻撃になるターン
    public double HaveDamageThisTurn { get; set; } = 100; //1ターンで喰らったダメージ量
    public int NormalAttackNum
    {
        get
        {
            int num = 1;
            foreach(BuffEffect bf in this.normalAttackNum)
            {
                num += (int)bf.Rate;
            }
            return num;
        }
    }
    public bool IsEnemy { get; set; } = false;
    public int HaveSkillPoint => this.haveSkillPoint;
    public E_Element Element { get { if (this.elementChange == null) return CharaClass.Element; else return this.elementChange.Element; } }
    //public int NoGetDamagedTurn => ElementClass.GetDictionaryValue(this.noGetDamagedTurn;
    public double NormalAttackPower => Atk * ToNormalAttackRate * GetToDamageRate(Element);
    public bool IsAlive => Hp > 0;
    public bool IsDefending { get; set; } = false;

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
    //バフなどのActive効果を消す
    private void InitActiveParameter()
    {
        this.hpRate = new List<BuffEffect>(); this.atkRate = new List<BuffEffect>(); this.spdRate = new List<BuffEffect>();
        this.toDamageRate = new List<BuffEffect>(); this.fromDamageRate = new List<BuffEffect>();
        this.noGetDamagedTurn = new Dictionary<E_Element, int>() { { E_Element.Fire, 0 }, { E_Element.Aqua, 0 }, { E_Element.Tree, 0 } };
        this.elementChange = null; this.normalAttackNum = new List<BuffEffect>();
        this.toNormalAttackRate = new List<BuffEffect>(); this.fromNormalAttackRate = new List<BuffEffect>();
        this.attractingEffectTurn = new Dictionary<E_Element, int>() { { E_Element.Fire, 0 }, { E_Element.Aqua, 0 }, { E_Element.Tree, 0 } };
        this.haveSkillPoint = 0; NormalAttackToAllTurn = 0; HaveDamageThisTurn = 0;
        IsDefending = false;
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
        Debug.Log("効果前のMaxHP:" + MaxHp);
        this.hpRate.Add(new BuffEffect(rate, effectTurn));
        Debug.Log(CharaClass.CharaName + "の体力" + rate + "倍");
        Debug.Log("効果後のMaxHP:" + MaxHp);
        StatusChange = true;
    }
    public void AddAtkRate(double rate, int effectTurn)
    {
        Debug.Log("効果前のAtkRate:" + GetRate(this.atkRate));
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
        Debug.Log("Rate:" + GetToDamageRate(element));
        this.toDamageRate.Add(new BuffEffect(element, rate, effectTurn));
        Debug.Log(CharaClass.CharaName + "の" + element.ToString() + "に与ダメージ" + rate + "倍");
        Debug.Log("Rate:" + GetToDamageRate(E_Element.Fire));
    }
    public void AddFromDamageRate(E_Element element, double rate, int effectTurn)
    {
        this.fromDamageRate.Add(new BuffEffect(element, rate, effectTurn));
        Debug.Log(CharaClass.CharaName + "の" + element.ToString() + "からの被ダメージ" + rate + "倍");
    }
    public void AddNoGetDamaged(E_Element element, int effectTurn)
    {
        if (ElementClass.IsFire(element)) this.noGetDamagedTurn[E_Element.Fire] += effectTurn;
        if (ElementClass.IsAqua(element)) this.noGetDamagedTurn[E_Element.Aqua] += effectTurn;
        if (ElementClass.IsTree(element)) this.noGetDamagedTurn[E_Element.Tree] += effectTurn;
        //this.noGetDamagedTurn
        //Debug.Log(CharaClass.CharaName + "が" + effectTurn + "無敵");
    }
    public void AddAttractEffectTurn(E_Element element, int effectTurn)
    {
        if (ElementClass.IsFire(element)) this.attractingEffectTurn[E_Element.Fire] += effectTurn;
        if (ElementClass.IsAqua(element)) this.attractingEffectTurn[E_Element.Aqua] += effectTurn;
        if (ElementClass.IsTree(element)) this.attractingEffectTurn[E_Element.Tree] += effectTurn;
    }
    public void SetElementChanged(E_Element element, int effectTurn)
    {
        Debug.Log(ElementClass.GetStringElement(Element));
        this.elementChange = new BuffEffect(element, 0, effectTurn);
        Debug.Log(CharaClass.CharaName + "の属性が" + effectTurn + "ターン" + element.ToString());
        Debug.Log(ElementClass.GetStringElement(Element));
    }
    public void DamagedByElementAttack(double power, E_Element atkElement) // power = 攻撃側の最終的な威力(atk * skillRate * elementRate)
    {
        if(IsNoDamaged(atkElement))
        {
            Debug.Log(CharaClass.CharaName + "に" + ElementClass.GetStringElement(atkElement) + "属性の攻撃が効かない");
            return;
        }
        else
        {
            if (IsDefending) power /= 2;
            DecreaseHp(power * GetFromDamageRate(atkElement) * ElementClass.GetElementCompatibilityRate(atkElement, this.Element)); // 威力*属性被ダメ減*属性相性
        }
    }
    public void DamagedByNormalAttack(double power, E_Element atkElement) // power = atk * normalAttackRate * elementRate
    {
        if (IsNoDamaged(atkElement))
        {
            Debug.Log(CharaClass.CharaName + "に" + ElementClass.GetStringElement(atkElement) + "属性の攻撃が効かない");
            return;
        }
        else
        {
            if (IsDefending) power /= 2;
            DecreaseHp(power * this.GetFromDamageRate(atkElement) * ElementClass.GetElementCompatibilityRate(atkElement, this.Element) * FromNormalAttackRate); // 威力*属性被ダメ減*属性相性*通常被ダメ
        }
    }
    public void AddHaveSkillPoint(int addValue)
    {
        if(this.haveSkillPoint + addValue < 0)
        {
            Debug.Log(CharaClass.CharaName + "のスキルポイントが" + this.haveSkillPoint + "減少した");
            this.haveSkillPoint = 0;
        }else if(addValue < 0)
        {
            Debug.Log(CharaClass.CharaName + "のスキルポイントが" + addValue + "減少した");
            this.haveSkillPoint += addValue;
        }
        else
        {
            Debug.Log(CharaClass.CharaName + "のスキルポイントが" + addValue + "増加した");
            this.haveSkillPoint += addValue;
        }
    }

    public void AddHpRegeneration(double rateOrValue, int effectTurn)
    {
        this.hpRegeneration.Add(new BuffEffect(rateOrValue, effectTurn));
    }
    public void AddSpRegeneration(int value, int effectTurn)
    {
        this.spRegeneration.Add(new BuffEffect((double)value, effectTurn));
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
    public double RecoverHpByRate(double rate)
    {
        return RecoverHp(MaxHp * rate);
    }

    public double DecreaseHp(double damage_value)
    {
        StatusChange = true;
        if(Hp - damage_value <= 0)
        {
            double diff = Hp;
            Debug.Log(charaClass.CharaName + "は" + (int)Hp + "のダメージを受けた");
            Debug.Log(charaClass.CharaName + "は倒れた");
            Hp = 0;
            InitActiveParameter();
            Reborn();
            return diff;
        }
        else
        {
            Debug.Log(charaClass.CharaName + "は" + (int)damage_value + "のダメージを受けた");
            Hp -= damage_value;
            return damage_value;
        }
    }
    public double DecreaseHpByRate(double rate)
    {
        return DecreaseHp(MaxHp * rate);
    }
    public void Reborn()
    {
        if (!Reborned && RebornHpRate > 0)
        {
            this.Hp = MaxHp * RebornHpRate;
            RebornHpRate = 0;
            Debug.Log(CharaClass.CharaName + "は復活した");
            Reborned = true;
        }
    }
    public bool IsNoDamaged(E_Element damageElement) //damageElementの攻撃を無効にしているかどうか
    {
        return ElementClass.GetTurn(this.noGetDamagedTurn, damageElement) > 0;
    }
    public bool IsAttracting(E_Element attackElement)
    {
        return ElementClass.GetTurn(this.attractingEffectTurn, attackElement) > 0;
    }
    public double GetToDamageRate(E_Element attackElement)
    {
        return ElementClass.GetRate(ToDamageRate, attackElement);
    }
    public double GetFromDamageRate(E_Element damageElement)
    {
        return ElementClass.GetRate(FromDamageRate, damageElement);
    }
    public void ElapseTurn(List<BuffEffect> buffList)
    {
        foreach(BuffEffect bf in buffList)
        {
            bf.EffectTurn--;
        }
        buffList.RemoveAll(bf => bf.EffectTurn < 1);
    }
    public void ElapseTurn(Dictionary<E_Element, int> dic)
    {
        if (dic[E_Element.Fire] > 0) dic[E_Element.Fire]--;
        if (dic[E_Element.Aqua] > 0) dic[E_Element.Aqua]--;
        if (dic[E_Element.Tree] > 0) dic[E_Element.Tree]--;
    }
    public void ElapseTurn(int turnValue)
    {
        if (turnValue > 0) turnValue--;
    }
    public void ElapseTurn(BuffEffect buff)
    {
        if (buff == null) return;
        buff.EffectTurn--;
        if (buff.EffectTurn < 1) buff = null;
    }

    public void ElapseAllTurn()
    {
        ElapseTurn(this.hpRate); ElapseTurn(this.atkRate); ElapseTurn(this.spdRate);
        ElapseTurn(this.toDamageRate); ElapseTurn(this.fromDamageRate); ElapseTurn(this.noGetDamagedTurn);
        ElapseTurn(this.elementChange); ElapseTurn(this.normalAttackNum); ElapseTurn(this.toNormalAttackRate);
        ElapseTurn(this.fromNormalAttackRate); ElapseTurn(this.attractingEffectTurn);
        ElapseTurn(NormalAttackToAllTurn);
    }

    public void SetBeforeAction() //行動前に呼ぶ関数
    {
        foreach(BuffEffect bf in this.hpRegeneration)
        {
            if (bf.Rate > 1) RecoverHp(bf.Rate);
            else RecoverHpByRate(bf.Rate);
        }
        foreach(BuffEffect bf in this.spRegeneration)
        {
            AddHaveSkillPoint((int)bf.Rate);
        }
        ElapseTurn(this.hpRegeneration);
        ElapseTurn(this.spRegeneration);
        IsDefending = false;
    }
    public void SetAfterActiton() //行動後に呼ぶ関数
    {
        ElapseAllTurn();
        AddHaveSkillPoint(1);
    }
}
