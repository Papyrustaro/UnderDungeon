using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumPassiveSkillType //どこで発動する効果か
{
    Battle, //戦闘中
    Dungeon, //ダンジョン中(店の商品+1など)
}
public class PassiveSkill : MonoBehaviour
{
    [SerializeField]
    private int id;
    [SerializeField]
    private EnumPassiveSkillType passiveSkillType;
    [SerializeField]
    private string skillName;
    [SerializeField]
    private string description;

    public PassiveSkill(int id, EnumPassiveSkillType passiveSkillType, string skillName, string description)
    {
        this.id = id; this.passiveSkillType = passiveSkillType;  this.skillName = skillName; this.description = description;
    }

    public int Id => id;
    public EnumPassiveSkillType PassiveSkillType => passiveSkillType;
    public string SkillName => skillName;
    public string Description => description;
}
