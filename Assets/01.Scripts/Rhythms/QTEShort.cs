using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class QTEShort : QTE
{
    void Start()
    {
        outerLineSize = 2.0f;
        outerLine.localScale = new Vector2(2.0f, 2.0f);
    }

    void Update()
    {
        if (isChecked)
            return;
        
        outerLineSize -= Time.deltaTime;

        if(outerLineSize <= 1 - judges[2])
        {
            if (manager.qteList.Count > 0 && manager.qteList[0] == this)
            manager.CheckQTE();
        }

        outerLine.localScale = new Vector2(outerLineSize, outerLineSize);
        innerImage.color = gradient.Evaluate(1 - Mathf.Abs(1 - outerLineSize));
    }

    public override void CheckJudge()
    {
        StageManager.Instance.StageResult.QteCheck = true;
        float timing = Mathf.Abs(1f - outerLineSize);

        string judgeText;

        if (timing < judges[0])
        {
            //Debug.Log("Perfect!");
            judgeText = "perfect";
            RhythmManager.Instance.checkJudgeText.text = "<b> Perfect </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.blue;
            manager.isOverGood = true;
        }
        else if (timing < judges[1])
        {
            //Debug.Log("Good!");
            judgeText = "good";
            RhythmManager.Instance.checkJudgeText.text = "<b> Good </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.green;
            manager.isOverGood = true;
            StageManager.Instance.StageResult.QteCheck = false;
        }
        else if (timing < judges[2])
        {
            //Debug.Log("Miss!");
            judgeText = "miss";
            RhythmManager.Instance.checkJudgeText.text = "<b> Miss </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.yellow;
            manager.isOverGood = false;
            StageManager.Instance.StageResult.QteCheck = false;
        } 
        else
        {
            //Debug.Log("Fail!");
            judgeText = "fail";
            RhythmManager.Instance.checkJudgeText.text = "<b> Fail </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.red;
            manager.isOverGood = false;
            StageManager.Instance.StageResult.QteCheck = false;
        }
        StopAllCoroutines();
        StartCoroutine(HideJudgeTextAfterDelay(0.2f));
        isChecked = true;

        if (timing < judges[1]) //Good 이상인 경우 파티클
            ParticlePlay();

        innerImage.gameObject.SetActive(false);
        outerLine.gameObject.SetActive(false);

        var sendEvent = new CustomEvent("rhythm_judged")
        {
            ["judgment_result"] = judgeText,
            ["judgment_result_index"] = qteIndex
        };

        AnalyticsService.Instance.RecordEvent(sendEvent);

        Invoke("DestroyObject", 0.5f);

    }

    IEnumerator HideJudgeTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RhythmManager.Instance.checkJudgeText.text = "";
    }

    private void ParticlePlay()
    {
        if(!particle.activeSelf)
            particle.SetActive(true);

        particle.GetComponent<ParticleSystem>().Play();
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
