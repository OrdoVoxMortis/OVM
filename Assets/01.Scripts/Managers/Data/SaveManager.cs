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
    //public List<Event> events;
}

[System.Serializable]
public class BlockSaveData
{
    public int id;
    public string blockName; 
}

public class SaveManager : SingleTon<SaveManager>
{
    private string SavePath => $"{Application.persistentDataPath}/save.json";
    private List<int> blockIds = new();
    public bool isReplay;
    //TODO. 스테이지 정보 저장&로드
    public void SaveGame()
    {
        SaveData data = new SaveData();

        data.stageId = StageManager.Instance.StageResult.id;
        
        data.playTime = StageManager.Instance.PlayTime;

        data.blocks = TimelineManager.Instance.PlacedBlocks.Where(b => b != null).Select(b=> new BlockSaveData
        {
            id = b.id,
            blockName = b.BlockName

        }).ToList();

        data.musicId = GameManager.Instance.SelectedBGM.name;

        //data.events = new List<Event>();
        //foreach (var e in TimelineManager.Instance.eventslots)
        //{
        //    data.events.Add(e.eventBlock);
        //}

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);

        Debug.Log($"게임 저장 완료 - {SavePath}");
    }

    public void Replay()
    {
        isReplay = true;
        if (!File.Exists(SavePath))
        {
            Debug.Log("세이브 파일 x");
            return;
        }

        string json = File.ReadAllText(SavePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        //if(ResourceManager.Instance.BgmList.TryGetValue(data.musicId, out var clip))
        //{
         //   GameManager.Instance.SetSelectedBGM(clip);
        //}

        StageManager.Instance.SetStage(data.stageId);

        foreach (var b in data.blocks)
        {
            blockIds.Add(b.id);
        }

        SceneManager.sceneLoaded += OnStageSceneLoaded;

        GameManager.Instance.LoadScene("Stage_Scene");
        //foreach (var e in data.events)
        //{
        //    TimelineManager.Instance.AddEventSlot(e);
        //}

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
            TimelineManager.Instance.LoadBlocks(blockIds);
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