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

    public int Id => id;
    public E_PassiveSkillType PassiveSkillType => passiveSkillType;
    public string SkillName => skillName;
    public string Description => description;
}
