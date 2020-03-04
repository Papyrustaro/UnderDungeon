using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_NormalBattle : DungeonSquare
{
    public override E_DungeonSquareType SquareType { get { return E_DungeonSquareType.通常戦闘; } }

    public override void SquareEvent()
    {
        Debug.Log("通常戦闘イベント発生");
    }
}
