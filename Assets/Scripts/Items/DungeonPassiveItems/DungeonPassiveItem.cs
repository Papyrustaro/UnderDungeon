using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonPassiveItem : DungeonPassiveEffect
{
    [SerializeField] private E_DungeonPassiveItem id;
    [SerializeField] private int sellPrice;



    /// <summary>
    /// 売却額
    /// </summary>
    public int SellPrice => this.sellPrice;

    /// <summary>
    /// 購入額(とりあえず、売却額の10倍で)
    /// </summary>
    public int BuyPrice => this.sellPrice * 10;

    /// <summary>
    /// アイテム名
    /// </summary>
    public override string EffectName => this.id.ToString();

    /// <summary>
    /// アイテムID
    /// </summary>
    public E_DungeonPassiveItem Id => this.id;
}
