using UnityEngine;
using UnityEngine.UI;

public class UI_PlayRhythm : BaseUI
{
    public Button playBtn;
    protected virtual void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Hide();
        playBtn.onClick.AddListener(StartRhythm);
    }

    private void StartRhythm()
    {
        // 모든 슬롯 가져오기
        var slots = TimelineManager.Instance.slots;

        // 슬롯 중 하나라도 currentItem이 있으면 실행
        bool hasItem = false;
        foreach (var slot in slots)
        {
            if (slot.currentItem != null)
            {
                hasItem = true;
                break;
            }
        }

        if (hasItem)
        {
            Debug.Log("암살 시작!");
            // 여기 암살 시작하는 코드 넣으면 됨
            RhythmManager.Instance.StartMusic();
        }
        else
        {
            Debug.Log("슬롯에 아이템이 없습니다!");
        }
    }
}
