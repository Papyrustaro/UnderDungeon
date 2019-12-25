using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffect
{
    private readonly E_Element element;
    private readonly int rate; //効果倍率
    private readonly int effectTurn; //効果持続ターン

    public BuffEffect(E_Element element, int rate, int effectTurn)
    {
        this.element = element; this.rate = rate; this.effectTurn = effectTurn;
    }
    public BuffEffect(int rate, int effectTurn)
    {
        this.element = E_Element.FireAquaTree; this.rate = rate; this.effectTurn = effectTurn;
    }

    public E_Element Element => this.element;
    public int Rate => this.rate;
    public int EffectTurn => this.effectTurn;
}
