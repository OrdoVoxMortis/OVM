using DataTable;
using GoogleSheet.Core.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[UGS(typeof(StageType))]
public enum StageType
{
    Normal,
    Hidden
}

public class StageManager : SingleTon<StageManager>
{
    public StageData CurrentStageData { get; private set; }
    public float PlayTime { get; private set; } = 0f;
    public StageResult StageResult { get; private set; }

    protected override void Awake()
    {
        PlayTime = 0f;
    }

    private void Update()
    {
        PlayTime += Time.deltaTime;
    }

    public void SetStage(string id)
    {
        if (DataManager.Instance.stageDict.TryGetValue(id, out var stage))
        {
            CurrentStageData = stage;
        }
    }

    public void SetStageResult(StageResult result)
    {
        StageResult = result;
    }


    
}
