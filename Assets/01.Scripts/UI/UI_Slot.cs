using UnityEngine;
using UnityEngine.EventSystems;


public class UI_Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public GameObject currentItem; // 슬롯 안에 들어있는 아이템 (프리팹 인스턴스)

    private Transform originalParent; // 드래그 시작할 때 아이템이 원래 어디에 있었는지, 기억하려고 사용
    private Canvas canvas; // 드래그 중에 아이템 따라다니게 할 때 필요

    public int slotIndex; // 시퀀스가 어느 슬롯에 생성될 지 확인 시 필요

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>(); // 자신의 부모중 canvas를 찾아서 저장한다
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            originalParent = currentItem.transform.parent; // 현재 부모 저장
            currentItem.transform.SetParent(canvas.transform); // 캔버스 위로 올림
            currentItem.GetComponent<CanvasGroup>().blocksRaycasts = false; // 드래그 중엔 Raycast 막기
        }
    }

    public void OnDrag(PointerEventData eventData) 
    {
        if (currentItem != null)
        {
            currentItem.transform.position = eventData.position; // 마우스 따라다니게
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            currentItem.transform.SetParent(originalParent); // 원래 자리로 돌림
            currentItem.transform.localPosition = Vector3.zero;
            currentItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            UI_Slot otherSlot = eventData.pointerDrag.GetComponentInParent<UI_Slot>();
            if (otherSlot != null)
            {
                SwapBlocks(otherSlot);
            }

            PullPlacedBlocks();

        }
    }

    private void SwapBlocks(UI_Slot otherSlot)
    {
        GameObject temp = currentItem;
        currentItem = otherSlot.currentItem;
        otherSlot.currentItem = temp;

        if (currentItem != null)
        {
            currentItem.transform.SetParent(transform);
            currentItem.transform.localPosition = Vector3.zero;
        }
        if (otherSlot.currentItem != null)
        {
            otherSlot.currentItem.transform.SetParent(otherSlot.transform);
            otherSlot.currentItem.transform.localPosition = Vector3.zero;
        }
    }

    private void PullPlacedBlocks()
    {
        // 슬롯들의 상위객체 찾아주기
        Transform slotsParent = transform.parent;
        int childCount = slotsParent.childCount;

        // 슬롯들을 순서대로 가져오기
        UI_Slot[] slots = new UI_Slot[childCount];
        for (int i = 0; i < childCount; i++)
        {
            slots[i] = slotsParent.GetChild(i).GetComponent<UI_Slot>();
        }

        // 상위의 코드 Slot_Manager를 통해 작동하도록 수정필요

        for (int i = 0; i<slots.Length; i++)
        {
            if (slots[i].currentItem == null)
            {
                for(int j = i+1; j < slots.Length; j++)
                {
                    if (slots[j].currentItem != null)
                    {
                        slots[i].currentItem = slots[j].currentItem;
                        slots[i].currentItem.transform.SetParent(slots[i].transform);
                        slots[i].currentItem.transform.localPosition = Vector3.zero;

                        slots[j].currentItem = null;
                        break;

                    }
                }
            }
        }
       
    }
}
