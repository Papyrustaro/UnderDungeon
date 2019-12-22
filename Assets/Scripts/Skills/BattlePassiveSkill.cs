using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePassiveSkill : MonoBehaviour
{
    [SerializeField]
    private E_BattlePassiveSkill id;
    [SerializeField]
    private string description;

    public E_BattlePassiveSkill ID => id;
    public string SkillName => id.ToString();
    public string Description => description;
}
