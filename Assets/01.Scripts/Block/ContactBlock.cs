using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactBlock : Block, IInteractable
{
    public bool IsTarget {  get; private set; } // 타겟

    protected override void LoadData()
    {
        base.LoadData();
        IsTarget = DataManager.Instance.blockDict[id].isTarget;
    }

    public void OnInteract()
    {
        TimelineManager.Instance.AddContactBlock(this);
        BlockManager.Instance.OnBlockUpdate?.Invoke();
    }
}
