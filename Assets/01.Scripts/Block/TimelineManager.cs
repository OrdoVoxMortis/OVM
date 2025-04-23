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
    [SerializeField] private UI_Event eventBlockPrefab;
    [SerializeField] private UI_Sequence targetBlockPrefab;
    public List<UI_Slot> slots = new();
    private int index = 0;

    private void Start()
    {
        CreateSlots();
        InitSlots();
        gameObject.SetActive(false);
    }

    public void InitSlots()
    {
        slots.Clear();

        foreach (Transform child in gameObject.transform)
        {
            UI_Slot slot = child.GetComponent<UI_Slot>();
            if (slot != null) slots.Add(slot);
        }
    }

    private void CreateSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            UI_Slot uiSlot = slotObj.GetComponent<UI_Slot>();

            if (uiSlot != null)
            {
                AddSlot(uiSlot);
            }
        }
    }

    public void AddBlock(Block block)
    {
        if (!block.IsActive)
        {
            block.IsActive = true;
            PlacedBlocks.Add(block);
            UI_Sequence sequenceUI;

            //시퀀스 생성
            if (block is ContactBlock) sequenceUI = Instantiate(targetBlockPrefab, slots[index].transform);
            else sequenceUI = Instantiate(sequencePrefab, slots[index].transform);

            //UI_Sequence sequenceUI = sequence.GetComponent<UI_Sequence>(); // 굳이 게임오브젝트 받아올 필요가 없다
            sequenceUI.block = block;
            sequenceUI.transform.localPosition = Vector3.zero;
            slots[index].slotIndex = index;
            slots[index].currentItem = sequenceUI;
            index++;
        }
    }

    public void DestroyBlock(Block block)
    {
        if (block.IsActive)
        {
            int index = PlacedBlocks.IndexOf(block);
            if (index >= 0)
            {
                block.IsActive = false;
                RemoveAndShiftLeft(index);
                
            }
        }
    }

    public void DestroyEvent(Event eventblock)
    {
        if (eventblock.IsActive)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].currentItem != null)
                {
                    UI_Event uiEvent = slots[i].currentItem.GetComponent<UI_Event>();
                    if (uiEvent != null && uiEvent.eventBlock == eventblock)
                    {
                        // 찾았으면 삭제
                        eventblock.IsActive = false;
                        Destroy(slots[i].currentItem.gameObject);
                        slots[i].currentItem = null;

                        // 슬롯 왼쪽으로 밀기
                        for (int j = i; j < slots.Count - 1; j++)
                        {
                            slots[j].currentItem = slots[j + 1].currentItem;
                            if (slots[j].currentItem != null)
                            {
                                slots[j].currentItem.transform.SetParent(slots[j].transform);
                                slots[j].currentItem.transform.localPosition = Vector3.zero;
                            }
                        }

                        // 마지막 슬롯 정리
                        if (slots.Count > 0)
                        {
                            int lastIndex = slots.Count - 1;
                            slots[lastIndex].currentItem = null;
                        }

                        index--;
                        break;
                    }
                }
            }
        }
    }
    public void AddEventSlot(Event eventblock)
    {
        UI_Event eventUI = Instantiate(eventBlockPrefab, slotParent);
        eventUI.eventBlock = eventblock;
        eventUI.transform.localPosition = Vector3.zero;
        index++;
    }

    public void AddContactBlock(ContactBlock block)
    {
        UI_Sequence targetUI = Instantiate(targetBlockPrefab, slots[index].transform);
        targetUI.block = block;
        targetUI.transform.localPosition = Vector3.zero;
        slots[index].slotIndex = index;
        slots[index].currentItem = targetUI;
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
        if (PlacedBlocks.Count == 0) return;
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

            current.IsSuccess = isSuccess;
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

    public void MoveBlockAndShift(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || fromIndex >= PlacedBlocks.Count) return;

        if (toIndex < 0) toIndex = 0;
        if (toIndex >= PlacedBlocks.Count) toIndex = PlacedBlocks.Count - 1;

        Block blockToMove = PlacedBlocks[fromIndex];
        PlacedBlocks.RemoveAt(fromIndex);

        PlacedBlocks.Insert(toIndex, blockToMove);
    }
    
    public void RemoveAndShiftLeft(int removeIndex)
    {
        if (removeIndex < 0 || removeIndex >= PlacedBlocks.Count) return;

        // 먼저 삭제할 GameObject를 파괴한다
        if (slots[removeIndex].currentItem != null)
        {
            Destroy(slots[removeIndex].currentItem.gameObject);
            slots[removeIndex].currentItem = null;
        }
        PlacedBlocks.RemoveAt(removeIndex);

        for(int i = removeIndex; i < slots.Count -1; i++)
        {
            slots[i].currentItem = slots[i + 1].currentItem;
            if (slots[i].currentItem != null)
            {
                slots[i].currentItem.transform.SetParent(slots[i].transform);
                slots[i].currentItem.transform.localPosition = Vector3.zero;
            }
        }

        if(slots.Count > 0)
        {
            int lastIndex = PlacedBlocks.Count;
            if (lastIndex < slots.Count) slots[lastIndex].currentItem = null;
        }
        index = PlacedBlocks.Count;
    }
}
