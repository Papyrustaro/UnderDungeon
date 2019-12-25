using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    private List<PlayerCharacter> playerList;
    [SerializeField]
    private List<EnemyCharacter> enemyList;

    public List<PlayerCharacter> PlayerList => this.playerList;
    public List<EnemyCharacter> EnemyList => this.enemyList;
}
