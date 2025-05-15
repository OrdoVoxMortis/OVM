using System.Collections;
using System.Collections.Generic;
using TMPro;
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
            manager.CheckQTE();
        }

        outerLine.localScale = new Vector2(outerLineSize, outerLineSize);
        innerImage.color = gradient.Evaluate(1 - Mathf.Abs(1 - outerLineSize));
    }

    public override void CheckJudge()
    {
        StageManager.Instance.StageResult.QteCheck = true;
        float timing = Mathf.Abs(1f - outerLineSize);
        
        if (timing < judges[0])
        {
            //Debug.Log("Perfect!");
            RhythmManager.Instance.checkJudgeText.text = "<b> Perfect </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.blue;
            isOverGood = true;
        }
        else if (timing < judges[1])
        {
            //Debug.Log("Good!");
            RhythmManager.Instance.checkJudgeText.text = "<b> Good </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.green;
            isOverGood = true;
            StageManager.Instance.StageResult.QteCheck = true;
        }
        else if (timing < judges[2])
        {
            //Debug.Log("Miss!");
            RhythmManager.Instance.checkJudgeText.text = "<b> Miss </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.yellow;
            isOverGood = false;
            StageManager.Instance.StageResult.QteCheck = false;
        } 
        else
        {
            //Debug.Log("Fail!");
            RhythmManager.Instance.checkJudgeText.text = "<b> Fail </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.red;
            isOverGood = false;
            StageManager.Instance.StageResult.QteCheck = false;
        }
        StopAllCoroutines();
        StartCoroutine(HideJudgeTextAfterDelay(0.2f));
        isChecked = true;
        
        if (timing < judges[1]) //Good 이상인 경우 파티클
            particle.SetActive(true);

        innerImage.gameObject.SetActive(false);
        outerLine.gameObject.SetActive(false);

        Invoke("DestroyObject", 0.5f);

    }

    IEnumerator HideJudgeTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RhythmManager.Instance.checkJudgeText.text = "";
    }


    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
