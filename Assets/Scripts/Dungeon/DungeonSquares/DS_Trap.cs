using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_Trap : DungeonSquare
{
    public override E_DungeonSquareType SquareType => E_DungeonSquareType.罠;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("罠イベント発生");
        dm.GetTrapped();
        dm.MoveScene(E_DungeonScene.SelectAction);
    }
}
