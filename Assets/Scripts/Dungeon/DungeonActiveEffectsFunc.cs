using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DungeonActiveEffectの処理を行うクラス(DungoenActiveEffect関数をとりあえず用意)→DungeonManagerクラスに全て入れるべきかも
/// </summary>
public class DungeonActiveEffectsFunc : MonoBehaviour
{
    /// <summary>
    /// ID管理用全dungeonActiveSkill(IDの順番に入れてね)要素番号ではなく、IDの値検索で良い気もする
    /// </summary>
    [SerializeField] private List<DungeonActiveSkill> dungeonActiveSkills = new List<DungeonActiveSkill>();

    /// <summary>
    /// ID管理用全dungeonActiveItem
    /// </summary>
    [SerializeField] private List<DungeonActiveItem> dungeonActiveItems = new List<DungeonActiveItem>();

    public DungeonActiveItem GetItem(E_DungeonActiveItem itemId) 
    {
        return this.dungeonActiveItems[(int)itemId];
    }
}
