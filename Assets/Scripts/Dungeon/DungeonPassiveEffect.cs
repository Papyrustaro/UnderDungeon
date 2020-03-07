using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum E_DungeonPassiveEffectType
{
    敵ドロップ率Up,
    宝箱解除成功率Up,
    店商品増加,
    店レア商品出現確率Up,
}
public class DungeonPassiveEffect : MonoBehaviour
{
    [SerializeField] private E_DungeonPassiveEffectType effectType;

    public E_DungeonPassiveEffectType EffectType => this.effectType;
}
