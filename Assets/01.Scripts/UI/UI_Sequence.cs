using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Sequence : MonoBehaviour
{
    public TimelineElement item;
    public TextMeshProUGUI blockName;
    public Image blockRange;
    public Image blockMiddle;
    public void Initialize(TimelineElement item)
    {
        this.item = item;

        if (blockName != null && item != null)
        {
            blockName.text = item.Name;
            blockRange.color = GetRangeColorById(item);
            blockMiddle.color = GetMiddleColorById(item);
        }
    }

    private Color GetRangeColorById(TimelineElement type)
    {
        switch (type) 
        {
            case ContactBlock:
                return new Color32(255,90,70,200); // 접촉
            case Event:
                return new Color32(255,250,140,200); // 이벤트
            default:
                return new Color32(255,129,0,200); // 기본 색
        }
    }

    private Color GetMiddleColorById(TimelineElement type)
    {
        switch (type)
        {
            case ContactBlock:
                return Color.red; // 예: Tool
            case Event:
                return Color.yellow; // 예: Food
            default:
                return new Color32(255,167,6,255); // 기본 색
        }
    }
}
