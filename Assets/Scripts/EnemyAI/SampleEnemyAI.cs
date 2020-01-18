using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEnemyAI : BaseEnemyAI
{
    public override void EnemyActionFunc(List<BattleCharacter> enemyList, List<BattleCharacter> playerList, BattleCharacter self, BattleActiveSkillsFunc skillFunc)
    {
        Debug.Log(self.CharaClass.CharaName + "の特殊行動");
        self.RecoverHp(100);
        foreach(BattleCharacter player in playerList)
        {
            if (player.IsAlive) player.DecreaseHp(100);
        }
    }
}
