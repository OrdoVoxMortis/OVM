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
        OnBlockUpdate += TimeLineValidator.ValidateCombinations;
    }
    public void SetGhostSequence(Block block)
    {
        ////리스트 시트에서 id 로드(Block에서 로드) -> Resource.Load로 프리팹 생성
        //foreach(int id in block.SuccessSequence)
        //{
        //    //성공 시퀀스 고스트 생성
        //    var successGhost = Instantiate();
        //}
        //foreach(int id in block.FailSequence)
        //{
        //    //실패 시퀀스 고스트 생성
        //    var failGhost = Instantiate();
        //}
        //foreach(int id in block.FixedSequence)
        //{
        //    //고정 시퀀스 고스트 생성
        //    var fixedGhost = Instantiate();
        //}
        //foreach(int id in block.BeforeFlexSequence)
        //{
        //    // 앞 유동 시간 시퀀스 고스트 생성
        //    var beforeGhost = Instantiate();
        //}
        //foreach(int id in block.AfterFlexSequence)
        //{
        //    // 뒤 유동 시간 시퀀스 고스트 생성
        //    var afterGhost = Instantiate();
        //}
    }

    public void ShowSequenceByResult(Block block, bool success) // 결과에 따라 고스트 생성
    {
        if (block.successSequenceRoot == null || block.failSequenceRoot == null) return;

        block.successSequenceRoot.gameObject.SetActive(success);
        block.failSequenceRoot.gameObject.SetActive(!success);
    }

    public void UpdateFlexGhost(Block block)
    {
        int beforeCount = Mathf.FloorToInt(block.BeforeFlexibleMarginTime / 60f * bpm);
        int afterCount = Mathf.FloorToInt(block.AfterFlexibleMarginTime / 60f * bpm);

        //for(int i = 0; i < beforeCount; i++)
        //{
        //    // 앞 유동 시간 고스트 생성
        //    var ghost = Instantiate();
        //}
        //for (int i = 0; i < afterCount; i++)
        //{
        //    // 뒤 유동 시간 고스트 생성
        //    var ghost = Instantiate();
        //}
    }

}
