using DataTable;
using GoogleSheet;
using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;

public class DataManager : SingleTon<DataManager>
{
    public Dictionary<int, NPC> npcDict { get; private set; } = new();

    protected override void Awake()
    {
        base.Awake();
        UnityGoogleSheet.LoadAllData();


    }

    private void Start()
    {
        npcDict = NPC.GetDictionary();
    }
}
