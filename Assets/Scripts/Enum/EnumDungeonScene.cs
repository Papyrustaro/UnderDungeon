using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_DungeonScene
{
    WaitSetFirstData, //ダンジョン潜入時の最初のデータセット
    SelectAction, //行動選択(サイコロを振る、アイテム使用、スキル使用、マップを見る)
    SelectDAS, //使用するDAS選択
    SelectDASTarget, //DASのターゲット選択
    SelectDAI, //使用するDAI選択
    SelectDAITarget, //DAIのターゲット選択
    ViewMap, //マップ全体を見る
    ViewAllyStatus, //味方の状態確認
    SelectMoveDirection, //分岐路にて、移動する方向選択
    WaitDungeonSquareEvent, //マスイベント終了待機(本来は、それぞれのイベントでも状態保存が必要)
}