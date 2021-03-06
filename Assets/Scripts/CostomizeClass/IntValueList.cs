﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 2次元のListをInspector上から代入したいときに使うクラス */
/* 参考: http://kan-kikuchi.hatenablog.com/entry/ValueListList */
[System.SerializableAttribute]
public class IntValueList
{
    [SerializeField]
    private List<IntValueList> valueListList = new List<IntValueList>(); //使用例

    public List<int> list = new List<int>();
    public IntValueList(List<int> list)
    {
        this.list = list;
    }
}
