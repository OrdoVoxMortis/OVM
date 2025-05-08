using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public string eventName;
    public string imageName;
    public bool isCollect;
}

[System.Serializable]
public class TimelineSaveData
{
    public bool isBlock;
    public int id;
}

public class SaveManager : SingleTon<SaveManager>
{
    private string SavePath => $"{Application.persistentDataPath}/save.json";
    private List<TimelineSaveData> elementIds = new();
    public bool isReplay;
    public bool eventReplay;
    //TODO. 스테이지 정보 저장&로드
    public void SaveGame()
    {
        SaveData data = new SaveData();

        data.blocks = new List<BlockSaveData>();
        data.events = new List<EventSaveData>();
        data.timeline = new List<TimelineSaveData>();

        data.stageId = StageManager.Instance.StageResult.id;

        data.playTime = StageManager.Instance.PlayTime;
        ;
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
                data.events.Add(new EventSaveData
                {
                    id = e.id,
                    eventName = e.Name,
                    imageName = e.ImageName,
                    isCollect = e.IsCollect
                });
                data.timeline.Add(new TimelineSaveData
                {
                    isBlock = false,
                    id = e.id,
                });
            }
        }

        data.musicId = GameManager.Instance.SelectedBGM.name;


        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);

        Debug.Log($"게임 저장 완료 - {SavePath}");
    }

    public void Replay(bool evt)
    {
        eventReplay = evt;
        isReplay = true;
        if (!File.Exists(SavePath))
        {
            Debug.Log("세이브 파일 x");
            return;
        }

        string json = File.ReadAllText(SavePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (ResourceManager.Instance.BgmList.TryGetValue(data.musicId, out var clip))
        {
            GameManager.Instance.SetSelectedBGM(clip);
        }

        StageManager.Instance.SetStage(data.stageId);

        foreach (var b in data.timeline)
        {
            if (evt)
            {
                if (b.isBlock) continue;
            }
            elementIds.Add(b);
        } 
        


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
}