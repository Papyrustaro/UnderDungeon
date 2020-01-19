using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 戦闘において、現在どの状態にあるか */
public enum E_BattleSituation
{
    PlayerSelectAction, //どの行動をとるかプレイヤーの入力待ち
    PlayerSelectActiveSkill, //どのスキルを発動するかプレイヤーの入力待ち
    PlayerSelectActiveItem, //どのアイテムを使うかプレイヤーの入力待ち
    WaitFinishAction, //発動開始後、発動のアニメーションなどが終わるの待ち
    PlayerSelectSkillTarget, //スキルの効果対象の入力待ち
    PlayerSelectItemTarget, //itemの効果対象の入力待ち

}

public enum E_PlayerAction
{
    NormalAttack,
    InvokeSkill,
    UseItem,
    Protect,
}