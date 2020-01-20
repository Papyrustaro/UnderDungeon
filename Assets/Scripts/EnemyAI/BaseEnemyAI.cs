using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyAI : MonoBehaviour
{

    public virtual void EnemyActionFunc(List<BattleCharacter> enemyList, List<BattleCharacter> playerList, BattleCharacter self, BattleActiveEffectsFunc skillFunc)
    {

    }
}
