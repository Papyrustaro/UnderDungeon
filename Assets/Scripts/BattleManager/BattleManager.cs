using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacter[] player;
    [SerializeField]
    private EnemyCharacter[] enemy;

    private void Awake()
    {
        try
        {
            foreach (PlayerCharacter pc in player)
            {
                Debug.Log(pc.Name + ": " + pc.Description);
            }
            foreach (EnemyCharacter ec in enemy)
            {
                Debug.Log(ec.Name + ": " + ec.Description);
            }
        }catch(Exception e)
        {
            Debug.Log(e);
        }
    }
}
