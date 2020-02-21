using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonBattleManager : MonoBehaviour
{
    [SerializeField] private List<BattleCharacter> charaList = new List<BattleCharacter>();
    [SerializeField] private Text announceText;
    [SerializeField] private BattleActiveEffectsFunc activeEffectFuncs;
    [SerializeField] private BattlePassiveEffectsFunc passiveEffectFuncs;
    [SerializeField] private BattleUIManager uiManager;

    [SerializeField] private List<BattleActiveItem> haveActiveItems;
    [SerializeField] private List<BattlePassiveItem> havePassiveItems;

    private bool finishAction = true;
    private int charaNum; //戦闘に参加しているchara数
    private int nextActionIndex = 99;
    private bool inputTargetWaiting = false;
    private BattleActiveEffect waitingEffect = null;
    private BattleCharacter target;
    private E_BattleSituation currentSituation = E_BattleSituation.SetParameterBeforeStartBattle;

    private List<BattleCharacter> allyList = new List<BattleCharacter>();
    private List<BattleCharacter> enemyList = new List<BattleCharacter>();

    private void Start()
    {
        charaNum = charaList.Count;
        foreach(BattleCharacter bc in charaList)
        {
            if(!bc.StatusChange) bc.Start(); //BattleCharacterのStartが終了していないとき、呼ぶ
            if (bc.IsEnemy) this.enemyList.Add(bc);
            else this.allyList.Add(bc);
        }
        
        this.target = this.GetAliveList(this.enemyList)[0];
        this.currentSituation = E_BattleSituation.SetParameterBeforeStartBattle;
        SetPassiveEffect();
        foreach (BattleCharacter bc in this.allyList)
        {
            bc.Hp = bc.MaxHp;
        }
        this.currentSituation = E_BattleSituation.WaitFinishAction;
    }

    private void Update()
    {
        if (finishAction)
        {
            if(charaNum <= nextActionIndex) //1周したら
            {
                nextActionIndex = 0;
                ReSetPassiveEffect();
                SortCharacterBySpd();
            }
            else
            {
                ReSetPassiveEffect();
            }
            this.charaList[nextActionIndex].SetBeforeAction();
            CharacterAction();
        }else
        {
            PlayerSelect();
        }
    }

    /// <summary>
    /// ボタンで取得したtarget情報をセット
    /// </summary>
    /// <param name="target">ターゲット</param>
    public void SetInputTarget(BattleCharacter target)
    {
        if (!target.IsAlive) { Debug.Log(target.CharaClass.CharaName + "は、倒れています"); return; } 
        this.target = target;
        Debug.Log("target: " + target.CharaClass.CharaName);
        this.inputTargetWaiting = false;

        if (!target.IsEnemy) //ボタンの有効を敵に戻す
        {
            uiManager.SetActiveAllyTargetButtons(false);
        }
    }

    /// <summary>
    /// 倒れていれば次のキャラへ、敵ならAI行動、プレイヤーなら入力待機に移行する
    /// </summary>
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
                this.finishAction = false;
            }
        }
        else
        {
            AdvanceTurn();
        }
    }

    private void DebugFunc()
    {
    }

    /// <summary>
    /// currentSituationによって、プレイヤーの入力遷移を分岐する
    /// </summary>
    private void PlayerSelect()
    {
        switch (this.currentSituation)
        {
            case E_BattleSituation.PlayerSelectAction:
                InputPlayerAction();
                break;
            case E_BattleSituation.PlayerSelectActiveSkill:
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

    /// <summary>
    /// 4つの行動(通常攻撃、スキル、アイテム、防御)からプレイヤーの入力で分岐する
    /// </summary>
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
        if (Input.GetKeyDown(((int)E_PlayerAction.Defend).ToString()))
        {
            Defend(charaList[nextActionIndex]);
        }
    }


    /// <summary>
    /// ActiveEffect(skill or item)の発動、ターン経過
    /// </summary>
    /// <param name="invoker">Effect発動者</param>
    /// <param name="effect">発動するActiveEffect</param>
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

    /// <summary>
    /// 防御処理、ターン経過
    /// </summary>
    /// <param name="defendChara">行動キャラ</param>
    private void Defend(BattleCharacter defendChara)
    {
        defendChara.IsDefending = true;
        ShowAnnounce(defendChara.CharaClass.CharaName + "は防御している");
        AdvanceTurn();
    }

    /// <summary>
    /// プレイヤーの行動選択のテキスト出力
    /// </summary>
    private void ShowPlayerActionSelect()
    {
        ShowAnnounce(charaList[nextActionIndex].CharaClass.CharaName + "のばん");
        ShowAnnounce((int)E_PlayerAction.NormalAttack+":通常攻撃 "+(int)E_PlayerAction.InvokeSkill+":スキル "+(int)E_PlayerAction.UseItem+":アイテム "+(int)E_PlayerAction.Defend+":防御");
        this.currentSituation = E_BattleSituation.PlayerSelectAction;
        this.finishAction = false;
    }

    /// <summary>
    /// 保持ActiveSkillをテキストに表示
    /// </summary>
    /// <param name="bc">スキルを表示するキャラ</param>
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

    /// <summary>
    /// 所持しているActiveItemsをテキストに表示する
    /// </summary>
    private void ShowActiveItemSelect()
    {
        string s = "";
        int i = 0; 
        foreach(BattleActiveItem item in this.haveActiveItems)
        {
            s += " " + i.ToString() + ":" + item.EffectName;
            i++;
        }
        ShowAnnounce(s);
    }

    /// <summary>
    /// ActiveSkillの選択画面で、番号入力で対応スキルを発動
    /// </summary>
    private void InputActuateSkill()
    {
        BattleCharacter invoker = charaList[nextActionIndex];
        if(this.waitingEffect != null)
        {
            InvokeEffect(invoker, this.waitingEffect);
            this.waitingEffect = null;
            return;
        }


        for(int i = 0; i < invoker.BattleActiveSkillID.Count; i++) 
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

    /// <summary>
    /// ActiveItemの選択画面で、番号入力で対応アイテムを発動
    /// </summary>
    private void InputActuateItem()
    {
        BattleCharacter invoker = charaList[nextActionIndex];
        if (this.waitingEffect != null)
        {
            InvokeEffect(invoker, this.waitingEffect);
            this.waitingEffect = null;
            return;
        }
        for (int i = 0; i < this.haveActiveItems.Count; i++) 
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                InvokeEffect(invoker, this.haveActiveItems[i]);
                if (this.currentSituation == E_BattleSituation.PlayerSelectItemTarget)
                {
                    this.waitingEffect = this.haveActiveItems[i];
                }
            }
        }
    }

    /// <summary>
    /// 生存しているキャラをリストで返す
    /// </summary>
    /// <param name="bcList">探索するキャラリスト</param>
    /// <returns>生存リスト</returns>
    private List<BattleCharacter> GetAliveList(List<BattleCharacter> bcList)
    {
        List<BattleCharacter> list = new List<BattleCharacter>();
        foreach(BattleCharacter bc in bcList)
        {
            if (bc.IsAlive) list.Add(bc);
        }
        return list;
    }
 
    /// <summary>
    /// 通常攻撃、ターン経過処理
    /// </summary>
    /// <param name="attacker">攻撃者</param>
    /// <param name="target">攻撃対象</param>
    private void NormalAttack(BattleCharacter attacker, BattleCharacter target)
    {
        ShowAnnounce(attacker.CharaClass.CharaName + "の通常攻撃");
        for(int i = 0; i < attacker.NormalAttackNum; i++)
        {
            target.DamagedByNormalAttack(attacker.NormalAttackPower, attacker.Element);
        }
        AdvanceTurn();
    }

    /// <summary>
    /// 通常攻撃、ターン経過処理
    /// </summary>
    /// <param name="attacker">攻撃者</param>
    /// <param name="targetList">攻撃対象リスト</param>
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

    /// <summary>
    /// ターン経過処理
    /// </summary>
    private void AdvanceTurn()
    {
        if (this.charaList[nextActionIndex].IsAlive) this.charaList[nextActionIndex].SetAfterAction();
        this.finishAction = true;
        this.nextActionIndex++;
    }

    /// <summary>
    /// AnnounceText.textを変える
    /// </summary>
    /// <param name="s">代入するtext</param>
    private void ShowAnnounce(string s)
    {
        Debug.Log(s);
        this.announceText.text = s;
    }

    /// <summary>
    /// charaListをSpd順にソート
    /// </summary>
    private void SortCharacterBySpd()
    {
        charaList.Sort((a, b) => (int)(b.Spd - a.Spd));
    }

    /// <summary>
    /// BattleManagerからBattleCharacterをセット
    /// </summary>
    public void SetCharacter()
    {
        DungeonManager dm = GetComponent<DungeonManager>();
        List<PlayerCharacter> playerList = dm.PlayerList;
        List<EnemyCharacter> enemyList = dm.EnemyList;
        foreach(PlayerCharacter player in playerList)
        {
            this.allyList.Add(player.GetComponent<BattleCharacter>());
            this.charaList.Add(player.GetComponent<BattleCharacter>());
        }
        foreach(EnemyCharacter enemy in enemyList)
        {
            this.enemyList.Add(enemy.GetComponent<BattleCharacter>());
            this.charaList.Add(enemy.GetComponent<BattleCharacter>());
        }
    }

    /// <summary>
    /// 攻撃を集中しているキャラクターをリストの先頭から探索
    /// </summary>
    /// <param name="attractElement">集中している属性</param>
    /// <param name="charaList">探索するキャラリスト</param>
    /// <returns>最初に見つかった集中させているキャラ(いなければnull)</returns>
    public BattleCharacter GetAttractingCharacter(E_Element attractElement, List<BattleCharacter> charaList) //charaListの先頭から検索していくため、sortされないlistを渡す必要がある
    {
        foreach(BattleCharacter bc in charaList)
        {
            if (bc.IsAlive && bc.IsAttracting(attractElement)) return bc;
        }
        return null;
    }

    /// <summary>
    /// 敵のAIにそって自動で行動する
    /// </summary>
    /// <param name="actionEnemy">行動する敵キャラ</param>
    private void EnemyAction(BattleCharacter actionEnemy)
    {
        if (!actionEnemy.EC.HaveSpecialAI)
        {
            List<BattleActiveSkill> useableSkill = new List<BattleActiveSkill>();
            foreach(E_BattleActiveSkill skillID in actionEnemy.EC.HaveBattleActtiveSkillID)
            {
                BattleActiveSkill skill = this.activeEffectFuncs.GetBattleActiveSkill(skillID);
                if(actionEnemy.HaveSkillPoint >= skill.NeedSkillPoint)
                {
                    if(skill.EffectType != E_BattleActiveEffectType.HP回復 || BattleMethod.GetLowerHpChara(this.enemyList, 0.5) != null) useableSkill.Add(this.activeEffectFuncs.GetBattleActiveSkill(skillID));
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

    /// <summary>
    /// Set all passiveEffects by skills and items.
    /// </summary>
    public void SetPassiveEffect()
    {
        foreach(BattleCharacter bc in this.allyList)
        {
            bc.RememberCondition(); //PassiveEffect反映の条件保持
        }
        //itemのpassiveEffect反映
        foreach(BattlePassiveEffect effect in this.havePassiveItems)
        {
            this.passiveEffectFuncs.EffectFunc(effect, this.allyList); //itemは見方全体にのみ対象
        }

        //skillのpassiveEffect反映
        foreach(BattleCharacter bc in this.allyList)
        {
            for(int i = 0; i < bc.PC.HaveBattlePassiveSkillID.Length; i++)
            {
                if (bc.PC.UseAbleBattlePassiveSkillLV[i] > bc.PC.LV) break; //解放されていないスキルは飛ばす
                BattlePassiveSkill passiveSkill = this.passiveEffectFuncs.GetBattlePassiveSkill(bc.PC.HaveBattlePassiveSkillID[i]);
                if (passiveSkill.EffectIsOnlyFirst() && this.currentSituation != E_BattleSituation.SetParameterBeforeStartBattle) break; //戦闘開始時のみ適用スキル
                switch (passiveSkill.TargetType)
                {
                    case E_TargetType.AllAlly:
                        this.passiveEffectFuncs.EffectFunc(passiveSkill, this.allyList);
                        break;
                    case E_TargetType.Self:
                        this.passiveEffectFuncs.EffectFunc(passiveSkill, new List<BattleCharacter>() { bc });
                        break;
                    case E_TargetType _:
                        Debug.Log("error");
                        break;
                }
            }
        }
        foreach(BattleCharacter bc in this.allyList)
        {
            bc.InitCondition(); //一応条件保持パラメータを初期化
        }
    }
    
    /// <summary>
    /// HPなどの条件変化に対応するための、PassiveEffectの初期化と再代入
    /// </summary>
    public void ReSetPassiveEffect()
    {
        foreach(BattleCharacter bc in this.allyList)
        {
            bc.InitPassiveParameter();
        }
        SetPassiveEffect();
        foreach(BattleCharacter bc in this.allyList)
        {
            if (bc.Hp > bc.MaxHp) bc.Hp = bc.MaxHp;
        }
    }

    /// <summary>
    /// 「もどる」ボタン押されたときの処理
    /// </summary>
    public void PressedBackButton()
    {
        switch (this.currentSituation)
        {
            case E_BattleSituation.PlayerSelectActiveSkill:
            case E_BattleSituation.PlayerSelectActiveItem:
                this.currentSituation = E_BattleSituation.PlayerSelectAction;
                ShowPlayerActionSelect();
                break;
            case E_BattleSituation.PlayerSelectSkillTarget:
                this.waitingEffect = null;
                if (inputTargetWaiting) //味方1人対象選択中
                {
                    this.uiManager.SetActiveAllyTargetButtons(false);
                    this.inputTargetWaiting = false;
                }
                this.currentSituation = E_BattleSituation.PlayerSelectActiveSkill;
                ShowActiveSkillSelect(charaList[nextActionIndex]);
                break;
            case E_BattleSituation.PlayerSelectItemTarget:
                this.waitingEffect = null;
                if (inputTargetWaiting) //味方1人対象選択中
                {
                    this.uiManager.SetActiveAllyTargetButtons(false);
                    this.inputTargetWaiting = false;
                }
                this.currentSituation = E_BattleSituation.PlayerSelectActiveItem;
                ShowActiveItemSelect();
                break;
            
        }
    }
}
