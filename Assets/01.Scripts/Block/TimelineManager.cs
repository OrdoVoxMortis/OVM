using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimelineManager : SingleTon<TimelineManager>
{
    public GameObject slotPrefab;
    public int slotCount;
    public Transform slotParent;
    public List<TimelineElement> PlacedBlocks { get; set; } = new();
    [SerializeField] private UI_Sequence sequencePrefab;
    [SerializeField] private UI_Event eventBlockPrefab;
    public List<UI_Slot> slots = new();
    public List<UI_Event> eventslots= new();
    private int index = 0;

    public float blockTime = 0f;
    protected override void Awake()
    {
        base.Awake();
        CreateSlots();
        InitSlots();
        PlacedBlocks.Clear();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void CalBlockTime(Block block)
    {
        blockTime += block.FixedTime;
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

    public void AddBlock(TimelineElement block)
    {
        if (!block.IsActive)
        {
            block.IsActive = true;
            PlacedBlocks.Add(block);
            UI_Sequence sequenceUI;

            //시퀀스 생성
            sequenceUI = Instantiate(sequencePrefab, slots[index].transform);
            sequenceUI.Initialize(block); 
            //UI_Sequence sequenceUI = sequence.GetComponent<UI_Sequence>(); // 굳이 게임오브젝트 받아올 필요가 없다
            sequenceUI.item = block;
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
        slots[index].slotIndex = index;
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
        List<Block> blockList = new List<Block>(ReturnBlocks());
        if (blockList.Count == 0) return;

        foreach (var block in blockList)
        {
            if (block.IsDeathTrigger) continue;
            block.IsSuccess = false;
        } 

        List<Block> availableBlocks = new List<Block>(blockList);
        foreach(var block in blockList)
        {
           availableBlocks.Add(block);
        }

        for (int i = 0; i < blockList.Count; i++)
        {
            Block current = blockList[i];
            if (!availableBlocks.Contains(current)) continue;

            bool success = true;
            Block prevSuccess = null;
            Block nextSuccess = null;

            // 접촉 블럭 특수 규칙 처리
            if (current is ContactBlock contact)
            {
                ContactBlockValid(contact, i);
                continue;
            }

            // 선행 조합 검사 (앞 블럭만)
            if (current.PreCombineRule != null && current.PreCombineRule.RuleType != CombineType.None)
            {
                success = false;
                for (int j = 0; j < i; j++)
                {
                    Block other = blockList[j];
                    if (!availableBlocks.Contains(other)) continue;

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

            // 후속 조합 검사 (뒤 블럭만)
            if (current.NextCombineRule != null && current.NextCombineRule.RuleType != CombineType.None)
            {
                success = false;
                for (int j = i + 1; j < PlacedBlocks.Count; j++)
                {
                    Block other = blockList[j];
                    if (!availableBlocks.Contains(other)) continue;

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
                Debug.Log($"[{prevSuccess.Name}] + [{current.Name}] 조합 결과: 성공");
            }
            if (nextSuccess != null)
            {
                nextSuccess.IsSuccess = true;
                Debug.Log($"[{current.Name}] + [{nextSuccess.Name}] 조합 결과: 성공");
            }

            current.SetGhost();
            prevSuccess?.SetGhost();
            nextSuccess?.SetGhost();

            // 사용된 블럭 available에서 제거
            availableBlocks.Remove(current);
            if (prevSuccess != null) availableBlocks.Remove(prevSuccess);
            if (nextSuccess != null) availableBlocks.Remove(nextSuccess);
        }

        // 실패 처리
        foreach (var block in blockList)
        {
            if (!block.IsSuccess)
            {
                block.SetGhost();
                Debug.Log($"[{block.Name}] 조합 결과: 실패");
            }
        }
    }



    private void ContactBlockValid(ContactBlock contact, int index)
    {
        bool hasDeathTriggerBefore = PlacedBlocks.GetRange(0, index).OfType<Block>().Any(b => b.IsDeathTrigger && b.IsSuccess);
        if (hasDeathTriggerBefore)
        {
            contact.IsSuccess = true;
            contact.SetGhost();
            Debug.Log($"{contact.Name} 접촉 블럭 조건 만족: 성공한 사망 트리거 존재");
        }
        else Debug.Log("사망 트리거 없음");
    }


    public void MoveBlockAndShift(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || fromIndex >= PlacedBlocks.Count) return;

        if (toIndex < 0) toIndex = 0;
        if (toIndex >= PlacedBlocks.Count) toIndex = PlacedBlocks.Count - 1;

        TimelineElement blockToMove = PlacedBlocks[fromIndex];
        PlacedBlocks.RemoveAt(fromIndex);
        PlacedBlocks.Insert(toIndex, blockToMove);

        for(int i = 0; i < slots.Count; i++)
        {
            if(i < PlacedBlocks.Count)
                slots[i].currentItem.Initialize(PlacedBlocks[i]);
            else
            {
                if(slots[i].currentItem != null)
                {
                    Destroy(slots[i].currentItem.gameObject);
                    slots[i].currentItem = null;
                }
            }
        }
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

    public void LoadBlocks(List<TimelineSaveData> elementIds)
    {
        Block[] allBlocks = FindObjectsOfType<Block>();
        Event[] allEvents = FindObjectsOfType<Event>();

        foreach (var element in elementIds)
        {
            if (element.isBlock)
            {
                var block = allBlocks.FirstOrDefault(b => b.id == element.id);
                if (block != null && !PlacedBlocks.Contains(block))
                {
                    AddBlock(block);
                }
                else Debug.Log($"Block: {element.id} not found");
            }
            else
            {
                var evt = allEvents.FirstOrDefault(e => e.id == element.id);
                if (evt != null && !PlacedBlocks.Contains(evt))
                {
                    AddBlock(evt);
                    evt.IsCollect = true;
                }
                else Debug.Log($"Event: {element.id} not found");
            }
            foreach (var item in PlacedBlocks)
            {
                if (item is Block block)
                    block.SetGhost();
            }
            ValidateCombinations();
        }
    }

    public List<Block> ReturnBlocks()
    {
        List<Block> blocks = new();
        foreach(var item in PlacedBlocks)
        {
            if(item is Block block) blocks.Add(block);
        }
        return blocks;
    }

    public List<Event> ReturnEvents()
    {
        List<Event> events = new();
        foreach(var item in PlacedBlocks)
        {
            if(item is Event e) events.Add(e);
        }
        return events;
    }
}
