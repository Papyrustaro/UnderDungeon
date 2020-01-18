using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonBattleManager : MonoBehaviour
{
    private double hpPassiveRate = 2, atkPassiveRate = 1, spdPassiveRate = 2; 
    private double toDamageFirePassiveRate = 1, toDamageAquaPassiveRate = 1, toDamageTreePassiveRate = 1;
    [SerializeField]private List<BattleCharacter> charaList = new List<BattleCharacter>();
    [SerializeField] private Text announceText;
    [SerializeField] private BattleActiveSkillsFunc activeSkillFuncs;
    [SerializeField] private BattleUIManager uiManager;

    private bool finishAction = true;
    private int charaNum; //戦闘に参加しているchara数
    private List<int> playerIndex, playerAliveIndex;
    private List<int> enemyIndex, enemyAliveIndex;
    private int nextActionIndex = 99;
    private bool inputWaiting = false; //プレイヤーの入力まち
    private bool inputTargetWaiting = false;
    private BattleActiveSkill waitingSkill = null;
    public List<BattleCharacter> AliveCharaList => GetAliveList(this.charaList);
    private int targetIndex;
    private E_BattleSituation currentSituation;

    private List<BattleCharacter> allyList = new List<BattleCharacter>();
    private List<BattleCharacter> enemyList = new List<BattleCharacter>();

    private bool debug = true;

    private void Awake()
    {
        //SetCharacter();
    }
    private void Start()
    {
        charaNum = charaList.Count;
        foreach(BattleCharacter bc in charaList)
        {
            if (bc.IsEnemy) this.enemyList.Add(bc);
            else this.allyList.Add(bc);
        }
    }

    private void Update()
    {
        /*if (debug)
        {
            DebugFunc(); debug = false;
        }
        return;*/
        if (finishAction)
        {
            //DebugFunc(); finishAction = false;
            if (charaNum <= nextActionIndex) //1週したら
            {
                nextActionIndex = 0;
                SortCharacterBySpd();
            }
            CharacterAction();
        }
        else if(!this.inputTargetWaiting)
        {
            InputActuateSkill();
        }
    }
    public void SetInputTarget(BattleCharacter target)
    {
        if (target.Hp < 0) { announceText.text = "そのキャラもう死んでるyo"; return; } //とりあえず
        this.targetIndex = charaList.IndexOf(target);
        BattleUIManager.ShowAnnounce("target: " + target.CharaClass.CharaName);
        this.inputTargetWaiting = false;

        if (!target.IsEnemy)
        {
            uiManager.SetActiveAllyTargetButtons(false);
        }
    }
    private void CharacterAction()
    {
        if (charaList[nextActionIndex].IsAlive)
        {
            if (charaList[nextActionIndex].IsEnemy)
            {
                //Debug.Log(ListManager.GetRandomIndex<BattleCharacter>(GetAliveList(this.allyList)).CharaClass.CharaName);
                NormalAttack(charaList[nextActionIndex], ListManager.GetRandomIndex<BattleCharacter>(GetAliveList(this.allyList)));
                nextActionIndex++;
            }
            else
            {
                ShowAnnounce(charaList[nextActionIndex].CharaClass.CharaName + "のばん");
                ShowActiveSkillSelect(charaList[nextActionIndex]);
                //this.inputWaiting = true;
                this.finishAction = false;
            }
        }
        else
        {
            nextActionIndex++;
        }
    }
    private void DebugFunc(BattleCharacter target)
    {
        Debug.Log(target.Hp + "/" + target.MaxHp);
    }
    //なぜこの処理をこちら側でするか→oneAllyのときターゲット入力が必要だから→それが解決できたらFuncクラスでの実装でもいいか?
    private void InvokeSkill(BattleCharacter invoker, BattleActiveSkill skill)
    {
        switch (skill.TargetType)
        {
            case E_TargetType.All:
                this.activeSkillFuncs.SkillFunc(skill, invoker, charaList);
                break;
            case E_TargetType.OneEnemy:
                if (GetAttractingCharacter(skill.SkillElement, this.enemyList) == null) this.activeSkillFuncs.SkillFunc(skill, invoker, new List<BattleCharacter>() { charaList[targetIndex] });
                else this.activeSkillFuncs.SkillFunc(skill, invoker, new List<BattleCharacter>() { GetAttractingCharacter(skill.SkillElement, this.enemyList) });
                break;
            case E_TargetType.AllAlly:
                this.activeSkillFuncs.SkillFunc(skill, invoker, allyList);
                break;
            case E_TargetType.AllEnemy:
                this.activeSkillFuncs.SkillFunc(skill, invoker, enemyList);
                break;
            case E_TargetType.Self:
                this.activeSkillFuncs.SkillFunc(skill, invoker, new List<BattleCharacter>() { invoker });
                break;
            case E_TargetType.OneAlly:
                //プレイヤー入力によるtargetIndexの指定処理
                if (charaList[targetIndex].IsEnemy)
                {
                    this.inputTargetWaiting = true;
                    this.uiManager.PromptSelectTargetOneAlly();
                    return;
                }
                this.activeSkillFuncs.SkillFunc(skill, invoker, new List<BattleCharacter>() { charaList[targetIndex] });
                break;
        }

        this.finishAction = true;
        this.nextActionIndex++;
    }
    private void ShowActiveSkillSelect(BattleCharacter bc)
    {
        string s = "";
        int i = 0;
        foreach(E_BattleActiveSkill id in bc.BattleActiveSkillID)
        {
            s += " " + i.ToString() + ":" + id.ToString();
            i++;
        }
        ShowAnnounce(s);
    }
    private void InputActuateSkill()
    {
        BattleCharacter invoker = charaList[nextActionIndex];
        if(this.waitingSkill != null)
        {
            InvokeSkill(invoker, this.waitingSkill);
            this.waitingSkill = null;
            return;
        }


        for(int i = 0; i < invoker.BattleActiveSkillID.Count; i++) //4で固定されちゃうかも
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                if(this.activeSkillFuncs.GetBattleActiveSkill(invoker.BattleActiveSkillID[i]).NeedSkillPoint > invoker.HaveSkillPoint)
                {
                    ShowAnnounce("スキルポイントが足りません");
                    this.finishAction = true; return;
                }
                InvokeSkill(invoker, this.activeSkillFuncs.GetBattleActiveSkill(invoker.BattleActiveSkillID[i]));
                if (this.inputTargetWaiting)
                {
                    this.waitingSkill = this.activeSkillFuncs.GetBattleActiveSkill(invoker.BattleActiveSkillID[i]);
                }
            }
        }
    }
    private List<BattleCharacter> GetAliveList(List<BattleCharacter> bcList)
    {
        List<BattleCharacter> list = new List<BattleCharacter>();
        foreach(BattleCharacter bc in bcList)
        {
            if (bc.IsAlive) list.Add(bc);
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
        ShowAnnounce(attacker.CharaClass.CharaName + "の通常攻撃");
        target.DamagedByNormalAttack(attacker.NormalAttackPower, attacker.Element);
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
        //SetCharaIndex();
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
    private void ShowAnnounce(E_BattleSituation scene)
    {
        switch (scene)
        {
            case E_BattleSituation.PlayerSelectAction:
                ShowAnnounce(charaList[nextActionIndex].CharaClass.CharaName + "はどうする");
                ShowAnnounce("0:通常攻撃 1:防御 2:スキル 3:アイテム");
                break;
            case E_BattleSituation.PlayerSelectActiveSkill:
                ShowActiveSkillSelect(charaList[nextActionIndex]);
                break;
            case E_BattleSituation.PlayerSelectItem:
                ShowAnnounce("アイテムを選択してください(未実装)");
                break;
            case E_BattleSituation.PlayerSelectSkillTarget:
                ShowAnnounce("スキルの対象を選択してください");
                break;
        }
    }
    public BattleCharacter GetAttractingCharacter(E_Element attractElement, List<BattleCharacter> charaList) //charaListの先頭から検索していくため、sortされないlistを渡す必要がある
    {
        foreach(BattleCharacter bc in charaList)
        {
            if (bc.IsAlive && bc.IsAttracting(attractElement)) return bc;
        }
        return null;
    }
}
