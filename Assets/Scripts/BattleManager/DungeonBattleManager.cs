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
    [SerializeField] private BattleActiveEffectsFunc activeEffectFuncs;
    [SerializeField] private BattleUIManager uiManager;

    [SerializeField] private List<BattleActiveItem> haveItems;

    private bool finishAction = true;
    private int charaNum; //戦闘に参加しているchara数
    private List<int> playerIndex, playerAliveIndex;
    private List<int> enemyIndex, enemyAliveIndex;
    private int nextActionIndex = 99;
    private bool inputWaiting = false; //プレイヤーの入力まち
    private bool inputTargetWaiting = false;
    private BattleActiveEffect waitingEffect = null;
    public List<BattleCharacter> AliveCharaList => GetAliveList(this.charaList);
    private int targetIndex;
    private BattleCharacter target;
    private E_BattleSituation currentSituation = E_BattleSituation.WaitFinishAction;

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
        //this.targetIndex = this.charaList.IndexOf(GetAliveList(this.enemyList)[0]);
        this.target = this.GetAliveList(this.enemyList)[0];
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
        }else
        {
            PlayerSelect();
        }
    }
    public void SetInputTarget(BattleCharacter target)
    {
        if (!target.IsAlive) { Debug.Log("もう死んでいます"); return; } //とりあえず
        //this.targetIndex = charaList.IndexOf(target);
        this.target = target;
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
                EnemyAction(charaList[nextActionIndex]);
            }
            else
            {
                if (!this.target.IsEnemy || !this.target.IsAlive) this.target = GetAliveList(this.enemyList)[0];
                ShowPlayerActionSelect();
                //ShowAnnounce(charaList[nextActionIndex].CharaClass.CharaName + "のばん");
                //ShowActiveSkillSelect(charaList[nextActionIndex]);
                this.finishAction = false;
            }
        }
        else
        {
            AdvanceTurn();
        }
    }
    private void DebugFunc(BattleCharacter target)
    {
        Debug.Log(target.Hp + "/" + target.MaxHp);
    }
    private void PlayerSelect()
    {
        switch (this.currentSituation)
        {
            case E_BattleSituation.PlayerSelectAction:
                InputPlayerAction();
                break;
            case E_BattleSituation.PlayerSelectActiveSkill:
                //ShowActiveSkillSelect(charaList[nextActionIndex]);
                InputActuateSkill();
                break;
            case E_BattleSituation.PlayerSelectActiveItem:
                InputActuateItem();
                break;
            case E_BattleSituation.PlayerSelectSkillTarget:
                if(!this.inputTargetWaiting) InputActuateSkill();
                break;
            case E_BattleSituation.PlayerSelectItemTarget:
                if(!this.inputTargetWaiting) InputActuateItem();
                break;
            case E_BattleSituation.WaitFinishAction:
                break;
        }
    }
    private void InputPlayerAction()
    {
        if (Input.GetKeyDown(((int)E_PlayerAction.NormalAttack).ToString()))
        {
            if (charaList[nextActionIndex].NormalAttackToAllTurn > 0) NormalAttack(charaList[nextActionIndex], GetAliveList(this.enemyList));
            else if (GetAttractingCharacter(this.charaList[nextActionIndex].Element, this.enemyList) == null) NormalAttack(charaList[nextActionIndex], target);
            else NormalAttack(charaList[nextActionIndex], GetAttractingCharacter(this.charaList[nextActionIndex].Element, this.enemyList));
        }
        if (Input.GetKeyDown(((int)E_PlayerAction.InvokeSkill).ToString()))
        {
            this.currentSituation = E_BattleSituation.PlayerSelectActiveSkill;
            ShowActiveSkillSelect(charaList[nextActionIndex]);
        }
        if (Input.GetKeyDown(((int)E_PlayerAction.UseItem).ToString()))
        {
            this.currentSituation = E_BattleSituation.PlayerSelectActiveItem;
            ShowActiveItemSelect();
        }
        if (Input.GetKeyDown(((int)E_PlayerAction.Protect).ToString()))
        {
            Debug.Log("防御した(未実装)");
        }
    }
    //なぜこの処理をこちら側でするか→oneAllyのときターゲット入力が必要だから→それが解決できたらFuncクラスでの実装でもいいか?
    private void InvokeEffect(BattleCharacter invoker, BattleActiveEffect effect)
    {
        switch (effect.TargetType)
        {
            case E_TargetType.All:
                this.activeEffectFuncs.EffectFunc(effect, invoker, charaList);
                break;
            case E_TargetType.OneEnemy:
                if (invoker.IsEnemy)
                {
                    if (GetAttractingCharacter(effect.EffectElement, this.allyList) == null) this.activeEffectFuncs.EffectFunc(effect, invoker, new List<BattleCharacter>() { ListManager.GetRandomIndex<BattleCharacter>(GetAliveList(this.allyList)) });
                    else this.activeEffectFuncs.EffectFunc(effect, invoker, new List<BattleCharacter>() { GetAttractingCharacter(effect.EffectElement, this.allyList) });
                }
                else
                {
                    if (GetAttractingCharacter(effect.EffectElement, this.enemyList) == null) this.activeEffectFuncs.EffectFunc(effect, invoker, new List<BattleCharacter>() { target });
                    else this.activeEffectFuncs.EffectFunc(effect, invoker, new List<BattleCharacter>() { GetAttractingCharacter(effect.EffectElement, this.enemyList) });
                }
                break;
            case E_TargetType.AllAlly:
                if (invoker.IsEnemy) this.activeEffectFuncs.EffectFunc(effect, invoker, this.enemyList);
                else this.activeEffectFuncs.EffectFunc(effect, invoker, allyList);
                break;
            case E_TargetType.AllEnemy:
                if (invoker.IsEnemy) this.activeEffectFuncs.EffectFunc(effect, invoker, this.allyList);
                else this.activeEffectFuncs.EffectFunc(effect, invoker, enemyList);
                break;
            case E_TargetType.Self:
                this.activeEffectFuncs.EffectFunc(effect, invoker, new List<BattleCharacter>() { invoker });
                break;
            case E_TargetType.OneAlly:
                if (invoker.IsEnemy)
                {
                    this.activeEffectFuncs.EffectFunc(effect, invoker, new List<BattleCharacter>() { ListManager.GetRandomIndex<BattleCharacter>(GetAliveList(this.enemyList)) });
                }
                else
                {
                    //プレイヤー入力によるtargetの指定処理
                    if (this.target.IsEnemy)
                    {
                        this.inputTargetWaiting = true;
                        if (this.currentSituation == E_BattleSituation.PlayerSelectActiveSkill) this.currentSituation = E_BattleSituation.PlayerSelectSkillTarget;
                        else if (this.currentSituation == E_BattleSituation.PlayerSelectActiveItem) this.currentSituation = E_BattleSituation.PlayerSelectItemTarget;
                        this.uiManager.PromptSelectTargetOneAlly();
                        return;
                    }
                    this.activeEffectFuncs.EffectFunc(effect, invoker, new List<BattleCharacter>() { this.target });
                }
                break;
        }
        AdvanceTurn();

    }
    private void ShowPlayerActionSelect()
    {
        ShowAnnounce(charaList[nextActionIndex].CharaClass.CharaName + "のばん");
        ShowAnnounce((int)E_PlayerAction.NormalAttack+":通常攻撃 "+(int)E_PlayerAction.InvokeSkill+":スキル "+(int)E_PlayerAction.UseItem+":アイテム "+(int)E_PlayerAction.Protect+":防御");
        this.currentSituation = E_BattleSituation.PlayerSelectAction;
        this.finishAction = false;
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
    private void ShowActiveItemSelect()
    {
        string s = "";
        int i = 0; 
        foreach(BattleActiveItem item in this.haveItems)
        {
            s += " " + i.ToString() + ":" + item.EffectName;
            i++;
        }
        ShowAnnounce(s);
    }
    private void InputActuateSkill()
    {
        BattleCharacter invoker = charaList[nextActionIndex];
        if(this.waitingEffect != null)
        {
            InvokeEffect(invoker, this.waitingEffect);
            this.waitingEffect = null;
            return;
        }


        for(int i = 0; i < invoker.BattleActiveSkillID.Count; i++) //4で固定されちゃうかも
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                if(this.activeEffectFuncs.GetBattleActiveSkill(invoker.BattleActiveSkillID[i]).NeedSkillPoint > invoker.HaveSkillPoint)
                {
                    ShowAnnounce("スキルポイントが足りません");
                    this.finishAction = true; return;
                }
                InvokeEffect(invoker, this.activeEffectFuncs.GetBattleActiveSkill(invoker.BattleActiveSkillID[i]));
                if (this.currentSituation == E_BattleSituation.PlayerSelectSkillTarget)
                {
                    this.waitingEffect = this.activeEffectFuncs.GetBattleActiveSkill(invoker.BattleActiveSkillID[i]);
                }
            }
        }
    }
    private void InputActuateItem()
    {
        BattleCharacter invoker = charaList[nextActionIndex];
        if (this.waitingEffect != null)
        {
            InvokeEffect(invoker, this.waitingEffect);
            this.waitingEffect = null;
            return;
        }
        for (int i = 0; i < this.haveItems.Count; i++) 
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                InvokeEffect(invoker, this.haveItems[i]);
                if (this.currentSituation == E_BattleSituation.PlayerSelectItemTarget)
                {
                    this.waitingEffect = this.haveItems[i];
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
        for(int i = 0; i < attacker.NormalAttackNum; i++)
        {
            target.DamagedByNormalAttack(attacker.NormalAttackPower, attacker.Element);
        }
        AdvanceTurn();
    }
    private void NormalAttack(BattleCharacter attacker, List<BattleCharacter> targetList)
    {
        ShowAnnounce(attacker.CharaClass.CharaName + "の通常攻撃");
        for(int i = 0; i < attacker.NormalAttackNum; i++)
        {
            foreach (BattleCharacter target in targetList)
            {
                target.DamagedByNormalAttack(attacker.NormalAttackPower, attacker.Element);
            }
        }
        AdvanceTurn();
    }
    private void AdvanceTurn()
    {
        if(this.charaList[nextActionIndex].IsAlive) this.charaList[nextActionIndex].AddHaveSkillPoint(1);
        this.finishAction = true;
        this.nextActionIndex++;
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
            case E_BattleSituation.PlayerSelectActiveItem:
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
    private void EnemyAction(BattleCharacter actionEnemy)
    {
        if (!actionEnemy.EC.HaveSpecialAI)
        {
            List<BattleActiveSkill> useableSkill = new List<BattleActiveSkill>();
            foreach(E_BattleActiveSkill skillID in actionEnemy.EC.HaveBattleActtiveSkillID)
            {
                if(actionEnemy.HaveSkillPoint >= this.activeEffectFuncs.GetBattleActiveSkill(skillID).NeedSkillPoint)
                {
                    useableSkill.Add(this.activeEffectFuncs.GetBattleActiveSkill(skillID));
                }
            }
            int action = UnityEngine.Random.Range(0, useableSkill.Count + 1);
            if (action == useableSkill.Count)
            {   //通常攻撃
                if (actionEnemy.NormalAttackToAllTurn > 0) NormalAttack(actionEnemy, GetAliveList(this.allyList));
                else if (GetAttractingCharacter(actionEnemy.Element, this.allyList) != null) NormalAttack(actionEnemy, GetAttractingCharacter(actionEnemy.Element, this.allyList));
                else NormalAttack(actionEnemy, ListManager.GetRandomIndex<BattleCharacter>(GetAliveList(this.allyList)));
            }
            else
            {
                InvokeEffect(actionEnemy, useableSkill[action]); //スキル発動
            }
        }
        else
        {
            //特殊な処理
            actionEnemy.EC.EnemyAI.EnemyActionFunc(this.enemyList, this.allyList, actionEnemy, this.activeEffectFuncs);
            AdvanceTurn();
        }
    }
}
