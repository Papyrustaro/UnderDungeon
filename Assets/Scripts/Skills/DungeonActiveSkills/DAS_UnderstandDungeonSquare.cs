using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAS_UnderstandDungeonSquare : DungeonActiveSkill
{
    public override E_DungeonActiveEffectType EffectType => E_DungeonActiveEffectType.マップ全体可視化;

    public override void EffectFunc(DungeonManager dm)
    {
        //処理
        throw new System.NotImplementedException();
    }
}
