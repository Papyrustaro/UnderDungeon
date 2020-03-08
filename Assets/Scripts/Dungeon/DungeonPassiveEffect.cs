using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum E_DungeonPassiveEffectType
{
    敵ドロップ率増加,
    宝箱解除成功率増加,
    店商品増加,
    店レア商品出現確率増加,
    特定イベント出現率増加,
    獲得経験値増加,
    獲得ゴールド増加,
    レア敵出現率増加,
    罠回避率増加,
    最大満腹度増加,
    マス可視範囲増加,
    開始時Dsp増加,

}
public abstract class DungeonPassiveEffect : MonoBehaviour
{
    [SerializeField] private E_DungeonPassiveEffectType effectType;
    [SerializeField] private string description;

    public E_DungeonPassiveEffectType EffectType => this.effectType;

    public abstract string EffectName { get; }

    public string Description
    {
        get
        {
            if (this.description != "") return this.description;
            else return "効果の説明";
        }
    }
    
}
