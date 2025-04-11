using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Sequence : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject dragSequence; // 드래그 되는 아이콘

    Vector3 startPosition; // 원래 드래그 되기 전의 위치

    [SerializeField] Transform onDragParent; // 드래그 중 변경할 부모 RectTransform 변수
    [HideInInspector] public Transform startParent; // 슬롯이 아닌 다른 오브젝트에 드랍할때 돌아갈 위치

    public void OnBeginDrag(PointerEventData eventData) // IBeginDragHandler 콜백함수
    {
        dragSequence = gameObject; // 드래그가 시작될때, 대상 ICON의 게임오브젝트를 static 변수에 할당 

        startPosition = transform.position;
        startParent = transform.parent;

        transform.SetParent(onDragParent);
        // 드래그 시작할때 부모 transform 변경
    }

    public void OnDrag(PointerEventData eventData) // IDragHandler 콜백함수
    {
        transform.position = Input.mousePosition; // 드래그 중에는 Icon을 마우스나 터치된 포인트의 위치로 이동
    }

    public void OnEndDrag(PointerEventData eventData) // IEndDragHandler 콜백함수
    {
        dragSequence = null;
        if(transform.parent == onDragParent)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
    }
}

