using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_Stair : DungeonSquare
{
    public override E_DungeonSquareType SquareType { get { return E_DungeonSquareType.階段; } }

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("階段イベント発生");
    }
}
