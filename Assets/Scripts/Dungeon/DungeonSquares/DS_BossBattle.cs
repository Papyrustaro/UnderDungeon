using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_BossBattle : DungeonSquare
{
    public override E_DungeonSquareType SquareType { get { return E_DungeonSquareType.ボス戦; } }

    public override void SquareEvent()
    {
        Debug.Log("ボス戦イベント発生");
    }
}
