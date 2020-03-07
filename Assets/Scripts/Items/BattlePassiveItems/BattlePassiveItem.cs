using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePassiveItem : BattlePassiveEffect
{
    [SerializeField] private E_BattlePassiveItem id;

    [SerializeField] private int sellPrice;

    /// <summary>
    /// 売却額
    /// </summary>
    public int SellPrice => this.sellPrice;

    /// <summary>
    /// 購入額(とりあえず、売却額の10倍で)
    /// </summary>
    public int BuyPrice => this.sellPrice * 10;

    public E_BattlePassiveItem ID => id;
    public override string EffectName => this.id.ToString();
    public override void OtherFunc(BattleCharacter target)
    {
        GetComponent<OtherBattlePassiveItemsFunc>().ItemFunc(target, this);
    }

}
