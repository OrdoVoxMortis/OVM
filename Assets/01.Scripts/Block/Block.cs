
using GoogleSheet.Core.Type;
using UnityEngine;
using Hamster.ZG.Type;
using System.Collections.Generic;

[UGS(typeof(BlockType))]
public enum BlockType
{
    None,
    Tool, // 도구
    Food, // 음식
    Terrain // 지형
}
[UGS(typeof(BlockAction))]
public enum BlockAction
{
    None,
    Collect, // 수집
    Use, // 사용
    Move // 이동
}

[UGS(typeof(CombineType))]
public enum CombineType
{
    None, // 규칙 없음
    AllowByType, // 특정 유형 허용
    AllowSpecific // 특정 블럭 허용
}

public class Block : MonoBehaviour
{
    public int id;
    public string BlockName {  get; private set; } // 이름

    public BlockType Type { get; private set; } // 유형
    public BlockAction Action { get; private set; } // 행동

    public float FixedTime {  get; private set; } // 필수 시간
    public float BeforeFlexibleMarginTime {  get; private set; } // 최대 앞 유동 시간
    public float AfterFlexibleMarginTime {  get; private set; } // 최대 뒤 유동 시간
    public float CurrentBeforeFlexTime { get; private set; } // 현재 앞 유동 시간
    public float CurrentAfterFlexTime { get; private set; } // 현재 뒤 유동 시간

    public CombineRule NextCombineRule { get; private set; } // 후속 조합 규칙
    public CombineRule PreCombineRule { get; private set; } // 선행 조합 규칙

    public Block NextBlock { get; set; } // 다음 블럭
    public Block PrevBlock { get; set; } // 이전 블럭

    public bool IsDeathTrigger {  get; private set; } // 사망 트리거

    public Transform successSequenceRoot;
    public Transform failSequenceRoot;
    public Transform fixedSequenceRoot;
    public Transform afterFlexSequenceRoot;
    public Transform beforeFlexSequenceRoot;

    public List<Ghost> SuccessSequence {  get; private set; } // 성공 노트 시퀀스
    public List<Ghost> FailSequence {  get; private set; } // 실패 노트 시퀀스
    public List<Ghost> FixedSequence {get; private set;} // 고정 시간 노트 시퀀스
    public List<Ghost> BeforeFlexSequence {get; private set;} // 앞 유동 시간 노트 시퀀스
    public List<Ghost> AfterFlexSequence {get; private set;} // 뒤 유동 시간 노트 시퀀스

    private void Awake()
    {
        LoadData();
    }


    protected virtual void LoadData()
    {
        var data = DataManager.Instance.blockDict[id];

        Type = data.type;
        Action = data.action;
        BlockName = data.blockName;
        PreCombineRule = data.prevCombineRule;
        NextCombineRule = data.nextCombineRule;
        FixedTime = data.fixedTime;
        BeforeFlexibleMarginTime = data.beforeFlexibleMarginTime;
        AfterFlexibleMarginTime = data.afterFlexibleMarginTime;
        IsDeathTrigger = data.isDeathTrigger;
        CurrentAfterFlexTime = AfterFlexibleMarginTime;
        CurrentBeforeFlexTime = BeforeFlexibleMarginTime;
        for(int i = 0; i < data.beforeFlexSequence.Count; i++)
        {
            int id = data.beforeFlexSequence[i];
           // BeforeFlexSequence[i] = 고스트[i];
        }
        for(int i = 0; i < data.afterFlexSequence.Count; i++)
        {
            int id = data.afterFlexSequence[i];
            //AfterFlexSequnece[i] = 고스트[i];
        }
        for(int i = 0; i < data.fixedSequence.Count; i++)
        {
            int id = data.fixedSequence[i];
            //FixedSequence[i] = 고스트[i];
        }
        for (int i = 0; i < data.successSequence.Count; i++)
        {
            int id = data.successSequence[i];
            //SuccessSequence[i] = 고스트[i];
        }
        for(int i = 0; i < data.failSequence.Count; i++)
        {
            int id = data.failSequence[i];
            //FailSequence[i] = 고스트[i];
        }
    }

    private void InteractBlock()
    {
        BlockManager.Instance.SetGhostSequence(this);
    }


}
