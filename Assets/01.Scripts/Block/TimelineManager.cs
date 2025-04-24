using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineManager : SingleTon<TimelineManager>
{
    public GameObject slotPrefab;
    public int slotCount;
    public Transform slotParent;
    public List<Block> PlacedBlocks { get; set; } = new();
    [SerializeField] private UI_Sequence sequencePrefab;
    [SerializeField] private UI_Event eventBlockPrefab;
    [SerializeField] private UI_Sequence targetBlockPrefab;
    public List<UI_Slot> slots = new();
    public List<UI_Event> eventslots= new();
    private int index = 0;

    public float blockTime = 0f;

    private void Start()
    {
        CreateSlots();
        InitSlots();
        gameObject.SetActive(false);
    }

    public void CalBlockTime()
    {
        foreach (var block in PlacedBlocks)
        {
            blockTime += block.FixedTime;
        }
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
        foreach (var slot in eventslots)
        {
            if (slot.eventBlock == eventblock)
            {
                eventblock.IsActive = false;
                eventslots.Remove(slot);
                Destroy(slot.gameObject);
                break;
            }
        }
    }
    public void AddEventSlot(Event eventblock)
    {
        UI_Event eventUI = Instantiate(eventBlockPrefab, slotParent);
        eventUI.eventBlock = eventblock;
        eventUI.transform.localPosition = Vector3.zero;
        eventslots.Add(eventUI);
        //slots[index].slotIndex = index;
        //index++;
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

        // 모든 블럭 초기화
        foreach (var block in PlacedBlocks)
        {
            block.IsSuccess = false;
        }

        // 조합에 사용 가능한 블럭 리스트
        List<Block> availableBlocks = new List<Block>(PlacedBlocks);

        for (int i = 0; i < PlacedBlocks.Count; i++)
        {
            Block current = PlacedBlocks[i];

            if (!availableBlocks.Contains(current)) continue;

            bool success = true;
            Block prevSuccess = null;
            Block nextSuccess = null;

            // 선행 검사
            if (current.PreCombineRule != null && current.PreCombineRule.RuleType != CombineType.None)
            {
                success = false;
                foreach (var other in availableBlocks)
                {
                    if (other == current) continue;

                    if (BlockValidator.CanCombineWithPrev(current, other))
                    {
                        prevSuccess = other;
                        success = true;
                        break;
                    }
                }

                if (!success && BlockValidator.RequiresPrevBlock(current))
                {
                    current.IsSuccess = false;
                    continue;
                }
            }

            // 후속 검사
            success = true; // 다시 초기화
            if (current.NextCombineRule != null && current.NextCombineRule.RuleType != CombineType.None)
            {
                success = false;
                foreach (var other in availableBlocks)
                {
                    if (other == current) continue;

                    if (BlockValidator.CanCombineWithNext(current, other))
                    {
                        nextSuccess = other;
                        success = true;
                        break;
                    }
                }

                if (!success && BlockValidator.RequiresNextBlock(current))
                {
                    current.IsSuccess = false;
                    continue;
                }
            }

            // 조합 성공 처리
            current.IsSuccess = true;
            if (prevSuccess != null)
            {
                prevSuccess.IsSuccess = true;
                Debug.Log($"[{current.BlockName}] + [{prevSuccess.BlockName}] 조합 결과: 성공");
            }
            if (nextSuccess != null)
            {
                nextSuccess.IsSuccess = true;
                Debug.Log($"[{current.BlockName}] + [{nextSuccess.BlockName}] 조합 결과: 성공");
            }

            current.SetGhost();
            prevSuccess?.SetGhost();
            nextSuccess?.SetGhost();
            

            // 사용된 블럭 제거
            availableBlocks.Remove(current);
            if (prevSuccess != null) availableBlocks.Remove(prevSuccess);
            if (nextSuccess != null) availableBlocks.Remove(nextSuccess);
        }

        // 실패 블럭들 고스트 처리
        foreach (var block in PlacedBlocks)
        {
            if (!block.IsSuccess)
            {
                block.SetGhost();
                Debug.Log($"[{block.BlockName}] 조합 결과: 실패");
            }
        }
    }


    private bool HasValidPreviousBlock(Block block, int index)
    {
        for (int i = 0; i < index; i++)
        {
            if (BlockValidator.CanCombineWithPrev(block, PlacedBlocks[i]))
            {
                return true;
            }
        }
        return false;
    }
    private bool HasValidNextBlock(Block block, int index)
    {
        for (int i = index + 1; i < PlacedBlocks.Count; i++)
        {
            if (BlockValidator.CanCombineWithNext(block, PlacedBlocks[i]))
            {
                return true;
            }
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
