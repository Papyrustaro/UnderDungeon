using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_BlackMarketeer : DungeonSquare
{
    public override E_DungeonSquareType SquareType { get { return E_DungeonSquareType.闇商人; } }

    public override void SquareEvent()
    {
        Debug.Log("闇商人イベント発生");
    }
}
