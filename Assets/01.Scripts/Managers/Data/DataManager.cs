using DataTable;
using System.Collections.Generic;
using UGS;
using UnityEngine;


public class DataManager : SingleTon<DataManager>
{
    public Dictionary<string, NpcData> npcDict { get; private set; } = new();
    public Dictionary<int, BlockData> blockDict { get; private set; } = new();
    public Dictionary<string, StageData> stageDict { get; private set; } = new();
    public Dictionary<string, MapData> mapDict { get; private set; } = new();
    public Dictionary<string, MissionData> missionDict { get; private set; } = new();
    public Dictionary<int, EventData> eventDict { get; private set; } = new();
    public Dictionary<string, MusicData> musicDict { get; private set; } = new();
    public Dictionary<string, SuspicionData> suspicionDict { get; private set; } = new();
    public Dictionary<string, NpcTypeData> npcTypeDict { get; private set; } = new();
    public Dictionary<string, ResultData> resultDict { get; private set; } = new();
    public Dictionary<string, DialogData> dialogDict { get; private set; } = new();
    public HashSet<string> interactMissionNoteIds { get; private set; } = new(); // 상호작용한 편지 오브젝트 
    protected override void Awake()
    {
        base.Awake();
        UnityGoogleSheet.LoadAllData();
        InitData();
    }

    private void InitData()
    {
        npcDict = NpcData.GetDictionary();
        npcTypeDict = NpcTypeData.GetDictionary();
        blockDict = BlockData.GetDictionary();
        stageDict = StageData.GetDictionary();
        mapDict = MapData.GetDictionary();
        missionDict = MissionData.GetDictionary();
        eventDict = EventData.GetDictionary();
        musicDict = MusicData.GetDictionary();
        suspicionDict = SuspicionData.GetDictionary();
        resultDict = ResultData.GetDictionary();
        dialogDict = DialogData.GetDictionary();
    }

    public void OnInteractMissionNote(string id)
    {
        if (interactMissionNoteIds.Count != 0) interactMissionNoteIds.Clear();
        interactMissionNoteIds.Add(id);
    }

    public bool IsMissionNoteOnInteract(string id)
    {
        return interactMissionNoteIds.Contains(id);
    }

}
