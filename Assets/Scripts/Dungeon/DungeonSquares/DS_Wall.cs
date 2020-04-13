using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_Wall : DungeonSquare
{
    public override E_DungeonSquareType SquareType => E_DungeonSquareType.壁;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("壁にめり込んでるぞ");
        dm.MoveScene(E_DungeonScene.SelectAction);
        throw new System.Exception();
    }
}
