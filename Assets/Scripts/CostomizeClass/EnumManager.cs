using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumManager : MonoBehaviour
{
    public static E_DungeonActiveEffectTargetType GetDungeonActiveEffectTargetType(E_TargetTypeToAlly targetTypeToAlly)
    {
        if (targetTypeToAlly == E_TargetTypeToAlly.OneAlly) return E_DungeonActiveEffectTargetType.OneAlly;
        else if (targetTypeToAlly == E_TargetTypeToAlly.AllAlly) return E_DungeonActiveEffectTargetType.AllAlly;
        else return E_DungeonActiveEffectTargetType.SelfAlly;
    }

    public static E_DungeonActiveEffectTargetType GetDungeonActiveEffectTargetType(E_TargetTypeToDungeonSquare targetTypeToDungeonSquare)
    {
        if (targetTypeToDungeonSquare == E_TargetTypeToDungeonSquare.OneDungeonSquare) return E_DungeonActiveEffectTargetType.OneDungeonSquare;
        else return E_DungeonActiveEffectTargetType.AllDungeonSquare;
    }
}
