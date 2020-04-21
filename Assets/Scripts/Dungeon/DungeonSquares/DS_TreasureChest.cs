using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_TreasureChest : DungeonSquare
{
    //出現しうるアイテム(IDでもいいけど)
    [SerializeField] private List<DungeonActiveItem> mayAppearDungeonActiveItems = new List<DungeonActiveItem>();
    [SerializeField] private List<DungeonPassiveItem> mayAppearDungeonPassiveItems = new List<DungeonPassiveItem>();
    [SerializeField] private List<BattleActiveItem> mayAppearBattleActiveItems = new List<BattleActiveItem>();
    [SerializeField] private List<BattlePassiveItem> mayAppearBattlePassiveItems = new List<BattlePassiveItem>();

    [SerializeField] private List<BattleCharacter> mayAppearEnemy = new List<BattleCharacter>();

    public override E_DungeonSquareType SquareType => E_DungeonSquareType.宝箱;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("宝箱イベント発生");
        OpenChest(dm);
    }

    /// <summary>
    /// 宝箱を開ける処理。ランダムでアイテムかモンスター戦闘。(アイテム獲得処理は参照渡しのため修正が必要かも)
    /// </summary>
    /// <param name="dm">DungeonManager</param>
    private void OpenChest(DungeonManager dm)
    {
        switch (UnityEngine.Random.Range(0, 9))
        {
            case 0:
            case 1:
                DungeonActiveItem daItem = this.mayAppearDungeonActiveItems[UnityEngine.Random.Range(0, this.mayAppearDungeonActiveItems.Count)];
                Debug.Log(daItem.EffectName + "を入手した");
                dm.HaveDungeonActiveItems.Add(daItem);
                break;
            case 2:
            case 3:
                DungeonPassiveItem dpItem = this.mayAppearDungeonPassiveItems[UnityEngine.Random.Range(0, this.mayAppearDungeonPassiveItems.Count)];
                Debug.Log(dpItem.EffectName + "を入手した");
                dm.AddDungeonPassiveItem(dpItem);
                break;
            case 4:
            case 5:
                BattleActiveItem baItem = this.mayAppearBattleActiveItems[UnityEngine.Random.Range(0, this.mayAppearBattleActiveItems.Count)];
                Debug.Log(baItem.EffectName + "を入手した");
                dm.HaveBattleActiveItems.Add(baItem);
                break;
            case 6:
            case 7:
                BattlePassiveItem bpItem = this.mayAppearBattlePassiveItems[UnityEngine.Random.Range(0, this.mayAppearBattlePassiveItems.Count)];
                Debug.Log(bpItem.EffectName + "を入手した");
                dm.HaveBattlePassiveItems.Add(bpItem);
                break;
            case 8:
                BattleCharacter enemy = Instantiate(this.mayAppearEnemy[UnityEngine.Random.Range(0, this.mayAppearEnemy.Count)], dm.EnemysRootObject.transform);
                Debug.Log(enemy.CharaClass.CharaName + "が出現した");
                dm.Enemys = new List<BattleCharacter>() { enemy };
                dm.MoveBattleScene();
                return;
        }
        dm.MoveScene(E_DungeonScene.SelectAction);
    }
}
