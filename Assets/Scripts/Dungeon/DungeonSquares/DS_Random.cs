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
        RandomSquareEvent(dm);
    }

    /// <summary>
    /// ランダムでひとつマスイベントを選び、イベントを呼ぶ
    /// </summary>
    /// <param name="dm">dungeonManager</param>
    private void RandomSquareEvent(DungeonManager dm)
    {
        MapManager mm = MapManager.Instance;
        List<DungeonSquare> mayApeearDungeonSquares = new List<DungeonSquare>();
        foreach(DungeonSquare ds in mm.MayApeearDungeonSquares)
        {
            switch (ds.SquareType)
            {
                case E_DungeonSquareType.呪いの宝箱:
                case E_DungeonSquareType.回復の泉:
                case E_DungeonSquareType.宝箱:
                case E_DungeonSquareType.店:
                case E_DungeonSquareType.祈祷:
                case E_DungeonSquareType.罠:
                case E_DungeonSquareType.通常戦闘:
                case E_DungeonSquareType.闇商人:
                case E_DungeonSquareType.なにもなし:
                    mayApeearDungeonSquares.Add(ds);
                    break;
            }
        }

        mayApeearDungeonSquares[UnityEngine.Random.Range(0, mayApeearDungeonSquares.Count)].SquareEvent(dm);
    }
}
