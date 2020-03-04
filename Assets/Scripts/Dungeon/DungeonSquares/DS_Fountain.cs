using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_Fountain : DungeonSquare
{
    public override E_DungeonSquareType SquareType { get { return E_DungeonSquareType.回復の泉; } }

    public override void SquareEvent()
    {
        Debug.Log("回復の泉イベント発生");
    }
}
