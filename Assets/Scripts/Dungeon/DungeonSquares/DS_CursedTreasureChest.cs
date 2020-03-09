using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_CursedTreasureChest : DungeonSquare
{
    //出現しうるアイテム(IDでもいいけど)
    [SerializeField] private List<DungeonActiveItem> mayApeearDungeonActiveItems = new List<DungeonActiveItem>();
    [SerializeField] private List<DungeonPassiveItem> mayApeearDungeonPassiveItems = new List<DungeonPassiveItem>();
    [SerializeField] private List<BattleActiveItem> mayApeearBattleActiveItems = new List<BattleActiveItem>();
    [SerializeField] private List<BattlePassiveItem> mayApeearBattlePassiveItems = new List<BattlePassiveItem>();

    [SerializeField] private List<EnemyCharacter> mayApeearEnemy = new List<EnemyCharacter>();
    public override E_DungeonSquareType SquareType => E_DungeonSquareType.呪いの宝箱;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("呪いの宝箱イベント発生");
        OpenCursedTreasureChest();
    }

    /// <summary>
    /// 宝箱を開ける処理。(全てのリストが1以上要素を持つ前提)
    /// </summary>
    private void OpenCursedTreasureChest()
    {
        Debug.Log("箱から出てきたのは");
        switch(UnityEngine.Random.Range(0, 9))
        {
            case 0:
            case 1:
                Debug.Log(this.mayApeearDungeonActiveItems[UnityEngine.Random.Range(0, this.mayApeearDungeonActiveItems.Count)].EffectName);
                break;
            case 2:
            case 3:
                Debug.Log(this.mayApeearDungeonPassiveItems[UnityEngine.Random.Range(0, this.mayApeearDungeonPassiveItems.Count)].EffectName);
                break;
            case 4:
            case 5:
                Debug.Log(this.mayApeearBattleActiveItems[UnityEngine.Random.Range(0, this.mayApeearBattleActiveItems.Count)].EffectName);
                break;
            case 6:
            case 7:
                Debug.Log(this.mayApeearBattlePassiveItems[UnityEngine.Random.Range(0, this.mayApeearBattlePassiveItems.Count)].EffectName);
                break;
            case 8:
                Debug.Log("モンスター: " + this.mayApeearEnemy[UnityEngine.Random.Range(0, this.mayApeearEnemy.Count)].CharaName);
                break;
        }
    }
}
