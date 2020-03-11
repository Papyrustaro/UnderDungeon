using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSquaresFunc : MonoBehaviour
{
    [SerializeField] private List<DungeonSquare> dungeonSquaresInThisFloor;

    public void DungeonSquareEvent(DungeonManager dm, E_DungeonSquareType dungeonSquareType)
    {
        if(dungeonSquareType != E_DungeonSquareType.ランダム)
        {
            this.dungeonSquaresInThisFloor.Find(ds => ds.SquareType == dungeonSquareType).SquareEvent(dm);
        }
        else
        {
            this.dungeonSquaresInThisFloor[UnityEngine.Random.Range(0, this.dungeonSquaresInThisFloor.Count)].SquareEvent(dm);
        }
    }
}
