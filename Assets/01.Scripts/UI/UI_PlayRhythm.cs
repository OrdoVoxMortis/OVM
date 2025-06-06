using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UI_PlayRhythm : BaseUI
{
    public Button playBtn;
    public Image playCancle;
    public TextMeshProUGUI interactText;
    public PostProcessingToggle postProcessingToggle;
    TimelineCamera timelineCam;

    public bool isEnabled = false;

    protected override void Awake()
    {
        base.Awake();
        timelineCam = FindObjectOfType <TimelineCamera>();
        if (timelineCam == null)
        {
            Debug.LogError("TimelineCamera를 찾을 수 없습니다.");
        }
    }

    private void Start()
    {
        Hide();
        playBtn.onClick.AddListener(StartRhythm);
        playCancle.gameObject.SetActive(false);
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
            if (postProcessingToggle != null && postProcessingToggle.SimulationPlayer != null)
            {
                postProcessingToggle.SimulationPlayer.SetActive(false);
                Debug.Log("시뮬레이션 플레이어 꺼짐");
            }
            Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = isEnabled;
            interactText.gameObject.SetActive(false);  
            GameManager.Instance.OnStart?.Invoke();
            GameManager.Instance.isEnd = false;
            // GameManager.Instance.Player.Input.PlayerActionUnsubscribe(); 입력이 된 부분이 없어서 null 오류 발생!
            GameManager.Instance.Player.gameObject.SetActive(false);
            TimelineManager.Instance.gameObject.SetActive(false);

            for(int i = 0; i < TimelineManager.Instance.PlacedBlocks.Count; i++)
            {
                RhythmManager.Instance.rhythmActions.Add(TimelineManager.Instance.PlacedBlocks[i].GetComponent<IRhythmActions>());                
            }
            RhythmManager.Instance.StartMusic();
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("슬롯에 아이템이 없습니다!");
            StartCoroutine(WaitUntilShow());
        }
    }
    IEnumerator WaitUntilShow()
    {
        playCancle.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        playCancle.gameObject.SetActive(false);
    }
}
