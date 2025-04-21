using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Event : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    public Event eventBlock;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public UI_Event(Event eventBlock)
    {
        this.eventBlock = eventBlock;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform); // 드래그 중에는 최상위로
        canvasGroup.blocksRaycasts = false;     // 드래그 중 레이캐스트 끄기
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그가 끝나면 다시 원래 슬롯 부모로 돌아가고,
        // 가장 가까운 위치로 끼워 넣기
        transform.SetParent(originalParent);

        int newIndex = FindClosestSlotIndex();
        transform.SetSiblingIndex(newIndex);

        canvasGroup.blocksRaycasts = true;
    }

    private int FindClosestSlotIndex()
    {
        int closestIndex = 0; // 가장 가까운 슬롯의 인덱스 (초기값: 0)
        float closestDistance = float.MaxValue; // 현재까지 발견한 가장 가까운 거리

        for (int i = 0; i < originalParent.childCount; i++) // 부모 안에 있는 모든 자식 슬롯들을 검사함
        {
            Transform child = originalParent.GetChild(i);
            if (child == transform) continue; // 자기 자신은 스킵

            // 현재 드래그 중인 슬롯과 비교 대상 슬롯 간의 거리 계산
            float distance = Vector3.Distance(transform.position, child.position);

            if (distance < closestDistance) // 더 가까운 슬롯을 발견하면 업데이트
            {
                closestDistance = distance; // 현재 거리 저장
                closestIndex = i; // 가장 가까운 슬롯의 인덱스 저장
            }
        }

        return closestIndex; // 가장 가까웠건 슬롯의 인덱스 반환
    }
}
