﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DungeonActiveItem : DungeonActiveEffect
{
    [SerializeField] private E_DungeonActiveItem id;
    [SerializeField] private int sellPrice;

    public E_DungeonActiveItem Id => this.id;

    public override string EffectName => this.id.ToString();

    /// <summary>
    /// 売却額
    /// </summary>
    public int SellPrice => this.sellPrice;

    /// <summary>
    /// 購入額(とりあえず、売却額の10倍で)
    /// </summary>
    public int BuyPrice => this.sellPrice * 10;
}
