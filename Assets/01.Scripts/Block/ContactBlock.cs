using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactBlock : Block
{
    Enemy enemy;
    public bool IsTarget {  get; private set; } // 타겟

    protected override void LoadData()
    {
        base.LoadData();
        IsTarget = DataManager.Instance.blockDict[id].isTarget;
    }
}
