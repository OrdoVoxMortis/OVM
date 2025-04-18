using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimelineManager : SingleTon<TimelineManager>
{
    //TODO. 타임라인 배치 변경될 때마다 업데이트
    public GameObject slotPrefab;
    public int slotCount;
    public Transform slotParent;
    public List<Block> PlacedBlocks { get; set; } = new();
    [SerializeField] private UI_Sequence sequencePrefab;
    public List<UI_Slot> slots = new();
    private int index = 0;

    private void Start()
    {
        CreateSlots();
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

    private void CreateSlots()
    {
        for(int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            UI_Slot uiSlot = slotObj.GetComponent<UI_Slot>(); 

            if(uiSlot != null)
            {
                AddSlot(uiSlot);
            }
        }
    }

    public void AddBlock(Block block)
    {
        PlacedBlocks.Add(block);

        //시퀀스 생성
        UI_Sequence sequenceUI = Instantiate(sequencePrefab, slots[index].transform);
        //UI_Sequence sequenceUI = sequence.GetComponent<UI_Sequence>(); // 굳이 게임오브젝트 받아올 필요가 없다
        sequenceUI.block = block;
        sequenceUI.transform.localPosition = Vector3.zero;
        slots[index].slotIndex = index;
        slots[index].currentItem = sequenceUI;
        index++;
    }

    public void AddSlot(UI_Slot newslot)
    {
        if (!slots.Contains(newslot))
        {
            slots.Add(newslot);
        }
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
