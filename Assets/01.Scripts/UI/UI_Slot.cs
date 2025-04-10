using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Slot : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (UI_Sequence.dragSequence != null)
        {
            UI_Sequence draggedSequence = UI_Sequence.dragSequence.GetComponent<UI_Sequence>();


            if (draggedSequence != null)
            {
                if (transform.childCount > 0) // 현재 할당된 슬롯에 다른 블록이 있는지 검사함
                {
                    Transform exisitingSequence = transform.GetChild(0); // 슬롯에 이미 존재하는 아이템 가져오기

                    exisitingSequence.SetParent(draggedSequence.startParent); // 기존 아이템을 드래그만 하면 원래 슬롯으로 이동

                    exisitingSequence.position = draggedSequence.startParent.position; // 기존 아이템의 위치를 원래 슬롯으로 맞춰줌
                }

                draggedSequence.transform.SetParent(transform); // 드래그하던 아이템을 현재 슬롯으로 이동

                draggedSequence.transform.position = transform.position; // 드래그하던 아이템의 위치를 슬롯의 위치로 맞춰줌

                draggedSequence.startParent = transform; // 드래그앤 드랍 성공시 부모 슬롯 갱신 (드래그 실패시 새로운 슬롯으로 부터 복구)
            }

        }
    }
}