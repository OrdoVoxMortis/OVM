using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
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
        if (unlockedEvents != null && unlockedEvents.Count > 0)
        {
            foreach (var e in unlockedEvents)
            {
                if (e == null || !e.isCollect) continue;
 
                eventText.text = e.eventName;
                Debug.Log(eventText.text.ToString());
                eventImage.sprite = ResourceManager.Instance.LoadImage(e.imageName);
            }
        }

        replayBtn.onClick.AddListener(Replay);

    }

    private void Replay()
    {
        SaveManager.Instance.ReplayEvent(eventId);
    }
}
