using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_None : DungeonSquare
{
    public override E_DungeonSquareType SquareType { get { return E_DungeonSquareType.なにもなし; } }

    public override void SquareEvent()
    {
        Debug.Log("イベント発生無し");
    }
}
