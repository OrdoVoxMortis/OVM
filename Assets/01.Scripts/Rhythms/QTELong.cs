using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTELong : QTE
{
    public float innerCircleSize;
    public RectTransform innerCircle;

    public float holdingTime;
    private float holdingDelta;

    public bool isHolding;
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
            Destroy(gameObject);
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
        if (isHolding)
            return;
        float timing = Mathf.Abs(1f - outerLineSize);
        if (timing > judges[1]) //실패시
        {
            if (timing < judges[2])
            {
                RhythmManager.Instance.checkJudgeText.text = "<b> Miss </b>";
                RhythmManager.Instance.checkJudgeText.color = Color.yellow;
                manager.isOverGood = false;
                StageManager.Instance.StageResult.QteCheck = false;
            }
            else
            {
                RhythmManager.Instance.checkJudgeText.text = "<b> Fail </b>";
                RhythmManager.Instance.checkJudgeText.color = Color.red;
                manager.isOverGood = false;
                StageManager.Instance.StageResult.QteCheck = false;
            }
            innerImage.gameObject.SetActive(false);
            outerLine.gameObject.SetActive(false);
            innerCircle.gameObject.SetActive(false);
            isChecked = true;
            isHolding = false;
            manager.isHolding = false;
            Invoke("DestroyObject", 0.5f);
        } 
        else //성공시
        {
            if (timing < judges[0])
            {
                Debug.Log("Perfect!");
                RhythmManager.Instance.checkJudgeText.text = "<b> Perfect </b>";
                RhythmManager.Instance.checkJudgeText.color = Color.blue;
            } 
            else
            {
                Debug.Log("Good!");
                RhythmManager.Instance.checkJudgeText.text = "<b> Good </b>";
                RhythmManager.Instance.checkJudgeText.color = Color.green;
                StageManager.Instance.StageResult.QteCheck = true;
            }
            manager.isOverGood = true;
            outerLine.localScale = new Vector2(1f, 1f);
            isHolding = true;
            manager.isHolding = true;
            manager.isLongNoteDoing = true;
            checkTime = 0f;
        }
        StopAllCoroutines();
        StartCoroutine(HideJudgeTextAfterDelay(0.2f));
    }

    public void ReleaseNote()
    {
        isHolding = false;
        isChecked = true;
        manager.isHolding = false;
        manager.isOverGood = false;
        if(manager.qteList.Count > 0 &&  manager.qteList[0] == this)
            manager.qteList.RemoveAt(0);
        Destroy(gameObject);
        //manager.CheckQTE();
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
