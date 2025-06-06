using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class QTELong : QTE
{
    public float innerCircleSize;
    public RectTransform innerCircle;

    public GameObject judgeCircle;

    [HideInInspector]
    public float holdingTime;

    private float holdingDelta;

    [HideInInspector]
    public bool isHolding;

    [HideInInspector]
    public List<float> holdingCheckTime;
    private int curTimeIndex;
    private float checkTime;


    void Start()
    {
        curTimeIndex = 0;
        outerLineSize = 2.0f;
        outerLine.localScale = new Vector2(2.0f, 2.0f);

        if (holdingTime == 0f)
            holdingTime = 1f;
        innerCircleSize = 0f;
        innerCircle.localScale = new Vector2(0f, 0f);
        holdingDelta = 1f / holdingTime;
        isHolding = false;
        
    }

    void Update()
    {
        if (isChecked)
            return;

        if (isHolding)
        {
            checkTime += Time.deltaTime;
            if(curTimeIndex < holdingCheckTime.Count &&checkTime > holdingCheckTime[curTimeIndex])
            {
                if (curTimeIndex == holdingCheckTime.Count - 1)
                {
                    manager.isHolding = false;
                }
                if (manager.qteList.Count > 0 && manager.qteList[0] == this)
                    manager.CheckQTE();

                curTimeIndex++;
            }
        }

        if (!isHolding && outerLineSize <= 1 - judges[2]) //누르는 거 자체를 놓친 경우
        {
            manager.CheckQTE();
        }

        if(isHolding && innerCircleSize >= 1f)
        {
            //삭제
            isHolding = false;
            manager.isHolding = false;
            if (manager.qteList.Count > 0 && manager.qteList[0] == this)
                manager.qteList.RemoveAt(0);
            Destroy(gameObject, 0.01f);
        }

        if (!isHolding) //바깥원 줄어듦
        {
            outerLineSize -= Time.deltaTime;
            outerLine.localScale = new Vector2(outerLineSize, outerLineSize);
        }

        if (isHolding) //안쪽원 채워짐
        {
            innerCircleSize += holdingDelta * Time.deltaTime;
            innerCircle.localScale = new Vector2(innerCircleSize, innerCircleSize);
            innerImage.color = gradient.Evaluate(Mathf.Abs(innerCircleSize));
        }

    }

    public override void CheckJudge()
    {
        if (manager.isOverGood)
            ParticlePlay();

        if (isHolding)
            return;
        float timing = Mathf.Abs(1f - outerLineSize);
        string judgeText;

        if (timing > judges[1]) //실패시
        {
            if (timing < judges[2])
            {
                judgeText = "miss";
                RhythmManager.Instance.checkJudgeText.text = "<b> Miss </b>";
                RhythmManager.Instance.checkJudgeText.color = Color.yellow;
                manager.isOverGood = false;
                StageManager.Instance.StageResult.QteCheck = false;
            }
            else
            {
                judgeText = "fail";
                RhythmManager.Instance.checkJudgeText.text = "<b> Fail </b>";
                RhythmManager.Instance.checkJudgeText.color = Color.red;
                manager.isOverGood = false;
                StageManager.Instance.StageResult.QteCheck = false;
            }
            innerImage.gameObject.SetActive(false);
            outerLine.gameObject.SetActive(false);
            innerCircle.gameObject.SetActive(false);
            judgeCircle.SetActive(false);
            isChecked = true;
            isHolding = false;
            manager.isHolding = false;
            Invoke("DestroyObject", 0.5f);
        } 
        else //성공시
        {
            if (timing < judges[0])
            {
                judgeText = "perfect";
                //Debug.Log("Perfect!");
                RhythmManager.Instance.checkJudgeText.text = "<b> Perfect </b>";
                RhythmManager.Instance.checkJudgeText.color = Color.blue;
            } 
            else
            {
                judgeText = "good";
                //Debug.Log("Good!");
                RhythmManager.Instance.checkJudgeText.text = "<b> Good </b>";
                RhythmManager.Instance.checkJudgeText.color = Color.green;
                StageManager.Instance.StageResult.QteCheck = false;
            }
            manager.isOverGood = true;
            outerLine.localScale = new Vector2(1f, 1f);
            isHolding = true;
            manager.isHolding = true;
            manager.isLongNoteDoing = true;
            checkTime = 0f;
            ParticlePlay();
        }

        var sendEvent = new CustomEvent("rhythm_judged")
        {
            ["judgment_result"] = judgeText,
            ["judgment_result_index"] = qteIndex
        };

        //AnalyticsService.Instance.RecordEvent(sendEvent);

        StopAllCoroutines();
        StartCoroutine(HideJudgeTextAfterDelay(0.2f));
    }

    public void ReleaseNote()
    {
        if (holdingCheckTime[holdingCheckTime.Count - 1] - checkTime > 0.2f 
            || holdingCheckTime[holdingCheckTime.Count - 1] - checkTime < -0.2f)
            manager.isOverGood = false;

        if (manager.qteList.Count > 0 && manager.qteList[0] == this)
        {
            if (holdingCheckTime[holdingCheckTime.Count - 1] - checkTime < 1.0f)
                manager.CheckQTE();
            if(manager.qteList.Count > 0 && manager.qteList[0] == this)
                manager.qteList.RemoveAt(0);
        }

        isHolding = false;
        isChecked = true;
        manager.isHolding = false;

        innerImage.gameObject.SetActive(false);
        outerLine.gameObject.SetActive(false);
        innerCircle.gameObject.SetActive(false);
        judgeCircle.SetActive(false);

        
        Destroy(gameObject, 0.5f);
    }
    

    IEnumerator HideJudgeTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RhythmManager.Instance.checkJudgeText.text = "";
    }

    private void ParticlePlay()
    {
        if (!particle.activeSelf)
            particle.SetActive(true);

        particle.GetComponent<ParticleSystem>().Play();
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
