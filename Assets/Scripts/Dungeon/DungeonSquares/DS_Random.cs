using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_Random : DungeonSquare
{
    public override E_DungeonSquareType SquareType { get { return E_DungeonSquareType.ランダム; } }

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("ランダムイベント発生");
    }
}
