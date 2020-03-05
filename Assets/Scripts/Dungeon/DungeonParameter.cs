using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョンの列挙。ダンジョン名も兼ねている(?)
/// </summary>
public enum E_Dungeon
{
    はじまりの洞窟,
    ダンジョン1,
    ダンジョン2, 
}

/// <summary>
/// ダンジョン毎にInspector上で設定するパラメータ保持クラス(階層数、クリア報酬、ボスなど)
/// </summary>
public class DungeonParameter : MonoBehaviour
{
    [SerializeField] private E_Dungeon dungeonID;
    [SerializeField] private int howManyFloors;
    [SerializeField] private EnemyCharacter bossEnemy;
    [SerializeField] private int goldByClear; //クリアでもらえるゴールド(他ランク経験値など)

    /// <summary>
    /// どのダンジョンか
    /// </summary>
    public E_Dungeon DungeonID => this.dungeonID;

    /// <summary>
    /// 何階層のダンジョンか
    /// </summary>
    public int HowManyFloors => this.howManyFloors;

    /// <summary>
    /// ボスキャラ情報
    /// </summary>
    public EnemyCharacter BossEnemy => this.bossEnemy;
    public int GoldByClear => this.goldByClear;
}
