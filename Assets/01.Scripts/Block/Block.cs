
using GoogleSheet.Core.Type;
using UnityEngine;


[UGS(typeof(BlockType))]
public enum BlockType
{
    Tool, // 도구
    Food, // 음식
    Terrain // 지형
}
[UGS(typeof(BlockAction))]
public enum BlockAction
{
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
    public float FlexibleMarginTime {  get; private set; } // 유동 시간

    public CombineRule NextCombineRule { get; private set; } // 후속 조합 규칙
    public CombineRule PreCombineRule { get; private set; } // 선행 조합 규칙

    public Block NextBlock { get; set; } // 다음 블럭
    public Block PrevBlock { get; set; } // 이전 블럭

    public bool IsDeathTrigger {  get; private set; } // 사망 트리거

    //public List<Ghost?> SuccessSequence {  get; private set; } // 성공 노트 시퀀스
    //public List<Ghost?> FailSequence {  get; private set; } // 실패 노트 시퀀스

    private void InitBlock()
    {
        LoadData();
    }

    protected virtual void LoadData()
    {
        var data = DataManager.Instance.blockDict[id];

        Type = data.Type;
        Action = data.Action;
        BlockName = data.BlockName;
        PreCombineRule = data.PrevCombineRule;
        NextCombineRule = data.NextCombineRule;
        FixedTime = data.FixedTime;
        FlexibleMarginTime = data.FlexibleMarginTime;
    }
    private void Start()
    {
        InitBlock();

    }
}
