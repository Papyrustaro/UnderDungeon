using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS_NormalBattle : DungeonSquare
{
    /// <summary>
    /// 出現しうる敵リスト
    /// </summary>
    [SerializeField] private List<EnemyCharacter> mayApeearEnemys = new List<EnemyCharacter>();

    public override E_DungeonSquareType SquareType => E_DungeonSquareType.通常戦闘;

    public override void SquareEvent(DungeonManager dm)
    {
        Debug.Log("通常戦闘イベント発生");
        ChooseEnemys();
    }

    /// <summary>
    /// 出現する敵選択(2~4体)
    /// </summary>
    private void ChooseEnemys()
    {
        List<EnemyCharacter> enemys = new List<EnemyCharacter>();
        int countOfEnemyKind = this.mayApeearEnemys.Count;
        for(int i = 0; i < UnityEngine.Random.Range(2, 5); i++)
        {
            enemys.Add(this.mayApeearEnemys[UnityEngine.Random.Range(0, countOfEnemyKind)]);
        }

        foreach(EnemyCharacter enemy in enemys)
        {
            Debug.Log("敵: " + enemy.CharaName);
        }
    }
}
