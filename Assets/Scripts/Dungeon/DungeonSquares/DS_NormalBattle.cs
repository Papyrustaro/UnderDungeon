using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_NormalBattle : DungeonSquare
{
    /// <summary>
    /// 出現しうる敵リスト
    /// </summary>
    [SerializeField] private List<BattleCharacter> mayApeearEnemys = new List<BattleCharacter>();

    public override E_DungeonSquareType SquareType => E_DungeonSquareType.通常戦闘;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("通常戦闘イベント発生");
        ChooseEnemys(dm);
    }

    /// <summary>
    /// 出現する敵選択(2~4体)
    /// </summary>
    private void ChooseEnemys(DungeonManager dm)
    {
        List<BattleCharacter> enemys = new List<BattleCharacter>();
        int countOfEnemyKind = this.mayApeearEnemys.Count;
        for(int i = 0; i < UnityEngine.Random.Range(2, 5); i++)
        {
            enemys.Add(this.mayApeearEnemys[UnityEngine.Random.Range(0, countOfEnemyKind)]);
        }
        dm.Enemys = enemys;

        foreach(BattleCharacter enemy in enemys)
        {
            Debug.Log("敵: " + enemy.CharaClass.CharaName);
        }
        dm.MoveBattleScene();
    }
}
