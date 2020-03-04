using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_Shop : DungeonSquare
{
    public override E_DungeonSquareType SquareType { get { return E_DungeonSquareType.店; } }

    public override void SquareEvent()
    {
        Debug.Log("店イベント発生");
    }
}
