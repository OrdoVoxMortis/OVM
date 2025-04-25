using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Sequence : MonoBehaviour
{
    public Block block;
    public TextMeshProUGUI blockName;

    public UI_Sequence(Block block)
    {
        this.block = block;
    }

    public void Initialize(Block block)
    {
        this.block = block;

        if (blockName != null && block != null)
        {
            blockName.text = block.BlockName;
        }
    }
}
