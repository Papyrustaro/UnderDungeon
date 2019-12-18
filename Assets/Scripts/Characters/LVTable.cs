using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVTable : MonoBehaviour
{
    private const int MAX_LV = 200; //最高レア度の最大限界突破数のLV
    private static readonly int[] lvTable = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; //Lv到達までに必要なExp

    public static int GetLV(int haveExp, int maxLV)
    {
        if(maxLV > MAX_LV)
        {
            Debug.Log("error: 最大レベルを超えています");
            return -1;
        }
        for (int i = 0; i < maxLV; i++)
        {
            if (haveExp < lvTable[i])
            {
                return i;
            }
        }
        return maxLV;
    }
    public static int GetMaxExp(int maxLV)
    {
        if(maxLV > MAX_LV)
        {
            Debug.Log("error: 最大レベルを超えています");
            return -1;
        }
        return lvTable[maxLV];
    }
    public static int GetMaxExceedLimitNum(int rarity)
    {
        return rarity;
    }
    public static int GetMaxLV(int rarity, int exceedLimitNum)
    {
        return 60 + rarity * 10 + exceedLimitNum * 10;
    }
}
