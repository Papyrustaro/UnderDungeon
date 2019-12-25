using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 戦闘中のEnemyクラス */
public class BattleEnemyCharacter : BattleCharacter
{
    private EnemyCharacter ec;
    private void Awake()
    {
        ec = GetComponent<EnemyCharacter>();
    }
    public EnemyCharacter EC => this.ec;
}
