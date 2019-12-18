using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum E_ActiveSkillType //物理スキルor特殊スキルorその他
{
    Physics,
    Special,
    Other,
}

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
    private int needSp; //消費sp
    [SerializeField]
    private E_Element element;
    [SerializeField]
    private E_ActiveSkillType activeSkillType;
    [SerializeField]
    private bool needTarget; //プレイヤーにターゲットを訊くかどうか(特定の味方対象など)

    private Action skillFunc; //こっちからはidだけで管理するかも

    public ActiveSkill(int id, string skillName, string description, int needSp, E_Element element, E_ActiveSkillType activeSkillType, bool needTarget, Action skillFunc)
    {
        this.id = id; this.skillName = skillName; this.description = description; this.needSp = needSp; this.element = element;
        this.activeSkillType = activeSkillType; this.needTarget = needTarget;
        this.skillFunc = skillFunc;
    }

    public int ID => id;
    public string SkillName => skillName;
    public string Description => description;
    public int NeedSp => needSp;
    public E_Element Element => element;
    public E_ActiveSkillType ActiveSkillType => activeSkillType;
    public bool NeedTarget => needTarget;
    public void SkillFunc()
    {
        this.skillFunc();
    }

}
