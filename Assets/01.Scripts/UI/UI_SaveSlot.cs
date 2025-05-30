
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Services.Analytics;
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

    [Header("히든 스테이지")]
    [SerializeField] private bool isHidden;
    public void SetSlot(SaveData data)
    {
        Debug.Log(data.stageId);
        id = data.stageId;
        if (DataManager.Instance.stageDict.TryGetValue(id, out var stage))
        {
            stageName.text = stage.stageName;
        }
        else stageName.text = "Unknown Stage";

        musicName.text = data.musicName;
        playTime.text = $"{(int)(data.playTime / 60)}분 {(int)data.playTime % 60}초";

        if(data.timeline != null && data.timeline.Count > 0)
        {
            foreach (var element in data.timeline)
            {
                if (element == null)
                {
                    Debug.Log("element null");
                    continue;
                }
                var blockObj = Instantiate(blockPrefab, blocks);
                var text = blockObj.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                {
                    if (element.isBlock)
                    {
                        var block = data.blocks.FirstOrDefault(b => b.id == element.id);
                        if (block != null) text.text = block.blockName;
                    }
                    else
                    {
                        var evt = data.events.FirstOrDefault(e => e.id == element.id);
                        if (evt != null) text.text = evt.eventName;
                    }
                }
                else Debug.Log("text not found");
            }
        }
        replayBtn.onClick.AddListener(Replay);
        if (retryBtn == null) return;
        retryBtn.onClick.AddListener(Retry);
    }

    private void Retry()
    {
        SaveManager.Instance.Retry(id);

        //var sendEvent = new CustomEvent("interact_savepoint")
        //{
        //    ["object_id"] = "TrashCan",
        //    ["stage_id"] = id,
        //    ["replay_button_click"] = false,
        //    ["retry_button_click"] = true
        //};
        //AnalyticsService.Instance.RecordEvent(sendEvent);
    }

    private void Replay()
    {
        SaveManager.Instance.Replay(isHidden);

        //var sendEvent = new CustomEvent("interact_savepoint")
        //{
        //    ["object_id"] = "TrashCan",
        //    ["stage_id"] = id,
        //    ["replay_button_click"] = true,
        //    ["retry_button_click"] = false
        //};
        //AnalyticsService.Instance.RecordEvent(sendEvent);
    }
}
