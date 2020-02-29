using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffect
{
    public E_Element Element { get; }
    public double Rate { get; }
    public int EffectTurn { get; set; }
    public BuffEffect(E_Element element, double rate, int effectTurn)
    {
        this.Element = element; this.Rate = rate; this.EffectTurn = effectTurn;
    }
    public BuffEffect(double rate, int effectTurn)
    {
        this.Element = E_Element.FireAquaTree; this.Rate = rate; this.EffectTurn = effectTurn;
    }
    public BuffEffect(E_Element element, int effectTurn)
    {
        this.Element = element; this.Rate = 0; this.EffectTurn = effectTurn;
    }
    public BuffEffect(int effectTurn)
    {
        this.Element = E_Element.FireAquaTree; this.Rate = 0; this.EffectTurn = effectTurn;
    }
}

public class BuffEffectWithType
{
    public E_BattleActiveEffectType EffectType { get; }
    public BuffEffect Buff { get; }

    public BuffEffectWithType(E_BattleActiveEffectType effectType, BuffEffect buffEffect)
    {
        this.EffectType = effectType; this.Buff = buffEffect;
    }
}