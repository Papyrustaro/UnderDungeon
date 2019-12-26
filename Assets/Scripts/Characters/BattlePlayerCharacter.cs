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
        AddSpdRate(20, 3);
        AddSpdRate(40, 2);
        AddSpdRate(60, 1);
    }
    public PlayerCharacter PC => this.pc;
}
