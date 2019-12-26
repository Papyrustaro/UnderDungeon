using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBattleManager : MonoBehaviour
{
    private List<BattlePlayerCharacter> playerList = new List<BattlePlayerCharacter>(); //自分のパーティ
    private List<BattleEnemyCharacter> enemyList = new List<BattleEnemyCharacter>(); //敵のパーティ
    private double hpPassiveRate = 2, atkPassiveRate = 1, spdPassiveRate = 2; 
    private double toDamageFirePassiveRate = 1, toDamageAquaPassiveRate = 1, toDamageTreePassiveRate = 1;
    private List<BattleCharacter> charaList = new List<BattleCharacter>();


    private void Awake()
    {
        SetCharacter();
    }
    private void Start()
    {
        Debug.Log("ソート前");
        Debug.Log(charaList.Count);
        foreach(BattleCharacter bc in charaList)
        {
            Debug.Log(bc.Spd);
        }
        SortCharacterBySpd();
        Debug.Log("ソート後");
        foreach(BattleCharacter bc in charaList)
        {
            Debug.Log(bc.Spd);
        }
    }

    private void Update()
    {
        
    }

    private void SortCharacterBySpd()
    {
        charaList.Sort((a, b) => b.Spd - a.Spd);
    }
    public void SetCharacter()
    {
        DungeonManager dm = GetComponent<DungeonManager>();
        List<PlayerCharacter> playerList = dm.PlayerList;
        List<EnemyCharacter> enemyList = dm.EnemyList;
        foreach(PlayerCharacter player in playerList)
        {
            this.playerList.Add(player.GetComponent<BattlePlayerCharacter>());
            this.charaList.Add(player.GetComponent<BattleCharacter>());
        }
        foreach(EnemyCharacter enemy in enemyList)
        {
            this.enemyList.Add(enemy.GetComponent<BattleEnemyCharacter>());
            this.charaList.Add(enemy.GetComponent<BattleCharacter>());
        }
    }
}
