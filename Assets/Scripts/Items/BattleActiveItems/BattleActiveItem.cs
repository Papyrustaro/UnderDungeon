using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActiveItem : BattleActiveEffect
{
    [SerializeField] private E_BattleActiveItem id;

    [SerializeField] private int sellPrice;

    /// <summary>
    /// 売却額
    /// </summary>
    public int SellPrice => this.sellPrice;

    /// <summary>
    /// 購入額(とりあえず、売却額の10倍で)
    /// </summary>
    public int BuyPrice => this.sellPrice * 10;

    public E_BattleActiveItem ID => this.id;
    public override string EffectName => this.id.ToString();


    public override void OtherFunc(BattleCharacter invoker, List<BattleCharacter> target)
    {
        GetComponent<OtherBattleActiveItemsFunc>().ItemFunc(invoker, target, this);
    }
}
