using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_BossBattle : DungeonSquare
{
    [SerializeField] private BattleCharacter bossCharacter;
    public override E_DungeonSquareType SquareType => E_DungeonSquareType.ボス戦;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("ボス戦イベント発生");
        Debug.Log("ボス名: " + bossCharacter.CharaClass.CharaName);
        MoveBattle(dm);
    }

    private void MoveBattle(DungeonManager dm)
    {
        BattleCharacter enemy = Instantiate(this.bossCharacter, dm.EnemysRootObject.transform);
        dm.Enemys = new List<BattleCharacter>() { enemy };
        dm.MoveBattleScene();
    }
}
