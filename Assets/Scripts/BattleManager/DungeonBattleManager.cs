using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBattleManager : MonoBehaviour
{
    private List<BattlePlayerCharacter> playerList = new List<BattlePlayerCharacter>();
    private List<BattleEnemyCharacter> enemyList = new List<BattleEnemyCharacter>();

    private void Awake()
    {
        SetCharacter();
        Debug.Log(playerList[1].PC.CharaName);
        Debug.Log(enemyList[2].EC.Description);
    }
    public void SetCharacter()
    {
        DungeonManager dm = GetComponent<DungeonManager>();
        List<PlayerCharacter> playerList = dm.PlayerList;
        List<EnemyCharacter> enemyList = dm.EnemyList;
        foreach(PlayerCharacter player in playerList)
        {
            this.playerList.Add(player.GetComponent<BattlePlayerCharacter>());
        }
        foreach(EnemyCharacter enemy in enemyList)
        {
            this.enemyList.Add(enemy.GetComponent<BattleEnemyCharacter>());
        }
    }
}
