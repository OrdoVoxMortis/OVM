using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SaveSlot : MonoBehaviour
{
    [Header("버튼")]
    public Button replayBtn;
    public Button retryBtn;

    [Header("스테이지 정보")]
    private string id;
    public Image stageImage;
    public TextMeshProUGUI stageName;
    public TextMeshProUGUI musicName;
    public TextMeshProUGUI playTime;
    public Transform blocks;
    public GameObject blockPrefab;

    public void SetSlot(SaveData data)
    {
        id = data.stageId;
        if (DataManager.Instance.stageDict.TryGetValue(data.stageId, out var stage))
        {
            stageName.text = stage.stageName;
        }
        else stageName.text = "Unknown Stage";

        musicName.text = data.musicId;
        playTime.text = $"{(int)(data.playTime / 60)}분 {(int)data.playTime % 60}초";

        if(data.blocks != null && data.blocks.Count > 0)
        {
            foreach (var block in data.blocks)
            {
                if (block == null)
                {
                    Debug.Log("block null");
                    continue;
                }
                var blockObj = Instantiate(blockPrefab, blocks);
                var text = blockObj.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null) text.text = block.blockName;
                else Debug.Log("text not found");
            }
        }
        replayBtn.onClick.AddListener(SaveManager.Instance.Replay);
        retryBtn.onClick.AddListener(Retry);
    }

    private void Retry()
    {
        SaveManager.Instance.Retry(id);
    }

}
