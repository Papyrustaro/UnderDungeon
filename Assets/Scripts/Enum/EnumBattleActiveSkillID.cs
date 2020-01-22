using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum E_BattleActiveSkill
{
    ファイアA,
    ファイアB,
    //ファイアC,
    アクアA,
    アクアB,
    //アクアC,
    ウィングA,
    ウィングB, 
    //ウィングC,
    ヒールA,
    ヒールB,
    //ヒールC,
    フレイA, //ここから全体効果
    バブルA,
    グランドA,
    ハイヒールA,
    バイキルト,
    HpUpA,
    SpdUpA,
    攻撃集中A,
    属性変化水A,
    復活付与A,
    無敵付与A,
    カウンターA,
    通常攻撃全体化A,
    通常攻撃被ダメージ減A,
    通常攻撃与ダメージ増A,
    通常攻撃回数増A,
    デバッグ,
    その他A,
}

public class EnumBattleActiveSkillID
{
    public static int EnumSize => Enum.GetValues(typeof(E_BattleActiveSkill)).Length;
}

public enum E_TargetType
{
    OneEnemy,
    AllEnemy,
    OneAlly,
    AllAlly,
    Self,
    All,
    RandomEnemy,
    RandomAlly,
    RandomAll,
}