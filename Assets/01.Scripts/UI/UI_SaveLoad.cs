using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SaveLoad : BaseUI
{
    [Header("세이브 로드 창 구성요소")]
    [SerializeField] private Button backBtn;
    [SerializeField] private Button normalBtn;
    [SerializeField] private Button hiddenBtn;
    [SerializeField] private Button eventBtn;
    [SerializeField] private Button replayBtn;
    [SerializeField] private Button retryBtn;
    [SerializeField] private TextMeshProUGUI dialogText;

    [Header("스테이지 데이터")]
    [SerializeField] private GameObject normalStagePrefab;
    [SerializeField] private GameObject eventStagePrefab;
    [SerializeField] private GameObject hiddenStagePrefab;

    [Header("보여줄 위치")]
    [SerializeField] private Transform stageWindow;

    private GameObject currentInstance; //  현재 화면에 띄운 프리팹

    protected override void Awake()
    {
        base.Awake();
        if (backBtn != null)
            backBtn.onClick.AddListener(Hide);
        if(replayBtn != null)
            replayBtn.onClick.AddListener(ReplayGame);
        if(retryBtn != null)
            retryBtn.onClick.AddListener(RetryGame);
        if (normalBtn != null)
            normalBtn.onClick.AddListener(SetNormalStage);
        if (hiddenBtn != null)
            hiddenBtn.onClick.AddListener(SetHiddenStage);
        if (eventBtn != null)
            eventBtn.onClick.AddListener(SetEventData);
    }

    private void SetNormalStage()
    {
        ShowStageData(normalStagePrefab);
    }

    private void SetHiddenStage()
    {
        ShowStageData(hiddenStagePrefab);
    }

    private void SetEventData()
    {
        ShowStageData(eventStagePrefab);
    }

    private void ShowStageData(GameObject go)
    {
        if(currentInstance != null)
        {
            Destroy(currentInstance);
        }

        if(go != null)
        {
            currentInstance = Instantiate(go, stageWindow); // stageWindow 밑에 생성
            currentInstance.transform.localPosition = Vector3.zero; // 위치값 고정
            currentInstance.transform.localScale = Vector3.one; // 스케일 값 고정
        }
    }

    private void ReplayGame()
    {
        Debug.Log("게임 재시작!");
    }

    private void RetryGame()
    {
        Debug.Log("게임 재시도!");
    }
}
