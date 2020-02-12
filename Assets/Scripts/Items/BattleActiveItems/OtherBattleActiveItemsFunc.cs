using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OtherBattleActiveItemsFunc : MonoBehaviour
{
    public virtual void ItemFunc(BattleCharacter invoker, List<BattleCharacter> target, BattleActiveItem item)
    {
    }
}
