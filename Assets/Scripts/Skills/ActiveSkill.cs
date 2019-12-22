using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* 戦闘時に選択して使用できるスキル */
public class ActiveSkill : MonoBehaviour
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string skillName; //スキル名
    [SerializeField]
    private string description; //スキルの説明
    [SerializeField]
    private int needTurn; //必要なターン
    [SerializeField]
    private E_Element element;
    [SerializeField]
    private bool needTarget; //プレイヤーにターゲットを訊くかどうか(特定の味方対象など)

    private Action skillFunc; //こっちからはidだけで管理するかも

    public int ID => id;
    public string SkillName => skillName;
    public string Description => description;
    public int NeedTurn => needTurn;
    public E_Element Element => element;
    public bool NeedTarget => needTarget;
    public void SkillFunc()
    {
        this.skillFunc();
    }

}
