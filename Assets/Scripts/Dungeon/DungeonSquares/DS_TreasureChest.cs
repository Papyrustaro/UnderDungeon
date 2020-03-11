using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_TreasureChest : DungeonSquare
{
    //出現しうるアイテム(IDでもいいけど)
    [SerializeField] private List<DungeonActiveItem> mayApeearDungeonActiveItems = new List<DungeonActiveItem>();
    [SerializeField] private List<DungeonPassiveItem> mayApeearDungeonPassiveItems = new List<DungeonPassiveItem>();
    [SerializeField] private List<BattleActiveItem> mayApeearBattleActiveItems = new List<BattleActiveItem>();
    [SerializeField] private List<BattlePassiveItem> mayApeearBattlePassiveItems = new List<BattlePassiveItem>();

    [SerializeField] private EnemyCharacter mayApeearEnemy;

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
        switch(UnityEngine.Random.Range(0, 9))
        {
            case 0:
            case 1:
                dm.HaveDungeonActiveItems.Add(this.mayApeearDungeonActiveItems[UnityEngine.Random.Range(0, this.mayApeearDungeonActiveItems.Count)]);
                break;
            case 2:
            case 3:
                dm.HaveDungeonPassiveItems.Add(this.mayApeearDungeonPassiveItems[UnityEngine.Random.Range(0, this.mayApeearDungeonPassiveItems.Count)]);
                break;
            case 4:
            case 5:
                dm.HaveBattleActiveItems.Add(this.mayApeearBattleActiveItems[UnityEngine.Random.Range(0, this.mayApeearBattleActiveItems.Count)]);
                break;
            case 6:
            case 7:
                dm.HaveBattlePassiveItems.Add(this.mayApeearBattlePassiveItems[UnityEngine.Random.Range(0, this.mayApeearBattlePassiveItems.Count)]);
                break;
            case 8:
                Debug.Log(this.mayApeearEnemy.CharaName + "があらわれた");
                break;
        }
    }
}
