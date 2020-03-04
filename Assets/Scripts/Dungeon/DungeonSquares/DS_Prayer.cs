using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_Prayer : DungeonSquare
{
    public override E_DungeonSquareType SquareType { get { return E_DungeonSquareType.祈祷; } }

    public override void SquareEvent()
    {
        Debug.Log("祈祷イベント発生");
    }
}
