using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string stageId;
    public List<Block> blocks;
    public List<Event> events;
}

public class SaveManager : SingleTon<SaveManager>
{
    private string SavePath => $"{Application.persistentDataPath}/save.json";
    //TODO. 스테이지 정보 저장&로드
    public void SaveGame()
    {
        SaveData data = new SaveData();

        data.stageId = GameManager.Instance.stageResult.id;

        data.blocks = new List<Block>();
        foreach(var block in TimelineManager.Instance.PlacedBlocks)
        {
            data.blocks.Add(block);
        }

        data.events = new List<Event>();
        foreach(var e in TimelineManager.Instance.eventslots)
        {
            data.events.Add(e.eventBlock);
        }

        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(SavePath, json);
        
        Debug.Log($"게임 저장 완료 - {SavePath}");
    }

    public void LoadGame()
    {
        if (!System.IO.File.Exists(SavePath))
        {
            Debug.Log("세이브 파일 x");
            return;
        }
        
        string json = System.IO.File.ReadAllText(SavePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        foreach (var block in data.blocks)
        {
            TimelineManager.Instance.AddBlock(block);
        }

        foreach(var e in data.events)
        {
            TimelineManager.Instance.AddEventSlot(e);
        }
    }
}
