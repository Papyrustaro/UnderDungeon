﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Element
{
    Fire,
    Aqua,
    Tree,
    FireAqua,
    AquaTree,
    TreeFire,
    FireAquaTree,
}

public class ElementClass
{
    public static bool IsFire(E_Element element)
    {
        return (element == E_Element.Fire || element == E_Element.FireAqua || element == E_Element.TreeFire || element == E_Element.FireAquaTree);
    }
    public static bool IsAqua(E_Element element)
    {
        return (element == E_Element.Aqua || element == E_Element.FireAqua || element == E_Element.AquaTree || element == E_Element.FireAquaTree);
    }
    public static bool IsTree(E_Element element)
    {
        return (element == E_Element.Tree || element == E_Element.AquaTree || element == E_Element.TreeFire || element == E_Element.FireAquaTree);
    }

    /// <summary>
    /// 列挙型に対する各属性の文字列を返す
    /// </summary>
    /// <param name="element">検索する属性</param>
    /// <returns>属性の文字列</returns>
    public static string GetStringElement(E_Element element)
    {
        switch (element)
        {
            case E_Element.Fire: return "火";
            case E_Element.Aqua: return "水";
            case E_Element.Tree: return "木";
            case E_Element.FireAqua: return "火・水";
            case E_Element.AquaTree: return "水・木";
            case E_Element.TreeFire: return "木・火";
            case E_Element.FireAquaTree: return "全";
            case E_Element _: return "エラー";
        }
    }

    /// <summary>
    /// bc.Elementを利用した属性検索
    /// </summary>
    /// <param name="baseList">検索するキャラリスト</param>
    /// <param name="searchElement">検索する属性</param>
    /// <returns>属性にマッチしたキャラのリスト</returns>
    public static List<BattleCharacter> GetListInElement(List<BattleCharacter> baseList, E_Element searchElement)
    {
        List<BattleCharacter> list = new List<BattleCharacter>();
        foreach(BattleCharacter bc in baseList)
        {
            if (ElementClass.IsFire(searchElement) && ElementClass.IsFire(bc.Element)) list.Add(bc);
            else if (ElementClass.IsAqua(searchElement) && ElementClass.IsAqua(bc.Element)) list.Add(bc);
            else if (ElementClass.IsTree(searchElement) && ElementClass.IsTree(bc.Element)) list.Add(bc);
        }
        return list;
    }

    /// <summary>
    /// PassiveEffect用の条件用保持パラメータ(Conditon)を利用した属性サーチ
    /// </summary>
    /// <param name="baseList">検索するキャラのリスト</param>
    /// <param name="searchElement">検索する属性</param>
    /// <returns>検索属性をもつキャラのリスト</returns>
    public static List<BattleCharacter> GetListInElementByCondition(List<BattleCharacter> baseList, E_Element searchElement)
    {
        List<BattleCharacter> list = new List<BattleCharacter>();
        foreach (BattleCharacter bc in baseList)
        {
            if (ElementClass.IsFire(searchElement) && ElementClass.IsFire(bc.Condition.Element)) list.Add(bc);
            else if (ElementClass.IsAqua(searchElement) && ElementClass.IsAqua(bc.Condition.Element)) list.Add(bc);
            else if (ElementClass.IsTree(searchElement) && ElementClass.IsTree(bc.Condition.Element)) list.Add(bc);
        }
        return list;
    }


    /// <summary>
    /// 属性相性の倍率を返す(攻撃火、防御火・木ならば、倍率1.5倍)
    /// </summary>
    /// <param name="attackElement">攻撃属性</param>
    /// <param name="targetElement">防御属性</param>
    /// <returns>属性相性の倍率</returns>
    public static double GetElementCompatibilityRate(E_Element attackElement, E_Element targetElement)
    {
        double rate = 1.0;
        if (ElementClass.IsFire(attackElement))
        {
            if (ElementClass.IsAqua(targetElement)) rate /= 2;
            if (ElementClass.IsTree(targetElement)) rate *= 2;
        }
        if (ElementClass.IsAqua(attackElement))
        {
            if (ElementClass.IsFire(targetElement)) rate *= 2;
            if (ElementClass.IsTree(targetElement)) rate /= 2;
        }
        if (ElementClass.IsTree(attackElement))
        {
            if (ElementClass.IsFire(targetElement)) rate /= 2;
            if (ElementClass.IsAqua(targetElement)) rate *= 2;
        }
        return rate;
    }

