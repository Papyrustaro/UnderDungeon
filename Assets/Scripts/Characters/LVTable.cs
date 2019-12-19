using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LVTable
{
    private const int MIN_LV = 40; //最低レア度、未限界突破の最大LV
    private const int MAX_LV = 100; //最高レア度の最大限界突破数のLV
    private const int UP_MAXLV_BY_RARITY = 10; //レア度1上がるあたりの最大レベル増分
    private const int UP_MAXLV_BY_EXCEED_LIMIT = 5; //限界突破1回あたりの最大レベル増分

    public static int MinOfMaxLV => MIN_LV;
    public static int MaxOfMaxLV => MAX_LV;
    public static int UpMaxLVByRarity => UP_MAXLV_BY_RARITY;
    public static int UpMaxLVByExceedLimit => UP_MAXLV_BY_EXCEED_LIMIT;

    private static readonly int[] expTable = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                                               11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                                               21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
                                               31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
                                               41, 42, 43, 44, 45, 46, 47, 48, 49, 50,
                                               51, 52, 53, 54, 55, 56, 57, 58, 59, 60,
                                               61, 62, 63, 64, 65, 66, 67, 68, 69, 70,
                                               71, 72, 73, 74, 75, 76, 77, 78, 79, 80,
                                               81, 82, 83, 84, 85, 86, 87, 88, 89, 90,
                                               91, 92, 93, 94, 95, 96, 97, 98, 99, 100, }; //Lv到達までに必要なExp

    public static int GetMaxLV(int rarity, int exceedLimitNum)
    {
        return MIN_LV + rarity * UP_MAXLV_BY_RARITY + exceedLimitNum * UP_MAXLV_BY_RARITY;
    }
    public static int GetLV(int haveExp, int maxLV)
    {
        for (int i = 0; i < maxLV; i++)
        {
            if(haveExp < expTable[i])
            {
                return i;
            }
        }
        Debug.Log("error: 所持経験値が最大値を超えています");
        throw new Exception();
    }
    public static int GetMaxExp(int maxLV)
    {
        return expTable[maxLV];
    }
}
