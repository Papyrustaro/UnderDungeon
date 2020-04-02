using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum E_DungeonActiveEffectType
{
    マスタイプ変化,
    移動量増加,
    移動量倍増,
    サイコロ変化, //移動量固定も担っている(?)
    Hp回復,
    Dsp回復,
    Bsp回復,
    アイテム獲得,
    マップ全体可視化, //全マスのイベントタイプがわかる(?)
    満腹度回復,
}

public enum E_TargetTypeToAlly
{
    OneAlly,
    AllAlly,
    SelfAlly,
}

public enum E_TargetTypeToDungeonSquare
{
    OneDungeonSquare,
    AllDungeonSquare,
}

public enum E_DungeonActiveEffectTargetType
{
    OneAlly,
    AllAlly,
    SelfAlly,
    OneDungeonSquare,
    AllDungeonSquare,
    Other,
    Error
}
public abstract class DungeonActiveEffect : MonoBehaviour
{
    [SerializeField] private string description;
    public abstract E_DungeonActiveEffectType EffectType { get; }

    public abstract string EffectName { get; }

    public abstract void EffectFunc(DungeonManager dm);

    public string Description
    {
        get
        {
            if (this.description != "") return this.description;
            else return "error";
        }
    }

    public virtual E_DungeonActiveEffectTargetType DungeonActiveEffectTargetType => E_DungeonActiveEffectTargetType.Other;
    public virtual List<E_DungeonSquareType> TargetDungeonSquareTypes => throw new System.Exception();
    public virtual int EffectRange => throw new System.Exception();
}