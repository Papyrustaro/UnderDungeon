using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_PassiveSkillType //どこで発動する効果か
{
    Battle, //戦闘中
    Dungeon, //ダンジョン中(店の商品+1など)
}
public class PassiveSkill : MonoBehaviour
{
    [SerializeField]
    private int id;
    [SerializeField]
    private E_PassiveSkillType passiveSkillType;
    [SerializeField]
    private string skillName;
    [SerializeField]
    private string description;

    public PassiveSkill(int id, E_PassiveSkillType passiveSkillType, string skillName, string description)
    {
        this.id = id; this.passiveSkillType = passiveSkillType;  this.skillName = skillName; this.description = description;
    }

    public int Id => id;
    public E_PassiveSkillType PassiveSkillType => passiveSkillType;
    public string SkillName => skillName;
    public string Description => description;
}
