using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMethod : MonoBehaviour
{
    /// <summary>
    /// searchListの中から最大Hpのx割以下のHpのキャラを返す
    /// </summary>
    /// <param name="searchList">探すキャラリスト</param>
    /// <param name="rateOfMaxHp">最大Hpに対するHpの割合</param>
    /// <returns>求めたキャラリスト</returns>
    public static List<BattleCharacter> GetLowerHpChara(List<BattleCharacter> searchList, double rateOfMaxHp)
    {
        List<BattleCharacter> list = new List<BattleCharacter>();
        foreach(BattleCharacter bc in searchList)
        {
            if ((rateOfMaxHp >= bc.Hp / bc.MaxHp) && bc.IsAlive) list.Add(bc);
        }
        return list;
    }
    /// <summary>
    /// searchListの中から最大Hpのx割以上のHpのキャラを返す
    /// </summary>
    /// <param name="searchList">探すキャラリスト</param>
    /// <param name="rateOfMaxHp">最大Hpに対するHpの割合</param>
    /// <returns>求めたキャラリスト</returns>
    public static List<BattleCharacter> GetHigherHpChara(List<BattleCharacter> searchList, double rateOfMaxHp)
    {
        List<BattleCharacter> list = new List<BattleCharacter>();
        foreach (BattleCharacter bc in searchList)
        {
            if (rateOfMaxHp <= bc.Hp / bc.MaxHp) list.Add(bc);
        }
        return list;
    }

    
}
