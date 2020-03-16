using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAS_ChangeDungeonSquareType : DungeonActiveSkill
{
    [SerializeField] private List<E_DungeonSquareType> targetDungeonSquareTypes = new List<E_DungeonSquareType>();
    [SerializeField] private E_DungeonSquareType afterChangeDungeonSquareType;
    [SerializeField] private int effectRange; //マスを変えられる範囲(自身から何マス離れているか)
    public override E_DungeonActiveEffectType EffectType => E_DungeonActiveEffectType.マスタイプ変化;

    public override void EffectFunc(DungeonManager dm)
    {
        dm.ChangeDungeonSquareType(targetDungeonSquareTypes, afterChangeDungeonSquareType, effectRange);
    }
}
