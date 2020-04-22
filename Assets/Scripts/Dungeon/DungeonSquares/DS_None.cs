using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_None : DungeonSquare
{
    public override E_DungeonSquareType SquareType => E_DungeonSquareType.なにもなし;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("イベント発生無し");
        dm.ElapseTurn();
        dm.MoveScene(E_DungeonScene.SelectAction);
    }
}
