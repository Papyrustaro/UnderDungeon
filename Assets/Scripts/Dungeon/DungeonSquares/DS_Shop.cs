using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_Shop : DungeonSquare
{
    //販売しうるアイテム(IDでもいいけど)
    [SerializeField] private List<DungeonActiveItem> mayApeearDungeonActiveItems = new List<DungeonActiveItem>();
    [SerializeField] private List<DungeonPassiveItem> mayApeearDungeonPassiveItems = new List<DungeonPassiveItem>();
    [SerializeField] private List<BattleActiveItem> mayApeearBattleActiveItems = new List<BattleActiveItem>();
    [SerializeField] private List<BattlePassiveItem> mayApeearBattlePassiveItems = new List<BattlePassiveItem>();

    public override E_DungeonSquareType SquareType => E_DungeonSquareType.店;

    /// <summary>
    /// プレイヤーが現在選択しているアイテムindex
    /// </summary>
    private int selectItemIndex = 0;

    /// <summary>
    /// すでにその商品を購入したかどうか
    /// </summary>
    private bool[] isBought = new bool[4];

    private E_ShopScene currentScene = E_ShopScene.FirstSet;
    //private bool needAnnounce = false;

    private DungeonActiveItem apeearDungeonActiveItem;
    private DungeonPassiveItem apeearDungeonPassiveItem;
    private BattleActiveItem apeearBattleActiveItem;
    private BattlePassiveItem apeearBattlePassiveItem;

    public override void SquareEvent(DungeonManager dm)
    {
        switch (this.currentScene)
        {
            case E_ShopScene.FirstSet:
                Debug.Log("店イベント発生");
                InitSet();
                ChooseSoldItems();
                dm.NeedAnnounce = true;
                this.currentScene = E_ShopScene.SelectItem;
                break;
            case E_ShopScene.SelectItem:
                if (dm.NeedAnnounce)
                {
                    ShowSoldItem(dm);
                }
                InputSelectItem(dm);
                break;
            case E_ShopScene.BuyOrNot:
                if (dm.NeedAnnounce) ShowBuyOrNot(dm);
                InputBuyOrNot(dm);
                break;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("店を出た");
            InitSet();
            dm.MoveScene(E_DungeonScene.SelectAction);
        }
    }

    /// <summary>
    /// 販売している商品を表示
    /// </summary>
    private void ShowSoldItem(DungeonManager dm)
    {
        string s = "購入する商品を選択してください\n";
        if (!isBought[0]) {
            if(selectItemIndex == 0) s += this.apeearDungeonActiveItem.EffectName + "(" + this.apeearDungeonActiveItem.BuyPrice + "G) ◀\n";
            else s += this.apeearDungeonActiveItem.EffectName + "(" + this.apeearDungeonActiveItem.BuyPrice + "G)\n";
        }
        else s += "(購入済)\n";

        if (!isBought[1])
        {
            if (selectItemIndex == 1) s += this.apeearDungeonPassiveItem.EffectName + "(" + this.apeearDungeonPassiveItem.BuyPrice + "G) ◀\n";
            else s += this.apeearDungeonPassiveItem.EffectName + "(" + this.apeearDungeonPassiveItem.BuyPrice + "G)\n";
        }
        else s += "(購入済)\n";

        if (!isBought[2]) { 
            if(selectItemIndex == 2) s += this.apeearBattleActiveItem.EffectName + "(" + this.apeearBattleActiveItem.BuyPrice + "G) ◀\n";
            else s += this.apeearBattleActiveItem.EffectName + "(" + this.apeearBattleActiveItem.BuyPrice + "G)\n";
        }
        else s += "(購入済)\n";

        if (!isBought[3]) { 
            if(selectItemIndex == 3) s += this.apeearBattlePassiveItem.EffectName + "(" + this.apeearBattlePassiveItem.BuyPrice + "G) ◀\n";
            else s += this.apeearBattlePassiveItem.EffectName + "(" + this.apeearBattlePassiveItem.BuyPrice + "G)\n";
        }
        else s += "(購入済)\n";

        dm.AnnounceByText(s);
    }

    /// <summary>
    /// プレイヤーの入力に応じて商品選択
    /// </summary>
    /// <param name="dm">DungeonManager</param>
    private void InputSelectItem(DungeonManager dm)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            int count = 0;
            for (int i = this.selectItemIndex - 1; i != selectItemIndex; i--, count++)
            {
                if(i < 0)
                {
                    i = this.isBought.Length - 1;
                }
                if (!this.isBought[i])
                {
                    this.selectItemIndex = i;
                    break;
                }
                if (count > 10) throw new System.Exception();
            }
            dm.NeedAnnounce = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            int count = 0;
            for(int i = this.selectItemIndex + 1; i != this.selectItemIndex; i++, count++)
            {
                if(i > this.isBought.Length - 1)
                {
                    i = 0;
                }
                if (!this.isBought[i])
                {
                    this.selectItemIndex = i;
                    break;
                }
                if (count > 10) throw new System.Exception();
            }
            dm.NeedAnnounce = true;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(selectItemIndex == 0)
            {
                if(this.apeearDungeonActiveItem.BuyPrice > dm.HaveGold)
                {
                    dm.AnnounceByText("所持Gが足りません");
                    dm.NeedAnnounce = true;
                    return;
                }
            }else if(selectItemIndex == 1)
            {
                if(this.apeearDungeonPassiveItem.BuyPrice > dm.HaveGold)
                {
                    dm.AnnounceByText("所持Gが足りません");
                    dm.NeedAnnounce = true;
                    return;
                }
            }else if(selectItemIndex == 2)
            {
                if(this.apeearBattleActiveItem.BuyPrice > dm.HaveGold)
                {
                    dm.AnnounceByText("所持Gが足りません");
                    dm.NeedAnnounce = true;
                    return;
                }
            }
            else
            {
                if(this.apeearBattlePassiveItem.BuyPrice > dm.HaveGold)
                {
                    dm.AnnounceByText("所持Gが足りません");
                    dm.NeedAnnounce = true;
                    return;
                }
            }
            dm.NeedAnnounce = true;
            this.currentScene = E_ShopScene.BuyOrNot;
        }
    }

    private void ShowBuyOrNot(DungeonManager dm)
    {
        string s = "以下の商品を購入しますか(所持G: " + dm.HaveGold + ")\n";
        if (selectItemIndex == 0) s += this.apeearDungeonActiveItem.EffectName + "(" + this.apeearDungeonActiveItem.BuyPrice + "G)\n";
        else if (selectItemIndex == 1) s += this.apeearDungeonPassiveItem.EffectName + "(" + this.apeearDungeonPassiveItem.BuyPrice + "G)\n";
        else if (selectItemIndex == 2) s += this.apeearBattleActiveItem.EffectName + "(" + this.apeearBattleActiveItem.BuyPrice + "G)\n";
        else s += this.apeearBattlePassiveItem.EffectName + "(" + this.apeearBattlePassiveItem.BuyPrice + "G)\n";
        s += "\nはい: [y] いいえ: [n]";
        dm.AnnounceByText(s);
    }

    private void InputBuyOrNot(DungeonManager dm)
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            //購入処理
            if (selectItemIndex == 0)
            {
                dm.HaveDungeonActiveItems.Add(this.apeearDungeonActiveItem);
                dm.AnnounceByText(this.apeearDungeonActiveItem.EffectName + "を購入しました");
                dm.HaveGold -= this.apeearDungeonActiveItem.BuyPrice;
            }
            else if (selectItemIndex == 1)
            {
                dm.HaveDungeonPassiveItems.Add(this.apeearDungeonPassiveItem);
                dm.AnnounceByText(this.apeearDungeonPassiveItem.EffectName + "を購入しました");
                dm.HaveGold -= this.apeearDungeonPassiveItem.BuyPrice;
            }
            else if (selectItemIndex == 2)
            {
                dm.HaveBattleActiveItems.Add(this.apeearBattleActiveItem);
                dm.AnnounceByText(this.apeearBattleActiveItem.EffectName + "を購入しました");
                dm.HaveGold -= this.apeearBattleActiveItem.BuyPrice;
            }
            else
            {
                dm.HaveBattlePassiveItems.Add(this.apeearBattlePassiveItem);
                dm.AnnounceByText(this.apeearBattlePassiveItem.EffectName + "を購入しました");
                dm.HaveGold -= this.apeearBattlePassiveItem.BuyPrice;
            }
            this.isBought[selectItemIndex] = true;
            SetSelectItemIndex(dm);
            dm.NeedAnnounce = true;
            this.currentScene = E_ShopScene.SelectItem;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            dm.NeedAnnounce = true;
            this.currentScene = E_ShopScene.SelectItem;
        }
    }

    /// <summary>
    /// 販売される商品をランダムで選択(とりあえず1種類1つずつ)
    /// </summary>
    private void ChooseSoldItems()
    {
        this.apeearDungeonActiveItem = this.mayApeearDungeonActiveItems[UnityEngine.Random.Range(0, this.mayApeearDungeonActiveItems.Count)];
        this.apeearDungeonPassiveItem = this.mayApeearDungeonPassiveItems[UnityEngine.Random.Range(0, this.mayApeearDungeonPassiveItems.Count)];
        this.apeearBattleActiveItem = this.mayApeearBattleActiveItems[UnityEngine.Random.Range(0, this.mayApeearBattleActiveItems.Count)];
        this.apeearBattlePassiveItem = this.mayApeearBattlePassiveItems[UnityEngine.Random.Range(0, this.mayApeearBattlePassiveItems.Count)];
    }

    /// <summary>
    /// 店イベント終了時の初期化処理
    /// </summary>
    private void InitSet()
    {
        this.currentScene = E_ShopScene.FirstSet;
        this.selectItemIndex = 0;
        this.isBought = new bool[4] { false, false, false, false };
    }
    private void SetSelectItemIndex(DungeonManager dm)
    {
        this.selectItemIndex = -1;
        for (int i = 0; i < this.isBought.Length; i++)
        {
            if (!this.isBought[i])
            {
                this.selectItemIndex = i;
                break;
            }
        }
        if(this.selectItemIndex == -1)
        {
            Debug.Log("販売できる商品はありません");
            Debug.Log("店を出た");
            //マスイベント終了処理
            dm.MoveScene(E_DungeonScene.SelectAction);
            InitSet();
        }
    }
}

public enum E_ShopScene
{
    FirstSet,
    SelectItem,
    BuyOrNot,
}