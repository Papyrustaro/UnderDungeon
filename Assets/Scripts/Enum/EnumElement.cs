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
}