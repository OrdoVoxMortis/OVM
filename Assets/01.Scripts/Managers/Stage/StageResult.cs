using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Analytics;
using UnityEngine;


public class StageResult : MonoBehaviour
{
    public string id;

    List<DataTable.BlockData> blocks = new(); // 배치된 블럭
    List<Block> useBlocks = new(); // 사용한 블럭
    List<DataTable.EventData> events = new(); // 배치된 이벤트
    List<Event> useEvents = new(); // 사용한 이벤트

    [HideInInspector] public string missionDialog; // 출력된 대사
    [HideInInspector] public string planDialog; // 출력된 대사
    [HideInInspector] public string eventDialog; // 출력된 대사
    [HideInInspector] public string rhythmDialog; // 출력된 대사

    public bool GhostCheck {  get; set; }
    public bool QteCheck {  get; set; }

    private void Start()
    {
        GameManager.Instance.OnGameClear += GameClear;
        StageManager.Instance.SetStage(id);
        StageManager.Instance.SetStageResult(this);
        if(blocks.Count != 0) GhostCheck = true;
        if(events.Count != 0) QteCheck = true;

        //if (DataManager.Instance.stageDict[id].stageType == StageType.Hidden) 
        //{
        //    var sendEvent = new CustomEvent("hidden_stage_entered")
        //    {
        //        ["stage_id"] = id
        //    };
        //    AnalyticsService.Instance.RecordEvent(sendEvent);
        //}

    }

    public void GameClear()
    {
        Init();
        SetResult();
    }

    void Init()
    {
        useBlocks = TimelineManager.Instance.ReturnBlocks();
        
        var blockIds = DataManager.Instance.stageDict[id].blockId;
        foreach (int blockId in blockIds)
        {
            if(DataManager.Instance.blockDict.TryGetValue(blockId, out var block))
            {
                blocks.Add(block);
            }
        }

        useEvents = TimelineManager.Instance.ReturnEvents();

        var eventIds = DataManager.Instance.stageDict[id].eventId;
        foreach(int id in eventIds)
        {
            if(DataManager.Instance.eventDict.TryGetValue(id, out var eventData))
            {
                events.Add(eventData);
            }
        }
        
        if (useBlocks == null) Debug.Log("blocks null");
    }

    void SetResult()
    {
        string missionKey = MissionResult();
        string planKey = PlanResult();
        string eventKey = EventResult();
        string rhythmKey = RhythmResult();

        missionDialog = !string.IsNullOrEmpty(missionKey)
            ? DataManager.Instance.dialogDict[DataManager.Instance.resultDict[missionKey].dialog].Dialog
            : string.Empty;

        planDialog = !string.IsNullOrEmpty(planKey)
            ? DataManager.Instance.dialogDict[DataManager.Instance.resultDict[planKey].dialog].Dialog
            : string.Empty;

        eventDialog = !string.IsNullOrEmpty(eventKey)
            ? DataManager.Instance.dialogDict[DataManager.Instance.resultDict[eventKey].dialog].Dialog
            : string.Empty;

        rhythmDialog = !string.IsNullOrEmpty(rhythmKey)
            ? DataManager.Instance.dialogDict[DataManager.Instance.resultDict[rhythmKey].dialog].Dialog
            : string.Empty;
    }

    public string MissionResult()
    {
        if (useBlocks != null && useBlocks.Count > 0)
        {
            if (useBlocks.OfType<ContactBlock>().Any(block => block.IsDeath))
            {
                return "R000";
            }
            return "R001";
        }
        return "R001";
    }

    public string PlanResult()
    {
        if (blocks.Count != 0 && useBlocks.TrueForAll(b => b.IsSuccess)) // 실패 판정 없음
        {
            if (useBlocks.Count == blocks.Count) // 모든 블럭 사용
            {
                if (useEvents.Count == events.Count) return "R004"; // 모든 이벤트 사용
                else return "R003";
            }
            else if (useBlocks.Count > 0) // 블럭 사용
            {
                if (useEvents.Count > 0) return "R006"; // 이벤트 사용
                else return "R005";
            }
        }
        else if (TimelineManager.Instance.blockTime >= 60f) return "R007";
        return string.Empty;
    }

    public string EventResult()
    {
        if (useBlocks.Count == 0 && useEvents.Count > 0) return "R008";
        if (useEvents.Count == 0) return "R009";
        return string.Empty;
    }

    public string RhythmResult()
    {
        if (GhostCheck && QteCheck) return "R010";
        else if (QteCheck) return "R011";
        return string.Empty ;
    }
}
