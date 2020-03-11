using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ランダムでどれかのマスのSquareEventを呼ぶ
/// </summary>
public class DS_Random : DungeonSquare
{
    public override E_DungeonSquareType SquareType => E_DungeonSquareType.ランダム;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("ランダムイベント発生");
    }
}
