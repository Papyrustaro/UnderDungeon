using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffect
{
    private readonly E_Element element;
    private readonly double rate; //効果倍率
    private int effectTurn; //効果持続ターン

    public BuffEffect(E_Element element, double rate, int effectTurn)
    {
        this.element = element; this.rate = rate; this.effectTurn = effectTurn;
    }
    public BuffEffect(double rate, int effectTurn)
    {
        this.element = E_Element.FireAquaTree; this.rate = rate; this.effectTurn = effectTurn;
    }
    public BuffEffect(E_Element element, int effectTurn)
    {
        this.element = element; this.rate = 0; this.effectTurn = effectTurn;
    }

    public E_Element Element => this.element;
    public double Rate => this.rate;
    public int EffectTurn { get { return this.effectTurn; } set { this.effectTurn = value; } }
}