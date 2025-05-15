using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScrollingText : MonoBehaviour
{
    public RectTransform textRect;
    public float speed = 50f;

    private float startX;
    private float endX;
    private RectTransform parentRect;

    void Start()
    {
        parentRect = textRect.parent.GetComponent<RectTransform>();

        LayoutRebuilder.ForceRebuildLayoutImmediate(textRect);

        float parentWidth = parentRect.rect.width;
        float textWidth = textRect.rect.width;

        startX = parentWidth / 2 + textWidth / 2;
        endX = -startX;

        textRect.anchoredPosition = new Vector2(startX, textRect.anchoredPosition.y);
    }

    void Update()
    {
        textRect.anchoredPosition += Vector2.left * speed * Time.unscaledDeltaTime;

        if (textRect.anchoredPosition.x <= endX)
        {
            textRect.anchoredPosition = new Vector2(startX, textRect.anchoredPosition.y);
        }
    }
}