    /// <summary>
    /// 辞書型(属性ごとターン管理)の検索属性のターン数を返す
    /// </summary>
    /// <param name="dic">検索する辞書型パラメータ</param>
    /// <param name="searchElement">検索属性</param>
    /// <returns>その属性のターン数</returns>
    public static int GetTurn(Dictionary<E_Element, int> dic, E_Element searchElement)
    {
        int turn = 0;
        if (IsFire(searchElement)) turn += dic[E_Element.Fire];
        if (IsAqua(searchElement)) turn += dic[E_Element.Aqua];
        if (IsTree(searchElement)) turn += dic[E_Element.Tree];
        return turn;
    }

    /// <summary>
    /// 辞書型(属性ごとの倍率管理)の検索属性の倍率を返す
    /// </summary>
    /// <param name="dic">検索する辞書型パラメータ</param>
    /// <param name="searchElement">検索属性</param>
    /// <returns>その属性の倍率</returns>
    public static double GetRate(Dictionary<E_Element, double> dic, E_Element searchElement)
    {
        double rate = 1;
        if (IsFire(searchElement)) rate *= dic[E_Element.Fire];
        if (IsAqua(searchElement)) rate *= dic[E_Element.Aqua];
        if (IsTree(searchElement)) rate *= dic[E_Element.Tree];
        return rate;
    }

    /// <summary>
    /// 属性ごとにターンを保持している辞書型変数へ、ターン増加(ActiveEffect)
    /// </summary>
    /// <param name="dic">属性ごとにターン保持している辞書型変数</param>
    /// <param name="effectElement">効果対象属性</param>
    /// <param name="addTurnValue">増加ターン数</param>
    public static void AddTurn(Dictionary<E_Element, int>dic, E_Element effectElement, int addTurnValue)
    {
        if (ElementClass.IsFire(effectElement)) dic[E_Element.Fire] += addTurnValue;
        if (ElementClass.IsAqua(effectElement)) dic[E_Element.Aqua] += addTurnValue;
        if (ElementClass.IsTree(effectElement)) dic[E_Element.Tree] += addTurnValue;
    }

    /// <summary>
    /// 属性ごとに倍率を保持している辞書型変数へ、倍率追加
    /// </summary>
    /// <param name="dic">属性ごとに倍率保持している辞書型変数</param>
    /// <param name="effectElement">効果対象属性</param>
    /// <param name="addRateValue">追加倍率</param>
    public static void AddRate(Dictionary<E_Element, double> dic, E_Element effectElement, double addRateValue)
    {
        if (ElementClass.IsFire(effectElement)) dic[E_Element.Fire] *= addRateValue;
        if (ElementClass.IsAqua(effectElement)) dic[E_Element.Aqua] *= addRateValue;
        if (ElementClass.IsTree(effectElement)) dic[E_Element.Tree] *= addRateValue;
    }

    /// <summary>
    /// 属性ごとにフラグを保持している辞書型変数へ、フラグ(true)追加
    /// </summary>
    /// <param name="dic">属性ごとにフラグ保持している辞書型変数</param>
    /// <param name="effectElement">効果対象属性</param>
    public static void SetTrueFlag(Dictionary<E_Element, bool> dic, E_Element effectElement)
    {
        if (ElementClass.IsFire(effectElement)) dic[E_Element.Fire] = true;
        if (ElementClass.IsAqua(effectElement)) dic[E_Element.Aqua] = true;
        if (ElementClass.IsTree(effectElement)) dic[E_Element.Tree] = true;
    }

    /// <summary>
    /// 辞書型でflag格納している変数を使ってsearchElementのflagを返す
    /// </summary>
    /// <param name="dic">属性ごとのflagが格納された検索元</param>
    /// <param name="searchElement">検索する属性</param>
    /// <returns></returns>
    public static bool GetFlagByElement(Dictionary<E_Element, bool> dic, E_Element searchElement)
    {
        if (IsFire(searchElement) && dic[E_Element.Fire] == true) return true;
        if (IsAqua(searchElement) && dic[E_Element.Aqua] == true) return true;
        if (IsTree(searchElement) && dic[E_Element.Tree] == true) return true;

        return false;
    }
}