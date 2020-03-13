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

    private DungeonManager dm;
    E_DungeonActiveEffectType effectType = E_DungeonActiveEffectType.サイコロ変化;

    /// <summary>
    /// 味方全員のBsp回復
    /// </summary>
    /// <param name="addValue">回復量</param>
    public void AddBspToAllAlly(int addValue)
    {
        foreach(BattleCharacter bc in this.dm.Allys)
        {
            bc.AddBsp(addValue);
        }
    }

    /// <summary>
    /// 味方全員のDsp回復
    /// </summary>
    /// <param name="addValue">回復量</param>
    public void AddDspToAllAlly(int addValue)
    {
        dm.ChangeAllDsp(addValue);
    }

    public void RecoverHpByRateAllAlly(double recoverRate)
    {
        foreach(BattleCharacter bc in this.dm.Allys)
        {
            bc.RecoverHpByRate(recoverRate);
        }
    }

    public void RecoverHpAllAlly(int recoverValue)
    {
        foreach(BattleCharacter bc in this.dm.Allys)
        {
            bc.RecoverHp(recoverValue);
        }
    }

    public void GetItem(E_DungeonActiveItem itemId) 
    {
        dm.HaveDungeonActiveItems.Add(this.dungeonActiveItems[(int)itemId]);
    }
}
