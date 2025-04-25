using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Sequence : MonoBehaviour
{
    public Block block;
    public Color contactBlockColor = Color.red;
    public TextMeshProUGUI blockName;

    public void Initialize(Block block)
    {
        this.block = block;

        if (blockName != null && block != null)
        {
            blockName.text = block.BlockName;
        }

        if(block is ContactBlock)
        {
            blockName.color = contactBlockColor;
        }
    }
}
