using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_Prayer : DungeonSquare
{
    public override E_DungeonSquareType SquareType => E_DungeonSquareType.祈祷;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("祈祷イベント発生");
        Pray(dm);
        dm.ElapseTurn();
        dm.MoveScene(E_DungeonScene.SelectAction);
    }

    private void Pray(DungeonManager dm)
    {
        switch(UnityEngine.Random.Range(0, 3))
        {
            case 0:
                dm.ActivateRandomGoodEffect();
                break;
            case 1:
                dm.ActivateRandomBadEffect();
                break;
            case 2:
                dm.ActivateRandomBadEffect();
                dm.ActivateRandomGoodEffect();
                break;
        }
    }
}
