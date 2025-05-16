using UnityEngine;

public class RectTransformPosition : MonoBehaviour
{
    public Vector2 fixedPosition;  // 고정할 위치 (x, y)

    void Start()
    {
        GetComponent<RectTransform>().anchoredPosition = fixedPosition;
    }
}
