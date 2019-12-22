using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPassiveSkill : MonoBehaviour
{
    [SerializeField]
    private E_DungeonPassiveSkill id;
    [SerializeField]
    private string description;

    public E_DungeonPassiveSkill Id => id;
    public string SkillName => id.ToString();
    public string Description => description;
}
