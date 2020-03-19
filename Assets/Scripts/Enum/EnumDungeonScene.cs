using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョンのメイン状態保存(個々のマスイベントは除く)
/// </summary>
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
    MovingDungeonSquare, //決まった移動方向に移動中
    SelectMoveDirection, //分岐路にて、移動する方向選択
    WaitDungeonSquareEvent, //マスイベント終了待機(本来は、それぞれのイベントでも状態保存が必要)
}

/// <summary>
/// ダンジョンのマス移動前のプレイヤーの行動選択
/// </summary>
public enum E_DungeonPlayerSelect
{
    RollDice, //サイコロを投げて移動開始
    UseDungeonActiveItem, //DAI使用
    InvokeDungeonActiveSkill, //DAS発動
    VerificateMap, //マップ確認
    VerificateAlly, //パーティ確認
}