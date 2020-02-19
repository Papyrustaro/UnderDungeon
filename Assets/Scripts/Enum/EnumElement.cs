using System.Collections;
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
    /// <param name="baseList"></param>
    /// <param name="searchElement"></param>
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
    /* 属性倍率を返す */
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
    public static int GetTurn(Dictionary<E_Element, int> dic, E_Element searchElement)
    {
        int turn = 0;
        if (IsFire(searchElement)) turn += dic[E_Element.Fire];
        if (IsAqua(searchElement)) turn += dic[E_Element.Aqua];
        if (IsTree(searchElement)) turn += dic[E_Element.Tree];
        return turn;
    }
    public static double GetRate(Dictionary<E_Element, double> dic, E_Element searchElement)
    {
        double rate = 1;
        if (IsFire(searchElement)) rate *= dic[E_Element.Fire];
        if (IsAqua(searchElement)) rate *= dic[E_Element.Aqua];
        if (IsTree(searchElement)) rate *= dic[E_Element.Tree];
        return rate;
    }
}