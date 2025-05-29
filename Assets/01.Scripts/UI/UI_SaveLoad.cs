using System.Collections.Generic;
using System.IO;
using TMPro;
using UniGLTF;
using Unity.VisualScripting;
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
        if (normalBtn != null)
            normalBtn.onClick.AddListener(SetNormalStage);
        if (hiddenBtn != null)
            hiddenBtn.onClick.AddListener(SetHiddenStage);
        if (eventBtn != null)
            eventBtn.onClick.AddListener(SetEventData);
        if(deleteBtn != null)
            deleteBtn.onClick.AddListener(DeleteStageData);

    }
    private void SetNormalStage()
    {
        if (stageWindow.childCount != 0)
        {
            foreach (var child in stageWindow.GetChildren())
            {
                Destroy(child.gameObject);
            }
        }
        LoadSaveData();
    }

    private void SetHiddenStage()
    {
        if (stageWindow.childCount != 0)
        {
            foreach (var child in stageWindow.GetChildren())
            {
                Destroy(child.gameObject);
            }
        }
        //ShowStageData(hiddenStagePrefab);
        LoadHidddenData();
    }

    private void SetEventData()
    {
        if (stageWindow.childCount != 0)
        {
            foreach (var child in stageWindow.GetChildren())
            {
                Destroy(child.gameObject);
            }
        }
        //ShowStageData(eventStagePrefab);
        LoadEventData();
    }

    private void ShowStageData(GameObject go)
    {


        if (go != null)
        {
            //currentInstance = Instantiate(go, stageWindow); // stageWindow 밑에 생성
            currentInstance.transform.localPosition = Vector3.zero; // 위치값 고정
            currentInstance.transform.localScale = Vector3.one; // 스케일 값 고정
        }
    }

    private void LoadSaveData()
    {

        string path = Application.persistentDataPath;
        string[] saveFiles = Directory.GetFiles(path, "save.json");

        foreach (string file in saveFiles)
        {
            string json = File.ReadAllText(file);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            currentInstance = Instantiate(normalStagePrefab, stageWindow);
            currentInstance.transform.localScale = Vector3.one;

            var slotUI = currentInstance.GetComponent<UI_SaveSlot>();
            if (slotUI != null) slotUI.SetSlot(data);
            loadedSlots.Add(currentInstance);
        }
    }
    private void LoadHidddenData()
    {
        if (currentInstance != null)
        {
            Destroy(currentInstance);
        }
        string path = $"{Application.persistentDataPath}";
        string[] saveFiles = Directory.GetFiles(path, "hidden_save.json");

        foreach (string file in saveFiles)
        {
            string json = File.ReadAllText(file);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            currentInstance = Instantiate(hiddenStagePrefab, stageWindow);
            currentInstance.transform.localScale = Vector3.one;

            var slotUI = currentInstance.GetComponent<UI_SaveSlot>();
            if (slotUI != null) slotUI.SetSlot(data);
            loadedSlots.Add(currentInstance);
        }
    }
    private void LoadEventData()
    {
        if (currentInstance != null)
        {
            Destroy(currentInstance);
        }
        var unlockedEvents = SaveManager.Instance.GetUnlockEvents();
        if (unlockedEvents == null || unlockedEvents.Count == 0) return;

        foreach (var eventData in unlockedEvents)
        {
            if (eventData == null || !eventData.isCollect) continue;

            GameObject instance = Instantiate(eventStagePrefab, stageWindow);
            instance.transform.localScale = Vector3.one;

            var slotUI = instance.GetComponent<UI_EventSlot>();
            if (slotUI != null) slotUI.SetSlot(eventData);

            loadedSlots.Add(instance);
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
