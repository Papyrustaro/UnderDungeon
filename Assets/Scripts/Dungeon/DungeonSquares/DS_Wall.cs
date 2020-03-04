using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_Wall : DungeonSquare
{
    public override E_DungeonSquareType SquareType { get { return E_DungeonSquareType.壁; } }

    public override void SquareEvent()
    {
        Debug.Log("壁にめり込んでるぞ");
    }
}
