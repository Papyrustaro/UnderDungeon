using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDungeonManager : MonoBehaviour
{
    [SerializeField] private DungeonManager dungeonManager;
    private void Awake()
    {
        this.dungeonManager = StaticDungeonData.dungeonManager;
    }
}
