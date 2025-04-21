
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

public class Block : MonoBehaviour, IInteractable
{
    public int id;
    public string BlockName {  get; private set; } // 이름

    public BlockType Type { get; private set; } // 유형
    public BlockAction Action { get; private set; } // 행동

    public float FixedTime {  get; private set; } // 필수 시간
    public float AfterFlexibleMarginTime {  get; private set; } // 최대 뒤 유동 시간
    public float CurrentAfterFlexTime { get; private set; } // 현재 뒤 유동 시간

    public CombineRule NextCombineRule { get; private set; } // 후속 조합 규칙
    public CombineRule PreCombineRule { get; private set; } // 선행 조합 규칙

    public Block NextBlock { get; set; } // 다음 블럭
    public Block PrevBlock { get; set; } // 이전 블럭

    public bool IsDeathTrigger {  get; private set; } // 사망 트리거

    //public Transform successSequenceRoot;
    //public Transform failSequenceRoot;
    //public Transform fixedSequenceRoot;
    //public Transform afterFlexSequenceRoot;
    //public Transform beforeFlexSequenceRoot;

    public Animation SuccessSequence {  get; private set; } // 성공 노트 시퀀스
    public Animation FailSequence {  get; private set; } // 실패 노트 시퀀스
    public Animation FixedSequence {get; private set;} // 고정 시간 노트 시퀀스
    public Animation AfterFlexSequence {get; private set;} // 뒤 유동 시간 노트 시퀀스

    public bool IsInteracted { get; set; } // 타임라인 내 배치됐는지

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
        AfterFlexibleMarginTime = data.afterFlexibleMarginTime;
        IsDeathTrigger = data.isDeathTrigger;
        CurrentAfterFlexTime = AfterFlexibleMarginTime;

        SuccessSequence = ResourceManager.Instance.LoadAnimation(data.successSequence);
        FailSequence = ResourceManager.Instance.LoadAnimation(data.failSequence);
        FixedSequence = ResourceManager.Instance.LoadAnimation(data.fixedSequence);
        AfterFlexSequence = ResourceManager.Instance.LoadAnimation(data.afterFlexSequence);
    }

    public void OnInteract()
    {
        TimelineManager.Instance.AddBlock(this);
        BlockManager.Instance.OnBlockUpdate?.Invoke();
    }

    public string GetInteractComponent()
    {
        return "E키를 눌러 타임라인에 추가";
    }
}
