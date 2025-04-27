using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Sequence : MonoBehaviour
{
    public Block block;
    public TextMeshProUGUI blockName;
    public Image blockRange;
    public Image blockMiddle;
    public void Initialize(Block block)
    {
        this.block = block;

        if (blockName != null && block != null)
        {
            blockName.text = block.BlockName;
            blockRange.color = GetRangeColorById(block.id);
            blockMiddle.color = GetMiddleColorById(block.id);
        }
    }

    private Color GetRangeColorById(int id)
    {
        switch (id) 
        {
            case 4:
                return new Color32(255,90,70,200); // 접촉
            case 5:
                return new Color32(255,250,140,200); // 이벤트
            default:
                return new Color32(255,129,0,200); // 기본 색
        }
    }

    private Color GetMiddleColorById(int id)
    {
        switch (id)
        {
            case 4:
                return Color.red; // 예: Tool
            case 5:
                return Color.yellow; // 예: Food
            default:
                return new Color32(255,167,6,255); // 기본 색
        }
    }
}
