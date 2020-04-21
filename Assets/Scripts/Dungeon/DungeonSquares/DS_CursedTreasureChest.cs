using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_CursedTreasureChest : DungeonSquare
{
    //出現しうるアイテム(IDでもいいけど)
    [SerializeField] private List<DungeonActiveItem> mayAppearDungeonActiveItems = new List<DungeonActiveItem>();
    [SerializeField] private List<DungeonPassiveItem> mayAppearDungeonPassiveItems = new List<DungeonPassiveItem>();
    [SerializeField] private List<BattleActiveItem> mayAppearBattleActiveItems = new List<BattleActiveItem>();
    [SerializeField] private List<BattlePassiveItem> mayAppearBattlePassiveItems = new List<BattlePassiveItem>();

    [SerializeField] private List<BattleCharacter> mayAppearEnemy = new List<BattleCharacter>();
    public override E_DungeonSquareType SquareType => E_DungeonSquareType.呪いの宝箱;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("呪いの宝箱イベント発生");
        OpenCursedTreasureChest(dm);
    }

    /// <summary>
    /// 宝箱を開ける処理。(全てのリストが1以上要素を持つ前提)
    /// </summary>
    private void OpenCursedTreasureChest(DungeonManager dm)
    {
        if(UnityEngine.Random.Range(0f, 1f) > dm.SuccessRateOfUnlockTreasureChest)
        {
            Debug.Log("宝箱の解除に失敗した"); /* あるいは鍵がかかっていないものも用意or成功率を宝箱ごとにわける */
            dm.MoveScene(E_DungeonScene.SelectAction);
            return;
        }

        switch(UnityEngine.Random.Range(0, 9))
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
                dm.HaveDungeonPassiveItems.Add(dpItem);
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
