using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_BossBattle : DungeonSquare
{
    [SerializeField] private EnemyCharacter bossCharacter;
    public override E_DungeonSquareType SquareType => E_DungeonSquareType.ボス戦;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("ボス戦イベント発生");
        Debug.Log("ボス名: " + bossCharacter.CharaName);
    }
}
