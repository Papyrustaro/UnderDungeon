using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_Fountain : DungeonSquare
{
    public override E_DungeonSquareType SquareType => E_DungeonSquareType.回復の泉;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("回復の泉イベント発生");
        HealHp(dm);
        dm.ElapseTurn();
        dm.MoveScene(E_DungeonScene.SelectAction);
    }

    private void HealHp(DungeonManager dm)
    {
        foreach(BattleCharacter bc in dm.Allys)
        {
            bc.RecoverHpByRate(1);
        }
    }
}
