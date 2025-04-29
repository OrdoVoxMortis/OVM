using System.Collections.Generic;
using System.IO;
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
    [SerializeField] private Button deleteBtn;
    [SerializeField] private TextMeshProUGUI dialogText;


    [Header("스테이지 데이터")]
    [SerializeField] private GameObject normalStagePrefab;
    [SerializeField] private GameObject eventStagePrefab;
    [SerializeField] private GameObject hiddenStagePrefab;

    [Header("보여줄 위치")]
    [SerializeField] private Transform stageWindow;

    private List<GameObject> loadedSlots = new(); // 생성된 슬롯들
    private GameObject currentInstance; //  현재 화면에 띄운 프리팹

    protected override void Awake()
    {
        base.Awake();
        if (backBtn != null)
            backBtn.onClick.AddListener(Hide);
        if (replayBtn != null)
            replayBtn.onClick.AddListener(ReplayGame);
        if (retryBtn != null)
            retryBtn.onClick.AddListener(RetryGame);
        if (normalBtn != null)
            normalBtn.onClick.AddListener(SetNormalStage);
        if (hiddenBtn != null)
            hiddenBtn.onClick.AddListener(SetHiddenStage);
        if (eventBtn != null)
            eventBtn.onClick.AddListener(SetEventData);
        deleteBtn.onClick.AddListener(DeleteStageData);
    }
    private void SetNormalStage()
    {
        LoadSaveSlots(normalStagePrefab);
        //ShowStageData(normalStagePrefab);
    }

    private void SetHiddenStage()
    {
        ShowStageData(hiddenStagePrefab);
        LoadSaveSlots(hiddenStagePrefab);
    }

    private void SetEventData()
    {
        ShowStageData(eventStagePrefab);
        LoadSaveSlots(eventStagePrefab);
    }

    private void ShowStageData(GameObject go)
    {
        if (currentInstance != null)
        {
            Destroy(currentInstance);
        }

        if (go != null)
        {
            //currentInstance = Instantiate(go, stageWindow); // stageWindow 밑에 생성
            currentInstance.transform.localPosition = Vector3.zero; // 위치값 고정
            currentInstance.transform.localScale = Vector3.one; // 스케일 값 고정
        }
    }

    private void LoadSaveSlots(GameObject prefab)
    {
        string path = Application.persistentDataPath;
        string[] saveFiles = Directory.GetFiles(path, "*.json");

        foreach (string file in saveFiles)
        {
            string json = File.ReadAllText(file);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            currentInstance = Instantiate(prefab, stageWindow);
            currentInstance.transform.localScale = Vector3.one;

            var slotUI = currentInstance.GetComponent<UI_SaveSlot>();
            if (slotUI != null) slotUI.SetSlot(data);
            loadedSlots.Add(currentInstance);
        }
    }

    public void DeleteStageData()
    {

        SaveManager.Instance.DeleteData();
        foreach(Transform child in stageWindow)
        {
            Destroy(child.gameObject);
        }
    }
}
