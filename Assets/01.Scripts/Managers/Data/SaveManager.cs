using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public string stageId;
    public float playTime;
    public string musicId; //(이름)
    public List<BlockSaveData> blocks;
    public List<EventSaveData> events;
    public List<TimelineSaveData> timeline;
}

[System.Serializable]
public class BlockSaveData
{
    public int id;
    public string blockName; 
}

[System.Serializable]
public class EventSaveData
{
    public int id;
    public string stageId;
    public string eventName;
    public string imageName;
    public string bgmName;
    public bool isCollect;
}

[System.Serializable]
public class TimelineSaveData
{
    public bool isBlock;
    public int id;
}

[System.Serializable]
public class EventUnlockData
{
    public List<TimelineSaveData> timeline;
    public List<EventSaveData> unlockedEvents = new();
}


public class SaveManager : SingleTon<SaveManager>
{
    private string SavePath => $"{Application.persistentDataPath}/save.json";
    private string UnlockPath => $"{Application.persistentDataPath}/Event/event_Unlock.json";
    private string HiddenPath => $"{Application.persistentDataPath}/Hidden/hidden_save.json";

    private List<TimelineSaveData> elementIds = new();
    private EventUnlockData unlockData;
    public bool isReplay;
    public bool eventReplay;
    //TODO. 스테이지 정보 저장&로드
    public void SaveGame()
    {
        SaveData data = new SaveData();

        data.blocks = new List<BlockSaveData>();
        data.events = new List<EventSaveData>();
        data.timeline = new List<TimelineSaveData>();
        LoadUnlockedEvents();
        data.stageId = StageManager.Instance.StageResult.id;

        data.playTime = StageManager.Instance.PlayTime;
        
        foreach (var element in TimelineManager.Instance.PlacedBlocks)
        {
            if (element is Block block)
            {
                data.blocks.Add(new BlockSaveData
                {
                    id = block.id,
                    blockName = block.Name
                });
                data.timeline.Add(new TimelineSaveData
                {
                    isBlock = true,
                    id = block.id
                });
            }
            else if (element is Event e)
            {
                var evt = new EventSaveData
                {
                    id = e.id,
                    eventName = e.Name,
                    imageName = e.ImageName,
                    bgmName = e.BgmName,
                    stageId = StageManager.Instance.StageResult.id,
                    isCollect = true
                };
                data.events.Add(evt);
                data.timeline.Add(new TimelineSaveData
                {
                    isBlock = false,
                    id = e.id,
                });

                UnlockEvent(evt);
            }
        }

        data.musicId = GameManager.Instance.BgmId;

        string path = SavePath;
        if(DataManager.Instance.stageDict.TryGetValue(data.stageId, out var stage))
        {
            if(stage.stageType == StageType.Hidden)
            {
                path = HiddenPath;
                Debug.Log($"히든 스테이지 저장: {path}");
            }
        }
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log($"게임 저장 완료 - {path}");
    }

    public void Replay(bool isHidden)
    {
        eventReplay = false;
        isReplay = true;
        string path = isHidden ? HiddenPath : SavePath;
        if (!File.Exists(path))
        {
            Debug.Log("세이브 파일 x");
            return;
        }

        string json = File.ReadAllText(path);

        SaveData data = JsonUtility.FromJson<SaveData>(json);
        

        if (ResourceManager.Instance.BgmList.TryGetValue(data.musicId, out var clip))
        {
            GameManager.Instance.SetSelectedBGM(clip);
        }

        StageManager.Instance.SetStage(data.stageId);

        foreach (var b in data.timeline)
        {
            elementIds.Add(b);
        } 
        


        SceneManager.sceneLoaded += OnStageSceneLoaded;

        GameManager.Instance.LoadScene(DataManager.Instance.stageDict[data.stageId].stageName);

    }
    public void ReplayEvent()
    {
        eventReplay = true;
        isReplay = true;
        if (!File.Exists(UnlockPath))
        {
            Debug.Log("세이브 파일 x");
            return;
        }

        string json = File.ReadAllText(UnlockPath);

        EventUnlockData data = JsonUtility.FromJson<EventUnlockData>(json);

        var targetEvent = data.unlockedEvents[0];

        if (ResourceManager.Instance.SfxList.TryGetValue(targetEvent.bgmName, out var clip))
        {
            GameManager.Instance.SetSelectedBGM(clip);
        }

        StageManager.Instance.SetStage("ST001");

        elementIds.Clear();
        elementIds.Add(new TimelineSaveData
        {
            isBlock = false,
            id = targetEvent.id
        });

        SceneManager.sceneLoaded += OnStageSceneLoaded;
        GameManager.Instance.LoadScene("Stage_Scene");
    }
    public void Retry(string id)
    {
        if (DataManager.Instance.stageDict.TryGetValue(id, out var stage))
        {
            //임시로 스테이지 지정
            SceneManager.LoadScene("Stage_Scene");

        }
    }

    public void DeleteData()
    {
        if (!string.IsNullOrEmpty(SavePath) && File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("세이브 파일 삭제");
        }

    }

    private void OnStageSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Stage_Scene")
        {
            TimelineManager.Instance.LoadBlocks(elementIds);
            RhythmManager.Instance.OnStart?.Invoke();

            StartCoroutine(DelayInit());
            SceneManager.sceneLoaded -= OnStageSceneLoaded;
        }
    }

    private IEnumerator DelayInit()
    {
        yield return null;

        for (int i = 0; i < TimelineManager.Instance.PlacedBlocks.Count; i++)
        {
            RhythmManager.Instance.rhythmActions.Add(TimelineManager.Instance.PlacedBlocks[i].GetComponent<IRhythmActions>());

        }
        RhythmManager.Instance.StartMusic();
    }

    public void LoadUnlockedEvents()
    {
        if (File.Exists(UnlockPath))
        {
            string json = File.ReadAllText(UnlockPath);
            unlockData = JsonUtility.FromJson<EventUnlockData>(json);
        }
        else unlockData = new EventUnlockData();
    }

    public void SaveEvent()
    {
        string json = JsonUtility.ToJson(unlockData, true);
        File.WriteAllText(UnlockPath, json);
    }

    public void UnlockEvent(EventSaveData data)
    {
        if (unlockData.unlockedEvents.Any(e => e.id == data.id)) return;

        data.isCollect = true;
        unlockData.unlockedEvents.Add(data);
        SaveEvent();
    }

    public List<EventSaveData> GetUnlockEvents()
    {
        if (unlockData == null) LoadUnlockedEvents();
        return unlockData.unlockedEvents;
    }
}