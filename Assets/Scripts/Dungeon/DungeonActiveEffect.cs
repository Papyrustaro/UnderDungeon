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
public abstract class DungeonActiveEffect : MonoBehaviour
{
    [SerializeField] private E_DungeonActiveEffectType effectType;
    [SerializeField] private string description;
    public E_DungeonActiveEffectType EffectType => this.effectType;

    public abstract string EffectName { get; }

    public abstract void EffectFunc(DungeonManager dm);

    public string Description
    {
        get
        {
            if (this.description != "") return this.description;
            else return "効果の説明";
        }
    }
}