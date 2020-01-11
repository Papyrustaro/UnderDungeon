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

public enum E_SkillType
{
    攻撃,
    HP回復,
    被ダメージ増減バフ,
    与ダメージ増減バフ,
    HPバフ,
    ATKバフ,
    SPDバフ, //デバフも倍率と対象変えるだけ
    スキルポイント増減,
    攻撃集中,
    属性変化,
    復活付与,
    無敵付与,
    カウンター,
    通常攻撃全体攻撃化,
    通常攻撃被ダメージ増減,
    通常攻撃与ダメージ増減,
    通常攻撃回数追加,
    攻撃集中被ダメ減,
    固定ダメージ,
    その他,
}

    /* あるいはBattleActiveSkillクラスに、技のtypeを持たせて、switch分岐なくすもありか */
    /* ・atk(sp_atk) * n倍の攻撃  
・ステータスバフ  
・受けるダメージ軽減  
・与えるダメージn倍  
・相手のSp減らす(自分増やす)  
・HP回復  
・挑発(敵の攻撃を寄せる)  
・一度だけ復活付与  
・パーティの属性を変化させる  
・毎ターンHP回復  
・反射  
・カウンター  
・無敵  
・バフを消す効果  
・素早さ遅い順に行動  
・味方にダメージを与えるがバフも与える  
・敵が攻撃系スキルしか発動できなくする  */

/*スキルに必要となるパラメータ
 効果ターン
 倍率or威力
 特殊なタイプはその都度switchで処理すればいい
     
     
     */