using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_Prayer : DungeonSquare
{
    public override E_DungeonSquareType SquareType => E_DungeonSquareType.祈祷;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("祈祷イベント発生");
    }

    private void Pray()
    {

    }

    /// <summary>
    /// ランダムでひとつ悪い効果適用
    /// </summary>
    private void ActivateRandomBadEffect()
    {

    }

    /// <summary>
    /// ランダムでひとつ良い効果発動
    /// </summary>
    private void ActivateRandomGoodEffect()
    {

    }
}
