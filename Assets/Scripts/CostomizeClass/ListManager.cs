using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ListManager
{
    public static void ShowAllIndex<T>(List<T> list)
    {
        Debug.Log("ShowAllIndexOf" + list);
        int i = 0;
        foreach(T item in list)
        {
            Debug.Log(i.ToString() + ": " + item);
            i++;
        }
    }
    public static T GetRandomIndex<T>(List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
    public static List<T> MakeListFromBaseList<T>(List<T> baseList, List<int> indexList)
    {
        List<T> makeList = new List<T>();
        foreach(int i in indexList)
        {
            makeList.Add(baseList[i]);
        }
        return makeList;
    }
}
