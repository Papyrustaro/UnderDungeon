using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_CursedTreasureChest : DungeonSquare
{
    public override E_DungeonSquareType SquareType { get { return E_DungeonSquareType.呪いの宝箱; } }

    public override void SquareEvent()
    {
        Debug.Log("呪いの宝箱イベント発生");
    }
}
