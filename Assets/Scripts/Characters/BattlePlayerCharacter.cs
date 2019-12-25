using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 戦闘中のPlayerクラス */
public class BattlePlayerCharacter : BattleCharacter
{
    private PlayerCharacter pc;
    private void Awake()
    {
        pc = GetComponent<PlayerCharacter>();
    }
    public PlayerCharacter PC => this.pc;
}
