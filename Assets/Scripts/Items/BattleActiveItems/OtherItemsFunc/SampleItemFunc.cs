using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleItemFunc : OtherBattleActiveItemsFunc
{
    public override void ItemFunc(BattleCharacter invoker, List<BattleCharacter> target, BattleActiveItem item)
    {
        Debug.Log("例外的なアイテム処理");
        Debug.Log("例外アイテム名:" + item.EffectName);
    }
}
