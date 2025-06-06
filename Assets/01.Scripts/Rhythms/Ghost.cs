using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [HideInInspector]
    public int ghostIndex;
    [HideInInspector]
    public bool isPointNotes = false;
    private float[] judges = new float[3] { 0.2f, 0.3f, 0.4f }; //perfect, good, miss, 0.4이후론 fail
    [HideInInspector]
    public float animTime;

    [HideInInspector]
    public bool isOverGood;

    public void CheckGhost(float timing)
    {
        StageManager.Instance.StageResult.GhostCheck = true;
        timing = Mathf.Abs(timing);

        string judgeText;

        if (timing < judges[0])
        {
            //Debug.Log("Perfect!");
            judgeText = "perfect";
            RhythmManager.Instance.checkJudgeText.text = "<b> Perfect </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.blue;
            isOverGood = true;
        }
        else if (timing < judges[1])
        {
            //Debug.Log("Good!");
            judgeText = "good";
            RhythmManager.Instance.checkJudgeText.text = "<b> Good </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.green;
            isOverGood = true;
            StageManager.Instance.StageResult.GhostCheck = false;
        }
        else if (timing < judges[2])
        {
            //Debug.Log("Miss!");
            judgeText = "miss";
            RhythmManager.Instance.checkJudgeText.text = "<b> Miss </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.yellow;
            isOverGood = false;
            StageManager.Instance.StageResult.GhostCheck = false;
        }
        else
        {
            //Debug.Log("Fail!");
            judgeText = "fail";
            RhythmManager.Instance.checkJudgeText.text = "<b> Fail </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.red;
            isOverGood = false;
            StageManager.Instance.StageResult.GhostCheck = false;
        }

        var sendEvent = new CustomEvent("rhythm_judged")
        {
            ["judgment_result"] = judgeText,
            ["judgment_result_index"] = ghostIndex
        };

        AnalyticsService.Instance.RecordEvent(sendEvent);

        StopAllCoroutines(); // 이전 판정 텍스트 제거 루틴이 있다면 중지
        StartCoroutine(HideJudgeTextAfterDelay(0.2f));
    }
    IEnumerator HideJudgeTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RhythmManager.Instance.checkJudgeText.text = "";
    }
}
