using DataTable;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UGS;
using UnityEngine;


public class DataManager : SingleTon<DataManager>
{
    public Dictionary<int, NpcData> npcDict { get; private set; } = new();
    public Dictionary<int, BlockData> blockDict { get; private set; } = new();
    //public Dictionary<int, StageData> stageDict { get; private set; } = new();
    //public Dictionary<int, MapData> mapDict { get; private set; } = new();
    //public Dictionary<int, MissonData> missonDict { get; private set; } = new();
    //public Dictionary<int, EventData> eventDict { get; private set; } = new();
    //public Dictionary<int, MusicData> musicDict { get; private set; } = new();
 
    protected override void Awake()
    {
        base.Awake();
        UnityGoogleSheet.LoadAllData();
    }

    private void Start()
    {
        npcDict = NpcData.GetDictionary();
        blockDict = BlockData.GetDictionary();
        //stageDict = StageData.GetDictionary();
        //mapDict = MapData.GetDictionary();
        //missonDict = MissonData.GetDictionary();
        //eventDict = EventData.GetDictionary();
        //musicDict = MusicData.GetDictionary();
    }
}
