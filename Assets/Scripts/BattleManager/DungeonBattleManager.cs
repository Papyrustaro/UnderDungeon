using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBattleManager : MonoBehaviour
{
    //[SerializeField]private List<BattlePlayerCharacter> playerList = new List<BattlePlayerCharacter>(); //自分のパーティ
    //[SerializeField]private List<BattleEnemyCharacter> enemyList = new List<BattleEnemyCharacter>(); //敵のパーティ
    private double hpPassiveRate = 2, atkPassiveRate = 1, spdPassiveRate = 2; 
    private double toDamageFirePassiveRate = 1, toDamageAquaPassiveRate = 1, toDamageTreePassiveRate = 1;
    [SerializeField]private List<BattleCharacter> charaList = new List<BattleCharacter>();

    private bool finishAction = true;
    private int charaNum; //戦闘に参加しているchara数
    private List<int> playerIndex;
    private List<int> enemyIndex;
    private int nextActionIndex = 0;


    private void Awake()
    {
        //SetCharacter();
    }
    private void Start()
    {
        charaNum = charaList.Count;
    }

    private void Update()
    {
        if (finishAction)
        {
            if (charaList[nextActionIndex].IsEnemy)
            {

            }
            else
            {

            }
        }
    }

    private void NormalAttack(BattleCharacter attacker, BattleCharacter target)
    {
        Debug.Log(attacker.CharaClass.CharaName + "の攻撃");
        target.DecreaseHp(attacker.Atk);
    }
    private void SortCharacterBySpd()
    {
        charaList.Sort((a, b) => (int)(b.Spd - a.Spd));
    }
    public void SetCharacter()
    {
        DungeonManager dm = GetComponent<DungeonManager>();
        List<PlayerCharacter> playerList = dm.PlayerList;
        List<EnemyCharacter> enemyList = dm.EnemyList;
        foreach(PlayerCharacter player in playerList)
        {
            //this.playerList.Add(player.GetComponent<BattlePlayerCharacter>());
            this.charaList.Add(player.GetComponent<BattleCharacter>());
        }
        foreach(EnemyCharacter enemy in enemyList)
        {
            //this.enemyList.Add(enemy.GetComponent<BattleEnemyCharacter>());
            this.charaList.Add(enemy.GetComponent<BattleCharacter>());
        }
    }
}
