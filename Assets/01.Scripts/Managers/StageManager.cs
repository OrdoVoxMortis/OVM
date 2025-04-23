using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class StageManager : MonoBehaviour
{
    public string id;

    List<DataTable.BlockData> blocks = new(); // 배치된 블럭
    List<Block> useBlocks = new(); // 사용한 블럭
    List<DataTable.EventData> events = new(); // 배치된 이벤트
    List<UI_Event> useEvents = new(); // 사용한 이벤트

    string missionDialog; // 출력된 대사
    string planDialog; // 출력된 대사
    string eventDialog; // 출력된 대사
    string rhythmDialog; // 출력된 대사


    void Init()
    {
        useBlocks = TimelineManager.Instance.PlacedBlocks;
        
        var blockIds = DataManager.Instance.stageDict[id].blockId;
        foreach (int blockId in blockIds)
        {
            if(DataManager.Instance.blockDict.TryGetValue(blockId, out var block))
            {
                blocks.Add(block);
            }
        }

        useEvents = TimelineManager.Instance.eventslots;

        var eventIds = DataManager.Instance.stageDict[id].eventId;
        foreach(string id in eventIds)
        {
            if(DataManager.Instance.eventDict.TryGetValue(id, out var eventData))
            {
                events.Add(eventData);
            }
        }

    }

    void StageResult()
    {
        missionDialog = DataManager.Instance.dialogDict[DataManager.Instance.resultDict[MissionResult()].dialog].Dialog;
        planDialog = DataManager.Instance.dialogDict[DataManager.Instance.resultDict[MissionResult()].dialog].Dialog;
        eventDialog = DataManager.Instance.dialogDict[DataManager.Instance.resultDict[EventResult()].dialog].Dialog;
        rhythmDialog = DataManager.Instance.dialogDict[DataManager.Instance.resultDict[MissionResult()].dialog].Dialog;
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
        return "R002";
    }

    public string PlanResult()
    {
        if (useBlocks.TrueForAll(b => b.IsSuccess)) // 실패 판정 없음
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
        else if (GameManager.Instance.playTime >= 60f) return "R007";
        return string.Empty;
    }

    public string EventResult()
    {
        if (useBlocks.Count == 0 && useEvents.Count > 0) return "R008";
        if (useEvents.Count == 0) return "R009";
        return string.Empty;
    }
}
