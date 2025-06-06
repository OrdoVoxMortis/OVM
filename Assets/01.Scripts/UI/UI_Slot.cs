using UnityEngine;
using UnityEngine.EventSystems;


public class UI_Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler,IPointerEnterHandler,IPointerExitHandler 
{
    public UI_Sequence currentItem; // 슬롯 안에 들어있는 아이템 (프리팹 인스턴스)

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
        if (currentItem != null)
            currentItem.SetOutline(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            currentItem.transform.SetParent(originalParent); // 원래 자리로 돌림
            currentItem.transform.localPosition = Vector3.zero;
            currentItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        if (currentItem != null)
            currentItem.SetOutline(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            UI_Slot otherSlot = eventData.pointerDrag.GetComponentInParent<UI_Slot>();
            if (otherSlot != null)
            {
                TimelineManager.Instance.MoveBlockAndShift(otherSlot.slotIndex,slotIndex);
                for (int i = 0; i < TimelineManager.Instance.PlacedBlocks.Count; i++)
                {
                    Debug.Log($"[정렬후] slot {i} = {(TimelineManager.Instance.PlacedBlocks[i] != null ? TimelineManager.Instance.PlacedBlocks[i].Name : "null")}");
                }
            }
            TimelineManager.Instance.OnBlockUpdate?.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
            currentItem.SetOutline(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentItem != null)
            currentItem.SetOutline(false);
    }
}
