using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ghost : MonoBehaviour
{

    public bool isPointNotes = false;
    private float[] judges = new float[3] { 0.2f, 0.3f, 0.4f }; //perfect, good, miss, 0.4이후론 fail
    public float animTime;
    

    public bool isOverGood;

    public void CheckGhost(float timing)
    {
        StageManager.Instance.StageResult.GhostCheck = true;
        timing = Mathf.Abs(timing);

        if (timing < judges[0])
        {
            Debug.Log("Perfect!");
            RhythmManager.Instance.checkJudgeText.text = "<b> Perfect </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.blue;
            isOverGood = true;
        }
        else if (timing < judges[1])
        {
            Debug.Log("Good!");
            RhythmManager.Instance.checkJudgeText.text = "<b> Good </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.green;
            isOverGood = true;
            StageManager.Instance.StageResult.GhostCheck = false;
        }
        else if (timing < judges[2])
        {
            Debug.Log("Miss!");
            RhythmManager.Instance.checkJudgeText.text = "<b> Miss </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.yellow;
            isOverGood = false;
            StageManager.Instance.StageResult.GhostCheck = false;
        }
        else
        {
            Debug.Log("Fail!");
            RhythmManager.Instance.checkJudgeText.text = "<b> Fail </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.red;
            isOverGood = false;
            StageManager.Instance.StageResult.GhostCheck = false;
        }

        StopAllCoroutines(); // 이전 판정 텍스트 제거 루틴이 있다면 중지
        StartCoroutine(HideJudgeTextAfterDelay(0.2f));
    }
    IEnumerator HideJudgeTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RhythmManager.Instance.checkJudgeText.text = "";
    }
}
