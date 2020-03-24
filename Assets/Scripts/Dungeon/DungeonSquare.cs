using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョンの1マス1マスの基底クラス
/// </summary>
public abstract class DungeonSquare : MonoBehaviour
{
    /// <summary>
    /// マスが上端から何行目か(0からスタート)
    /// </summary>
    public int RowPosition { get; set; } = 1;

    /// <summary>
    /// マスが左端から何列目か(0からスタート)
    /// </summary>
    public int ColumnPosition { get; set; } = 2;

    /// <summary>
    /// マスの種類(店、戦闘など)(抽象)
    /// </summary>
    public abstract E_DungeonSquareType SquareType { get; }

    /// <summary>
    /// マスに止まったときのイベント(抽象)
    /// </summary>
    public abstract void SquareEvent(DungeonManager dm);
}


/// <summary>
/// ダンジョンのマスの種類(店、戦闘など)
/// </summary>
public enum E_DungeonSquareType
{
    通常戦闘,
    店,
    闇商人,
    宝箱,
    罠,
    回復の泉, //どれかひとつのパラメータを全回復など
    ボス戦,
    階段,
    ランダム,
    なにもなし,
    壁,
    祈祷, //祈祷に成功すれば様々なメリット、失敗すればデメリット。両方あることも
    呪いの宝箱, //デメリット+レアアイテムなど獲得
    Error,
}
