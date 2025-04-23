using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactBlock : Block
{
    public bool IsTarget { get; private set; } // 타겟
    public bool IsDeath {  get; private set; }  
    protected override void LoadData()
    {
        base.LoadData();
        IsTarget = DataManager.Instance.blockDict[id].isTarget;
    }
}
