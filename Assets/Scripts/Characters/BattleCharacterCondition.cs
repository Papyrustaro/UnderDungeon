using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleCharacterのPassiveEffect条件判別のためのパラメータ保持クラス
/// </summary>
public class BattleCharacterCondition
{
    public double MaxHp { get; set; }
    public double Hp { get; set; }
    public double Spd { get; set; }
    public double Atk { get; set; }
    public E_Element Element { get; set; }
    public int Sp { get; set; }

    public BattleCharacterCondition()
    {
        MaxHp = -1; Hp = -1; Spd = -1; Atk = -1; Element = E_Element.FireAquaTree; Sp = -1;
    }
    public BattleCharacterCondition(double maxHp, double hp, double spd, double atk, E_Element element, int skillPoint)
    {
        this.MaxHp = maxHp; this.Hp = hp; this.Spd = spd; this.Atk = atk; this.Element = element; this.Sp = skillPoint;
    }
    public void SetParameter(double maxHp, double hp, double spd, double atk, E_Element element, int skillPoint)
    {
        this.MaxHp = maxHp; this.Hp = hp; this.Spd = spd; this.Atk = atk; this.Element = element; this.Sp = skillPoint;
    }
    public void InitParameter()
    {
        this.MaxHp = -1; this.Hp = -1; this.Spd = -1; this.Atk = 1; this.Element = E_Element.FireAquaTree; this.Sp = -1;
    }
}
