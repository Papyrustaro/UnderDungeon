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

    private List<E_BattleActiveSkill> battleActiveSkillID = new List<E_BattleActiveSkill>();
    private Character charaClass;

    /// <summary>
    /// 経過ターン処理後に適用するActiveEffectの保存用リスト
    /// </summary>
    private List<BuffEffectWithType> beforeSetBuffEffect = new List<BuffEffectWithType>();


    /* passiveのみ考慮したプロパティ */
    public double PassiveMaxHpRate { get; set; } = 1; //passive考慮したステータス charaClass.maxHP * battlePassiveSkill * itemPassiveSkill
    public double PassiveAtkRate { get; set; } = 1;
    public double PassiveSpdRate { get; set; } = 1;
    public Dictionary<E_Element, double> PassiveToDamageRate { get; set; }
    public Dictionary<E_Element, double> PassiveFromDamageRate { get; set; }
    public double PassiveToNormalAttackRate { get; set; } = 1;
    public double PassiveFromNormalAttackRate { get; set; } = 1;

    /// <summary>
    /// 防御状態での被ダメージ倍率(属性関係なし)
    /// </summary>
    public double PassiveFromDamageRateInDefending { get; set; } = 1; 
    public Dictionary<E_Element, bool> PassiveAttractInDefending { get; set; }

    /// <summary>
    /// 1ターンでの自然Sp回復量
    /// </summary>
    public int PassiveHealSpInTurn { get; set; } = 1;
    public int PassiveNormalAttackNum { get; set; } = 1;


    /*  passiveとactiveを反映したプロパティ */
    /// <summary>
    /// 全ての効果を考慮した最大Hp
    /// </summary>
    public double MaxHp => CharaClass.MaxHp * PassiveMaxHpRate * GetRate(hpRate); 

    /// <summary>
    /// 全ての効果を考慮した最大Atk
    /// </summary>
    public double Atk => CharaClass.MaxAtk * PassiveAtkRate * GetRate(atkRate);

    /// <summary>
    /// 全ての効果を考慮した最大Spd
    /// </summary>
    public double Spd => CharaClass.MaxSpd * PassiveSpdRate * GetRate(spdRate);

    /// <summary>
    /// 与ダメージ倍率(Fire,Aqua,Treeのみkeyに持つため、基本はGetToDamageRate(element)を利用
    /// </summary>
    private Dictionary<E_Element, double> ToDamageRate => GetRate(PassiveToDamageRate, toDamageRate);

    /// <summary>
    /// 被ダメージ倍率(Fire,Aqua,Treeのみkeyに持つため、基本はGetToDamageRate(element)を利用
    /// </summary>
    private Dictionary<E_Element, double> FromDamageRate => GetRate(PassiveFromDamageRate, fromDamageRate);

    public double ToNormalAttackRate => PassiveToNormalAttackRate * GetRate(this.toNormalAttackRate);
    public double FromNormalAttackRate => PassiveFromNormalAttackRate * GetRate(this.fromNormalAttackRate);




    /* その他プロパティ */
    /// <summary>
    /// PassiveEffectなどの条件判定用の一時状態保存クラス
    /// </summary>
    public BattleCharacterCondition Condition { get; set; } = new BattleCharacterCondition();

    public List<E_BattleActiveSkill> BattleActiveSkillID => this.battleActiveSkillID;
    public bool StatusChange { get; set; } = false;

    /// <summary>
    /// 現在のHp
    /// </summary>
    public double Hp { get; set; }

    /// <summary>
    /// ベースとなるCharacterクラス
    /// </summary>
    public Character CharaClass
    {
        get
        {
            if (charaClass == null) SetCharacter();
            return charaClass;
        }
    }
    public PlayerCharacter PC { get; set; }
    public EnemyCharacter EC { get; set; }

    /// <summary>
    /// 復活する際のHp回復割合(Max1, 0で復活しない)
    /// </summary>
    public double RebornHpRate { get; set; } = 0; 
    public bool Reborned { get; set; } 
    public int NormalAttackToAllTurn { get; set; } = 0; 
    public double HaveDamageThisTurn { get; set; } = 0; 
    public int NormalAttackNum
    {
        get
        {
            int num = PassiveNormalAttackNum;
            foreach(BuffEffect bf in this.normalAttackNum)
            {
                num += (int)bf.Rate;
            }
            return num;
        }
    }
    public bool IsEnemy { get; set; } = false;
    public int HaveSkillPoint { get; set; } = 0;
    public E_Element Element { get { if (this.elementChange == null) return CharaClass.Element; else return this.elementChange.Element; } }
    public double NormalAttackPower => Atk * ToNormalAttackRate * GetToDamageRate(Element);
    public bool IsAlive => Hp > 0;
    public bool IsDefending { get; set; } = false;

    /// <summary>
    /// 複数回呼ばれる可能性あり
    /// </summary>
    public void Start()
    {
        SetCharacter();
        SetBaseStatus();
        SetActiveSkill();
        StatusChange = true;
    }

    /// <summary>
    /// PassiveEffectの初期化、HpをCharaClass.MaxHpに
    /// </summary>
    private void SetBaseStatus()
    {
        InitPassiveParameter();
        Hp = MaxHp; //この行いらないかもしれん(素のMaxHp)
    }

    /// <summary>
    /// ステータス条件用にConditionに各パラメータを記憶
    /// </summary>
    public void RememberCondition()
    {
        Condition.SetParameter(this.MaxHp, this.Hp, this.Spd, this.Atk, this.Element, this.HaveSkillPoint);
    }

    /// <summary>
    /// ステータス条件用Conditionの初期化
    /// </summary>
    public void InitCondition()
    {
        Condition.InitParameter();
    }

    /// <summary>
    /// PassiveEffect用パラメータの初期化
    /// </summary>
    public void InitPassiveParameter()
    {
        PassiveMaxHpRate = 1;
        PassiveAtkRate = 1;
        PassiveSpdRate = 1;
        PassiveToDamageRate = new Dictionary<E_Element, double>() { { E_Element.Fire, 1.0 }, { E_Element.Aqua, 1.0 }, { E_Element.Tree, 1.0 } };
        PassiveFromDamageRate = new Dictionary<E_Element, double>() { { E_Element.Fire, 1.0 }, { E_Element.Aqua, 1.0 }, { E_Element.Tree, 1.0 } };
        PassiveToNormalAttackRate = 1; PassiveFromNormalAttackRate = 1;
        PassiveFromDamageRateInDefending = 1;
        PassiveAttractInDefending = new Dictionary<E_Element, bool>() { { E_Element.Fire, false }, { E_Element.Aqua, false }, { E_Element.Tree, false } };
        PassiveHealSpInTurn = 1;
        PassiveNormalAttackNum = 1;
    }

    /// <summary>
    /// CharaClass,PlayerCharacter(EnemyCharacter)をアタッチ
    /// </summary>
    private void SetCharacter()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            this.PC = GetComponent<PlayerCharacter>();
            this.charaClass = PC.CharaClass;
        }
        else
        {
            this.EC = GetComponent<EnemyCharacter>();
            this.charaClass = EC.CharaClass;
            this.IsEnemy = true;
        }
    }

    /// <summary>
    /// 使用可能なBattleActiveSkillのセット
    /// </summary>
    private void SetActiveSkill()
    {
        this.battleActiveSkillID = new List<E_BattleActiveSkill>();
        if (!this.IsEnemy)
        {
            for(int i = 0; i < CharacterConstValue.MAX_HAVE_ACTIVE_SKILL; i++)
            {
                if (PC.LV >= PC.UseAbleBattleActiveSkillLV[i]) this.battleActiveSkillID.Add(PC.HaveBattleActiveSkillID[i]);
                else break;
            }
        }
    }
    
    /// <summary>
    /// BattleActiveEffectの初期化
    /// </summary>
    private void InitActiveParameter()
    {
        this.hpRate = new List<BuffEffect>(); this.atkRate = new List<BuffEffect>(); this.spdRate = new List<BuffEffect>();
        this.toDamageRate = new List<BuffEffect>(); this.fromDamageRate = new List<BuffEffect>();
        this.noGetDamagedTurn = new Dictionary<E_Element, int>() { { E_Element.Fire, 0 }, { E_Element.Aqua, 0 }, { E_Element.Tree, 0 } };
        this.elementChange = null; this.normalAttackNum = new List<BuffEffect>();
        this.toNormalAttackRate = new List<BuffEffect>(); this.fromNormalAttackRate = new List<BuffEffect>();
        this.attractingEffectTurn = new Dictionary<E_Element, int>() { { E_Element.Fire, 0 }, { E_Element.Aqua, 0 }, { E_Element.Tree, 0 } };
        this.HaveSkillPoint = 0; NormalAttackToAllTurn = 0; HaveDamageThisTurn = 0;
        IsDefending = false;
        this.beforeSetBuffEffect = new List<BuffEffectWithType>();
    }
    
    /// <summary>
    /// BuffEffectのRateを計算して、最終的な倍率を返す
    /// </summary>
    /// <param name="bfList">ActiveEffect</param>
    /// <returns>最終的な倍率</returns>
    public double GetRate(List<BuffEffect> bfList) 
    {
        double rate = 1.0;
        foreach(BuffEffect bf in bfList)
        {
            rate *= bf.Rate;
        }
        return rate;
    }

    /// <summary>
    /// 各属性ダメージのPassiveとActiveを考慮した最終的な倍率を返す
    /// </summary>
    /// <param name="passive">PassiveEffect</param>
    /// <param name="activeList">ActiveEffect</param>
    /// <returns>3属性の最終的な倍率</returns>
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

    /// <summary>
    /// ActiveなHpバフ付与
    /// </summary>
    /// <param name="rate">倍率</param>
    /// <param name="effectTurn">効果ターン</param>
    public void AddHpRate(double rate, int effectTurn)
    {
        this.beforeSetBuffEffect.Add(new BuffEffectWithType(E_BattleActiveEffectType.HPバフ, new BuffEffect(rate, effectTurn)));
        StatusChange = true;
    }

    /// <summary>
    /// ActiveなAtkバフ付与
    /// </summary>
    /// <param name="rate">倍率</param>
    /// <param name="effectTurn">効果ターン</param>
    public void AddAtkRate(double rate, int effectTurn)
    {
        this.beforeSetBuffEffect.Add(new BuffEffectWithType(E_BattleActiveEffectType.ATKバフ, new BuffEffect(rate, effectTurn)));
        StatusChange = true;
    }

    /// <summary>
    /// ActiveなSpdバフ付与
    /// </summary>
    /// <param name="rate">倍率</param>
    /// <param name="effectTurn">効果ターン</param>
    public void AddSpdRate(double rate, int effectTurn)
    {
        this.beforeSetBuffEffect.Add(new BuffEffectWithType(E_BattleActiveEffectType.SPDバフ, new BuffEffect(rate, effectTurn)));
        StatusChange = true;
    }

    /// <summary>
    /// Activeな通常攻撃与ダメージバフ付与
    /// </summary>
    /// <param name="rate">倍率</param>
    /// <param name="effectTurn">効果ターン</param>
    public void AddToNormalAttackRate(double rate, int effectTurn)
    {
        this.beforeSetBuffEffect.Add(new BuffEffectWithType(E_BattleActiveEffectType.通常攻撃与ダメージ増減, new BuffEffect(rate, effectTurn)));
    }

    /// <summary>
    /// Activeな通常攻撃被ダメージバフ付与
    /// </summary>
    /// <param name="rate">倍率</param>
    /// <param name="effectTurn">効果ターン</param>
    public void AddFromNormalAttackRate(double rate, int effectTurn)
    {
        this.beforeSetBuffEffect.Add(new BuffEffectWithType(E_BattleActiveEffectType.通常攻撃被ダメージ増減, new BuffEffect(rate, effectTurn)));
    }

    /// <summary>
    /// Activeな通常攻撃回数増加バフ付与
    /// </summary>
    /// <param name="addNum">増加回数</param>
    /// <param name="effectTurn">効果ターン</param>
    public void AddNormalAttackNum(int addNum, int effectTurn)
    {
        this.beforeSetBuffEffect.Add(new BuffEffectWithType(E_BattleActiveEffectType.通常攻撃回数追加, new BuffEffect((double)addNum, effectTurn)));
    }

    /// <summary>
    /// Activeな与ダメージバフ付与
    /// </summary>
    /// <param name="element">どの属性に対する与ダメージバフか</param>
    /// <param name="rate">倍率</param>
    /// <param name="effectTurn">効果ターン</param>
    public void AddToDamageRate(E_Element element, double rate, int effectTurn)
    {
        this.beforeSetBuffEffect.Add(new BuffEffectWithType(E_BattleActiveEffectType.与ダメージ増減バフ, new BuffEffect(element, rate, effectTurn)));
    }

    /// <summary>
    /// Activeな被ダメージバフ付与
    /// </summary>
    /// <param name="element">どの属性に対する被ダメージバフか</param>
    /// <param name="rate">倍率</param>
    /// <param name="effectTurn">効果ターン</param>
    public void AddFromDamageRate(E_Element element, double rate, int effectTurn)
    {
        this.beforeSetBuffEffect.Add(new BuffEffectWithType(E_BattleActiveEffectType.被ダメージ増減バフ, new BuffEffect(element, rate, effectTurn)));
    }

    /// <summary>
    /// Activeな攻撃無効バフ付与
    /// </summary>
    /// <param name="element">どの属性からの攻撃を無効とするか</param>
    /// <param name="effectTurn">効果ターン</param>
    public void AddNoGetDamaged(E_Element element, int effectTurn)
    {
        this.beforeSetBuffEffect.Add(new BuffEffectWithType(E_BattleActiveEffectType.無敵付与, new BuffEffect(element, effectTurn)));
    }

    /// <summary>
    /// Activeな攻撃集中効果付与
    /// </summary>
    /// <param name="element">集中する属性</param>
    /// <param name="effectTurn">効果ターン</param>
    public void AddAttractEffectTurn(E_Element element, int effectTurn)
    {
        this.beforeSetBuffEffect.Add(new BuffEffectWithType(E_BattleActiveEffectType.攻撃集中, new BuffEffect(element, effectTurn)));
    }

    /// <summary>
    /// Activeな属性変化(すでにある場合は前回の効果は消える)
    /// </summary>
    /// <param name="element">変化後の属性</param>
    /// <param name="effectTurn">効果ターン</param>
    public void SetElementChanged(E_Element element, int effectTurn)
    {
        this.beforeSetBuffEffect.Add(new BuffEffectWithType(E_BattleActiveEffectType.属性変化, new BuffEffect(element, effectTurn)));
    }

    /// <summary>
    /// atkElement属性からpowerのダメージ(与ダメージ計算済)
    /// </summary>
    /// <param name="power">与ダメージ計算後の最終的な威力(atk * skillRate * toDamageRate[atkElement])</param>
    /// <param name="atkElement">攻撃属性</param>
    public void DamagedByElementAttack(double power, E_Element atkElement)
    {
        if(IsNoDamaged(atkElement))
        {
            Debug.Log(CharaClass.CharaName + "に" + ElementClass.GetStringElement(atkElement) + "属性の攻撃が効かない");
            return;
        }
        else
        {
            if (IsDefending) power *= 0.5 * PassiveFromDamageRateInDefending;
            DecreaseHp(power * GetFromDamageRate(atkElement) * ElementClass.GetElementCompatibilityRate(atkElement, this.Element)); // 威力*属性被ダメ減*属性相性
        }
    }

    /// <summary>
    /// atkElement属性からpowerのダメージ(与ダメージ計算済)
    /// </summary>
    /// <param name="power">与ダメージの最終的な威力(atk * ToNormalAttackDamageRate)</param>
    /// <param name="atkElement">通常攻撃の属性(攻撃者の属性)</param>
    public void DamagedByNormalAttack(double power, E_Element atkElement) // power = atk * normalAttackRate * elementRate
    {
        if (IsNoDamaged(atkElement))
        {
            Debug.Log(CharaClass.CharaName + "に" + ElementClass.GetStringElement(atkElement) + "属性の攻撃が効かない");
            return;
        }
        else
        {
            if (IsDefending) power *= 0.5 * PassiveFromDamageRateInDefending;
            DecreaseHp(power * this.GetFromDamageRate(atkElement) * ElementClass.GetElementCompatibilityRate(atkElement, this.Element) * FromNormalAttackRate); // 威力*属性被ダメ減*属性相性*通常被ダメ
        }
    }

    /// <summary>
    /// 所持Spの増減(addValueが負で減少)
    /// </summary>
    /// <param name="addValue">Sp増加量</param>
    public void AddHaveSkillPoint(int addValue)
    {
        if(this.HaveSkillPoint + addValue < 0)
        {
            Debug.Log(CharaClass.CharaName + "のスキルポイントが" + this.HaveSkillPoint + "減少した");
            this.HaveSkillPoint = 0;
        }else if(addValue < 0)
        {
            Debug.Log(CharaClass.CharaName + "のスキルポイントが" + addValue + "減少した");
            this.HaveSkillPoint += addValue;
        }
        else
        {
            Debug.Log(CharaClass.CharaName + "のスキルポイントが" + addValue + "増加した");
            this.HaveSkillPoint += addValue;
        }
    }

    /// <summary>
    /// AcitiveHpリジェネ効果付与
    /// </summary>
    /// <param name="rateOrValue">回復量(1以下で最大Hpの割合回復、1以上で固定値回復)</param>
    /// <param name="effectTurn">効果ターン</param>
    public void AddHpRegeneration(double rateOrValue, int effectTurn)
    {
        this.beforeSetBuffEffect.Add(new BuffEffectWithType(E_BattleActiveEffectType.HPリジェネ, new BuffEffect(rateOrValue, effectTurn)));
    }

    /// <summary>
    /// ActiveSpリジェネ効果付与
    /// </summary>
    /// <param name="value">リジェネ量</param>
    /// <param name="effectTurn">効果ターン</param>
    public void AddSpRegeneration(int value, int effectTurn)
    {
        this.beforeSetBuffEffect.Add(new BuffEffectWithType(E_BattleActiveEffectType.SPリジェネ, new BuffEffect((double)value, effectTurn)));
    }

    /// <summary>
    /// 通常攻撃全体化のターン数を増加させる(初期値0)
    /// </summary>
    /// <param name="addTurn">増加ターン数</param>
    public void AddNormalAttackToAllTurn(int addTurn) {
        this.beforeSetBuffEffect.Add(new BuffEffectWithType(E_BattleActiveEffectType.通常攻撃全体攻撃化, new BuffEffect(addTurn)));
    }

    /// <summary>
    /// Hp回復
    /// </summary>
    /// <param name="value">回復量(固定値)</param>
    /// <returns>実際に回復した量</returns>
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

    /// <summary>
    /// Hpを最大Hpの割合回復
    /// </summary>
    /// <param name="rate">最大Hpに対する割合</param>
    /// <returns>実際に回復した量</returns>
    public double RecoverHpByRate(double rate)
    {
        return RecoverHp(MaxHp * rate);
    }

    /// <summary>
    /// 固定ダメージ
    /// </summary>
    /// <param name="damage_value">固定ダメージ量</param>
    /// <returns>実際に受けたダメージ量</returns>
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
            HaveDamageThisTurn += damage_value;
            return damage_value;
        }
    }

    /// <summary>
    /// 最大Hpに対する割合ダメージ
    /// </summary>
    /// <param name="rate">割合</param>
    /// <returns>実際に受けたダメージ量</returns>
    public double DecreaseHpByRate(double rate)
    {
        return DecreaseHp(MaxHp * rate);
    }

    /// <summary>
    /// 復活処理
    /// </summary>
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

    /// <summary>
    /// damageElementの属性を無効にしているかどうか
    /// </summary>
    /// <param name="damageElement">検索する属性</param>
    /// <returns>無効にしているかどうか</returns>
    public bool IsNoDamaged(E_Element damageElement)
    {
        return ElementClass.GetTurn(this.noGetDamagedTurn, damageElement) > 0;
    }

    /// <summary>
    /// attackElementの属性を自身に集中しているかどうか
    /// </summary>
    /// <param name="attackElement">検索する属性</param>
    /// <returns>自身に集中しているか</returns>
    public bool IsAttracting(E_Element attackElement)
    {
        return (IsDefending && PassiveAttractInDefending[attackElement]) || (ElementClass.GetTurn(this.attractingEffectTurn, attackElement) > 0);
    }

    /// <summary>
    /// 全て考慮したToDamageRateの値を返す(複属性対応)
    /// </summary>
    /// <param name="attackElement">攻撃属性</param>
    /// <returns>倍率</returns>
    public double GetToDamageRate(E_Element attackElement)
    {
        return ElementClass.GetRate(ToDamageRate, attackElement);
    }
    /// <summary>
    /// 全て考慮したFromDamageRateの値を返す(複属性対応)
    /// </summary>
    /// <param name="damageElement">被ダメ属性</param>
    /// <returns>倍率</returns>
    public double GetFromDamageRate(E_Element damageElement)
    {
        return ElementClass.GetRate(FromDamageRate, damageElement);
    }

    /// <summary>
    /// BuffListのeffectTurnを1ターン経過させる
    /// </summary>
    /// <param name="buffList">ターン経過するバフ</param>
    public void ElapseTurn(List<BuffEffect> buffList)
    {
        foreach(BuffEffect bf in buffList)
        {
            bf.EffectTurn--;
        }
        buffList.RemoveAll(bf => bf.EffectTurn < 1);
    }

    /// <summary>
    /// Dictionary(element, effectTurn)を1ターン経過させる
    /// </summary>
    /// <param name="dic">dictionary(element, effectTurn)</param>
    public void ElapseTurn(Dictionary<E_Element, int> dic)
    {
        if (dic[E_Element.Fire] > 0) dic[E_Element.Fire]--;
        if (dic[E_Element.Aqua] > 0) dic[E_Element.Aqua]--;
        if (dic[E_Element.Tree] > 0) dic[E_Element.Tree]--;
    }

    /// <summary>
    /// 経過後のターン値を返す(値渡しであるので注意)
    /// </summary>
    /// <param name="turnValue">経過させるパラメータ</param>
    /// <returns>経過後のターン</returns>
    public int ElapseTurn(int turnValue)
    {
        if (turnValue > 0) turnValue--;
        return turnValue;
    }

    /// <summary>
    /// BuffEffectのeffectTurnを1ターン経過させる
    /// </summary>
    /// <param name="buff">1ターン経過させるBuffEffect</param>
    public void ElapseTurn(BuffEffect buff)
    {
        if (buff == null) return;
        buff.EffectTurn--;
        if (buff.EffectTurn < 1) buff = null;
    }

    /// <summary>
    /// 全てのActiveEffectを1ターン経過処理(リジェネは除く)
    /// </summary>
    public void ElapseAllTurn()
    {
        ElapseTurn(this.hpRate); ElapseTurn(this.atkRate); ElapseTurn(this.spdRate);
        ElapseTurn(this.toDamageRate); ElapseTurn(this.fromDamageRate); ElapseTurn(this.noGetDamagedTurn);
        ElapseTurn(this.elementChange); ElapseTurn(this.normalAttackNum); ElapseTurn(this.toNormalAttackRate);
        ElapseTurn(this.fromNormalAttackRate); ElapseTurn(this.attractingEffectTurn);
        NormalAttackToAllTurn = ElapseTurn(this.NormalAttackToAllTurn);
    }

    /// <summary>
    /// キャラ行動開始前に呼ぶ関数。呼ばれるのは行動前の生存キャラのみ。リジェネ適用、防御状態解除
    /// </summary>
    public void SetBeforeSelfAction()
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

    /// <summary>
    /// 行動後に呼ぶ関数。呼ばれるのは行動したキャラのみ。AcitveEffectの効果を全て1ターン経過、Sp自然回復
    /// </summary>
    public void SetAfterSelfAction()
    {
        ElapseAllTurn();
        AddHaveSkillPoint(PassiveHealSpInTurn);
    }

    /// <summary>
    /// キャラ行動前に呼ぶ関数。全キャラ呼ばれる。呼ばれるのは行動者処理の後(?)
    /// </summary>
    public void SetBeforeAction()
    {

    }

    /// <summary>
    /// キャラ行動後に呼ぶ関数。全キャラ呼ばれる。呼ばれるのは行動者処理の後
    /// </summary>
    public void SetAfterAction()
    {
        SetBuffEffectAfterElapseTurn();
    }
    

    /// <summary>
    /// ターン経過処理後に呼ぶ、ActiveEffect適用関数
    /// </summary>
    private void SetBuffEffectAfterElapseTurn()
    {
        foreach(BuffEffectWithType buff in this.beforeSetBuffEffect)
        {
            switch (buff.EffectType)
            {
                case E_BattleActiveEffectType.ATKバフ:
                    this.atkRate.Add(buff.Buff);
                    Debug.Log(CharaClass.CharaName + "のAtkが" + buff.Buff.EffectTurn + "ターン" + buff.Buff.Rate + "倍");
                    break;
                case E_BattleActiveEffectType.HPバフ:
                    this.hpRate.Add(buff.Buff);
                    Debug.Log(CharaClass.CharaName + "のHpが" + buff.Buff.EffectTurn + "ターン" + buff.Buff.Rate + "倍");
                    break;
                case E_BattleActiveEffectType.HPリジェネ:
                    this.hpRegeneration.Add(buff.Buff);
                    Debug.Log(CharaClass.CharaName + "が" + buff.Buff.EffectTurn + "ターンHpを" + buff.Buff.Rate + "回復");
                    break;
                case E_BattleActiveEffectType.SPDバフ:
                    this.spdRate.Add(buff.Buff);
                    Debug.Log(CharaClass.CharaName + "のSpdが" + buff.Buff.EffectTurn + "ターン" + buff.Buff.Rate + "倍");
                    break;
                case E_BattleActiveEffectType.SPリジェネ:
                    this.spRegeneration.Add(buff.Buff);
                    Debug.Log(CharaClass.CharaName + "が" + buff.Buff.EffectTurn + "ターンSpを" + buff.Buff.Rate + "回復");
                    break;
                case E_BattleActiveEffectType.与ダメージ増減バフ:
                    this.toDamageRate.Add(buff.Buff);
                    Debug.Log(CharaClass.CharaName + "の" + ElementClass.GetStringElement(buff.Buff.Element) + "への与ダメージが" + buff.Buff.EffectTurn + "ターン" + buff.Buff.Rate + "倍");
                    break;
                case E_BattleActiveEffectType.属性変化:
                    this.elementChange = buff.Buff;
                    Debug.Log(CharaClass.CharaName + "の属性が" + buff.Buff.EffectTurn + "ターン" + ElementClass.GetStringElement(buff.Buff.Element) + "属性に変化");
                    break;
                case E_BattleActiveEffectType.攻撃集中:
                    ElementClass.AddTurn(this.attractingEffectTurn, buff.Buff.Element, buff.Buff.EffectTurn);
                    Debug.Log(CharaClass.CharaName + "が" + buff.Buff.EffectTurn + "ターン" + ElementClass.GetStringElement(buff.Buff.Element) + "属性の攻撃集中");
                    break;
                case E_BattleActiveEffectType.無敵付与:
                    ElementClass.AddTurn(this.noGetDamagedTurn, buff.Buff.Element, buff.Buff.EffectTurn);
                    Debug.Log(CharaClass.CharaName + "が" + buff.Buff.EffectTurn + "ターン" + ElementClass.GetStringElement(buff.Buff.Element) + "属性の攻撃無敵");
                    break;
                case E_BattleActiveEffectType.被ダメージ増減バフ:
                    this.fromDamageRate.Add(buff.Buff);
                    Debug.Log(CharaClass.CharaName + "の" + ElementClass.GetStringElement(buff.Buff.Element) + "からの被ダメージが" + buff.Buff.EffectTurn + "ターン" + buff.Buff.Rate + "倍");
                    break;
                case E_BattleActiveEffectType.通常攻撃与ダメージ増減:
                    this.toNormalAttackRate.Add(buff.Buff);
                    Debug.Log(CharaClass.CharaName + "通常攻撃の与ダメージが" + buff.Buff.EffectTurn + "ターン" + buff.Buff.Rate + "倍");
                    break;
                case E_BattleActiveEffectType.通常攻撃全体攻撃化:
                    this.NormalAttackToAllTurn += buff.Buff.EffectTurn;
                    Debug.Log(CharaClass.CharaName + "の通常攻撃が" + buff.Buff.EffectTurn + "ターン全体化");
                    break;
                case E_BattleActiveEffectType.通常攻撃回数追加:
                    this.normalAttackNum.Add(buff.Buff);
                    Debug.Log(CharaClass.CharaName + "通常攻撃回数が" + buff.Buff.EffectTurn + "ターン" + buff.Buff.Rate + "回増加");
                    break;
                case E_BattleActiveEffectType.通常攻撃被ダメージ増減:
                    this.fromNormalAttackRate.Add(buff.Buff);
                    Debug.Log(CharaClass.CharaName + "の通常攻撃の被ダメージが" + buff.Buff.EffectTurn + "ターン" + buff.Buff.Rate + "倍");
                    break;
                case E_BattleActiveEffectType _:
                    Debug.Log("error");
                    break;
            }
        }

        this.beforeSetBuffEffect = new List<BuffEffectWithType>(); //効果適用後、記憶用変数の初期化
    }
}
