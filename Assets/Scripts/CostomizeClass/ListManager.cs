using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

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
        return list[Random.Range(0, list.Count)];
    }
}
