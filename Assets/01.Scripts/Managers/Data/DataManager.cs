using DataTable;
using System.Collections.Generic;
using UGS;


public class DataManager : SingleTon<DataManager>
{
    public Dictionary<int, NpcData> npcDict { get; private set; } = new();
    public Dictionary<int, BlockData> blockDict { get; private set; } = new();
    protected override void Awake()
    {
        base.Awake();
        UnityGoogleSheet.LoadAllData();
    }

    private void Start()
    {
        npcDict = NpcData.GetDictionary();
        blockDict = BlockData.GetDictionary();
    }
}
