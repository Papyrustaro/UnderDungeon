using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSquaresFunc : MonoBehaviour
{
    private List<DungeonSquare> dungeonSquares;

    public void DungeonSquareEvent(DungeonManager dm, E_DungeonSquareType dungeonSquareType)
    {
        /*if(dungeonSquareType != E_DungeonSquareType.ランダム)
        {*/
            this.dungeonSquares.Find(ds => ds.SquareType == dungeonSquareType).SquareEvent(dm);
        /*}
        else
        {
            this.dungeonSquares[UnityEngine.Random.Range(0, this.dungeonSquares.Count)].SquareEvent(dm);
        }*/
    }

    public void SetMayAppearDungeonSquares(List<DungeonSquare> dungeonSquares)
    {
        this.dungeonSquares = dungeonSquares;
    }
}
