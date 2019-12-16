using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SkillType //物理スキルor特殊スキルorその他
{
    Physics,
    Special,
    Other,
}

/* 戦闘時に選択して使用できるスキル */
public class BattleSkill
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string name; //スキル名
    [SerializeField]
    private string description; //スキルの説明
    [SerializeField]
    private int needSp; //消費sp
    [SerializeField]
    private int element;
    [SerializeField]
    private int physicsOrSpecials; //物理スキルor特殊スキルorその他
    [SerializeField]
    private bool needTarget; //プレイヤーにターゲットを訊くかどうか(特定の味方対象など)

    private Action skillFunc; //こっちからはidだけで管理するかも

    public BattleSkill(int id, string name, string description, int needSp, int element, int physicsOrSpecials, bool needTarget, Action skillFunc)
    {
        this.id = id; this.name = name; this.description = description; this.needSp = needSp; this.element = element;
        this.physicsOrSpecials = physicsOrSpecials; this.needTarget = needTarget;
        this.skillFunc = skillFunc;
    }

    public int ID => id;
    public string Name => name;
    public string Description => description;
    public int NeedSp => needSp;
    public int Element => element;
    public int PhysicsOrSpecials => physicsOrSpecials;
    public bool NeedTarget => needTarget;
    public void SkillFunc()
    {
        this.skillFunc();
    }

}
