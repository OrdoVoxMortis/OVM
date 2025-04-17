using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimelineManager : SingleTon<TimelineManager>
{
    //TODO. 타임라인 배치 변경될 때마다 업데이트
    public List<Block> PlacedBlocks { get; set; } = new();
    [SerializeField] private GameObject sequencePrefab;
    private List<UI_Slot> slots = new();
    private int index = 0;

    //PlacedBlocks에 원소 하나 추가될 때마다 -> index(상호작용된 블럭 수)++ (처음엔 0개니까 index 0에 생성됨)
    //index에 맞는 slot자식으로 Sequence_UI 생성 
    private void Start()
    {
        InitSlots();
    }

    public void InitSlots()
    {
        slots.Clear();

        foreach (Transform child in gameObject.transform)
        {
            UI_Slot slot = child.GetComponent<UI_Slot>();
            if(slot != null) slots.Add(slot);
        }
    }

    public void AddBlock(Block block)
    {
        PlacedBlocks.Add(block);

        //시퀀스 생성
        GameObject sequence = Instantiate(sequencePrefab, slots[index].transform);
        UI_Sequence sequenceUI = sequence.GetComponent<UI_Sequence>();
        sequenceUI.block = block;
        sequence.transform.localPosition = Vector3.zero;

        slots[index].currentItem = sequence;
        index++;
    }
    public void ValidateCombinations()
    {
        for (int i = 0; i < PlacedBlocks.Count; i++)
        {
            Block current = PlacedBlocks[i];

            bool isSuccess = true;

            // 선행 규칙 검사
            if (current.PreCombineRule != null && current.PreCombineRule.RuleType != CombineType.None)
            {
                bool hasValidPrev = HasValidPreviousBlock(current, i);
                if (!hasValidPrev && BlockValidator.RequiresPrevBlock(current))
                {
                    isSuccess = false;
                }
            }

            // 후속 규칙 검사
            if (current.NextCombineRule != null && current.NextCombineRule.RuleType != CombineType.None)
            {
                bool hasValidNext = HasValidNextBlock(current, i);
                if (!hasValidNext && BlockValidator.RequiresNextBlock(current))
                {
                    isSuccess = false;
                }
            }

            //TODO. 검사 결과에 따라 시퀀스 출력
            

            Debug.Log($"[{current.BlockName}] 조합 결과: {(isSuccess ? "성공" : "실패")}");
        }
    }

    private bool HasValidPreviousBlock(Block block, int index)
    {
        for (int i = 0; i < index; i++)
        {
            if (BlockValidator.CanCombineWithPrev(block, PlacedBlocks[i]))
                return true;
        }
        return false;
    }
    private bool HasValidNextBlock(Block block, int index)
    {
        for (int i = index + 1; i < PlacedBlocks.Count; i++)
        {
            if (BlockValidator.CanCombineWithNext(block, PlacedBlocks[i]))
                return true;
        }
        return false;
    }
}
