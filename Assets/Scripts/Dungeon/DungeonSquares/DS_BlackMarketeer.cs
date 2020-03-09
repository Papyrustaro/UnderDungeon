using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_BlackMarketeer : DungeonSquare
{
    //闇商人が販売しうるアイテム(IDでもいいけど)
    [SerializeField] private List<DungeonActiveItem> mayApeearDungeonActiveItems = new List<DungeonActiveItem>();
    [SerializeField] private List<DungeonPassiveItem> mayApeearDungeonPassiveItems = new List<DungeonPassiveItem>();
    [SerializeField] private List<BattleActiveItem> mayApeearBattleActiveItems = new List<BattleActiveItem>();
    [SerializeField] private List<BattlePassiveItem> mayApeearBattlePassiveItems = new List<BattlePassiveItem>();

    public override E_DungeonSquareType SquareType => E_DungeonSquareType.闇商人;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("闇商人イベント発生");
        ChooseSoldItems();
    }

    /// <summary>
    /// 販売される商品をランダムで選択(とりあえず1種類1つずつ)
    /// </summary>
    private void ChooseSoldItems()
    {
        Debug.Log("販売している商品");
        Debug.Log(this.mayApeearDungeonActiveItems[UnityEngine.Random.Range(0, this.mayApeearDungeonActiveItems.Count)].EffectName);
        Debug.Log(this.mayApeearDungeonPassiveItems[UnityEngine.Random.Range(0, this.mayApeearDungeonPassiveItems.Count)].EffectName);
        Debug.Log(this.mayApeearBattleActiveItems[UnityEngine.Random.Range(0, this.mayApeearBattleActiveItems.Count)].EffectName);
        Debug.Log(this.mayApeearBattlePassiveItems[UnityEngine.Random.Range(0, this.mayApeearBattlePassiveItems.Count)].EffectName);
    }
}
