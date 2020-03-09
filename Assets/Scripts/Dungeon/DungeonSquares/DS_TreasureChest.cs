using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_TreasureChest : DungeonSquare
{
    public override E_DungeonSquareType SquareType { get { return E_DungeonSquareType.宝箱; } }

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("宝箱イベント発生");
    }
}
