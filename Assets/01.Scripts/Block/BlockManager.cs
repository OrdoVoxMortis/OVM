using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : SingleTon<BlockManager>
{
    public Action OnBlockUpdate; //TODO. 상호작용 & 드롭될 때 호출
    private float bpm = 120f; // TODO.추후에 받아오기
    private void Start()
    {
        OnBlockUpdate += TimelineManager.Instance.ValidateCombinations;
    }



}
