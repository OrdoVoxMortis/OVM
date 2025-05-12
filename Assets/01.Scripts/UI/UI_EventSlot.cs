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

    public void SetSlot(SaveData data)
    {
        id = data.stageId;

        if (data.events != null && data.blocks.Count > 0)
        {
            foreach (var e in data.events)
            {
                if (e == null || !e.isCollect) continue;

                eventText.text = $"{e.eventName}";
                eventImage.sprite = ResourceManager.Instance.LoadImage(e.imageName);
            }
        }
        replayBtn.onClick.AddListener(Replay);

    }

    private void Replay()
    {
        SaveManager.Instance.Replay(true);
    }
}
