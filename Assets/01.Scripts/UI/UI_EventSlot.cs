using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class UI_EventSlot : MonoBehaviour
{
    [Header("버튼")]
    public Button replayBtn;

    [Header("이벤트 정보")]
    private string id;
    public TextMeshProUGUI eventText;
    public Image eventImage;

    private int eventId;
    public void SetSlot(EventSaveData data)
    {
        id = data.stageId;
        eventId = data.id;
        var unlockedEvents = SaveManager.Instance.GetUnlockEvents();
        if (data != null)
        {

            if (data == null || !data.isCollect) return;

            eventText.text = data.eventName;
            Debug.Log(gameObject.GetHashCode() + eventText.text.ToString());
            eventImage.sprite = ResourceManager.Instance.LoadImage(data.imageName);

        }

        replayBtn.onClick.AddListener(Replay);

    }

    private void Replay()
    {
        SaveManager.Instance.ReplayEvent(eventId);

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
