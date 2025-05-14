using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class QTELong : QTE
{
    public float innerCircleSize;
    public RectTransform innerCircle;

    public float holdingTime;
    private float holdingDelta;

    private bool isHolding;

    void Start()
    {
        outerLineSize = 2.0f;
        outerLine.localScale = new Vector2(2.0f, 2.0f);

        if (holdingTime == 0f)
            holdingTime = 1f;

        holdingDelta = 1f / holdingTime;
        isHolding = false;
    }

    void Update()
    {
        if (isChecked)
            return;

        if (outerLineSize <= 1 - judges[2]) //누르는 거 자체를 놓친 경우
        {
            manager.CheckQTE();
            //CheckJudge();
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
        }

        innerImage.color = gradient.Evaluate(1 - Mathf.Abs(1 - outerLineSize));
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
                isOverGood = false;
                StageManager.Instance.StageResult.QteCheck = false;
            }
            else
            {
                RhythmManager.Instance.checkJudgeText.text = "<b> Fail </b>";
                RhythmManager.Instance.checkJudgeText.color = Color.red;
                isOverGood = false;
                StageManager.Instance.StageResult.QteCheck = false;
            }
        }

        isHolding = true;
    }

    /*
    public void CheckJudge()
    {
        StageManager.Instance.StageResult.QteCheck = true;
        float timing = Mathf.Abs(1f - outerLineSize);
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
            StageManager.Instance.StageResult.QteCheck = false;
        }
        else if (timing < judges[2])
        {
            Debug.Log("Miss!");
            RhythmManager.Instance.checkJudgeText.text = "<b> Miss </b>";
            RhythmManager.Instance.checkJudgeText.color = Color.yellow;
            isOverGood = false;
            StageManager.Instance.StageResult.QteCheck = false;
        }
        else
        {
            Debug.Log("Fail!");
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

        //innerImage.gameObject.SetActive(false);
        outerLine.gameObject.SetActive(false);

        Invoke("DestroyObject", 0.5f);

    }
    */

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
