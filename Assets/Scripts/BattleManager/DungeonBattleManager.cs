using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonBattleManager : MonoBehaviour
{
    //[SerializeField]private List<BattlePlayerCharacter> playerList = new List<BattlePlayerCharacter>(); //自分のパーティ
    //[SerializeField]private List<BattleEnemyCharacter> enemyList = new List<BattleEnemyCharacter>(); //敵のパーティ
    private double hpPassiveRate = 2, atkPassiveRate = 1, spdPassiveRate = 2; 
    private double toDamageFirePassiveRate = 1, toDamageAquaPassiveRate = 1, toDamageTreePassiveRate = 1;
    [SerializeField]private List<BattleCharacter> charaList = new List<BattleCharacter>();
    [SerializeField] private Text announceText;

    private bool finishAction = true;
    private int charaNum; //戦闘に参加しているchara数
    private List<int> playerIndex, playerAliveIndex;
    private List<int> enemyIndex, enemyAliveIndex;
    private int nextActionIndex = 99;
    private bool inputWaiting = false; //プレイヤーの入力まち
    public List<BattleCharacter> AliveCharaList => GetAliveList(this.charaList);
    private int targetIndex;


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
            SetCharaIndex();
            //DebugFunc(); finishAction = false;
            if (charaNum <= nextActionIndex) //1週したら
            {
                nextActionIndex = 0;
                SortCharacterBySpd();
            }
            if(charaList[nextActionIndex].Hp > 0)
            {
                if (charaList[nextActionIndex].IsEnemy)
                {
                    NormalAttack(charaList[nextActionIndex], charaList[ListManager.GetRandomIndex<int>(playerAliveIndex)]);
                }
                else
                {
                    ShowAnnounce(charaList[nextActionIndex].CharaClass.CharaName + "のばん");
                    //announceText.text = "こうげき対象を選択してください";
                    this.inputWaiting = true;
                    this.finishAction = false;
                }
            }
            nextActionIndex++;
        }
        else
        {
            if(!this.inputWaiting) //finishAction == プレイヤーの行動が終わったと捉えたときのみ
            {
                NormalAttack(charaList[nextActionIndex], charaList[targetIndex]);
                this.finishAction = true;
            }
        }
    }
    public void SetInputTarget(BattleCharacter bc, E_TargetType targetType)
    {
        if (bc.Hp < 0) { announceText.text = "そのキャラもう死んでるyo"; return; } //とりあえず
        if((bc.IsEnemy && targetType == E_TargetType.OneEnemy) || (!bc.IsEnemy && targetType == E_TargetType.OneAlly))
        {
            this.targetIndex = charaList.IndexOf(bc);
            this.inputWaiting = false;
        }
    }
    private void DebugFunc()
    {
        Debug.Log(charaList[ListManager.GetRandomIndex<int>(playerAliveIndex)]);
    }
    private List<BattleCharacter> GetAliveList(List<BattleCharacter> bcList)
    {
        //list.RemoveAll(bc => bc.Hp < 0);
        List<BattleCharacter> list = new List<BattleCharacter>();
        foreach(BattleCharacter bc in bcList)
        {
            if (bc.Hp > 0) list.Add(bc);
        }
        return list;
    }
    private List<int> GetAliveIndexListOfCharaList(List<BattleCharacter> bcList)
    {
        List<int> list = new List<int>();
        foreach(BattleCharacter bc in bcList)
        {
            if (bc.Hp > 0) list.Add(charaList.IndexOf(bc));
        }
        return list;
    }
    private void NormalAttack(BattleCharacter attacker, BattleCharacter target)
    {
        ShowAnnounce(attacker.CharaClass.CharaName + "の攻撃");
        int damage = (int)target.DecreaseHp(attacker.Atk);
        ShowAnnounce(target.CharaClass.CharaName + "は" + damage + "のダメージを受けた");
    }
    private int DecideTargetIndexRandom()
    {
        if(this.playerAliveIndex == null)
        {
            Debug.Log("プレイヤーのキャラ全滅してるよ");
        }
        return playerAliveIndex[UnityEngine.Random.Range(0, playerAliveIndex.Count)];
    }
    private void ShowAnnounce(string s)
    {
        Debug.Log(s);
        this.announceText.text = s;
    }
    private void SetCharaIndex()
    {
        this.playerIndex = new List<int>();
        this.enemyIndex = new List<int>();
        this.playerAliveIndex = new List<int>();
        this.enemyAliveIndex = new List<int>();
        int index = 0;
        foreach(BattleCharacter bc in charaList)
        {
            if (bc.IsEnemy)
            {
                enemyIndex.Add(index);
                if (bc.Hp > 0) enemyAliveIndex.Add(index);
            }
            else
            {
                playerIndex.Add(index);
                if (bc.Hp > 0) playerAliveIndex.Add(index);
            }
            index++;
        }
    }
    private void SortCharacterBySpd()
    {
        charaList.Sort((a, b) => (int)(b.Spd - a.Spd));
        SetCharaIndex();
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
