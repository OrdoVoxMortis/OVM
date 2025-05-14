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
        innerCircleSize = 0f;
        innerCircle.localScale = new Vector2(0f, 0f);
        holdingDelta = 1f / holdingTime;
        isHolding = false;
    }

    void Update()
    {
        if (isChecked)
            return;

        if (!isHolding && outerLineSize <= 1 - judges[2]) //누르는 거 자체를 놓친 경우
        {
            manager.CheckQTE();
        }

        if(isHolding && innerCircleSize >= 1f)
        {
            //삭제
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
            innerImage.gameObject.SetActive(false);
            outerLine.gameObject.SetActive(false);
            innerCircle.gameObject.SetActive(false);
            isChecked = true;
            isHolding = false;
            Invoke("DestroyObject", 0.5f);
        } 
        else //성공시
        {
            if (timing < judges[0])
            {
                Debug.Log("Perfect!");
                RhythmManager.Instance.checkJudgeText.text = "<b> Perfect </b>";
                RhythmManager.Instance.checkJudgeText.color = Color.blue;
                isOverGood = true;
            } 
            else
            {
                Debug.Log("Good!");
                RhythmManager.Instance.checkJudgeText.text = "<b> Good </b>";
                RhythmManager.Instance.checkJudgeText.color = Color.green;
                isOverGood = true;
                StageManager.Instance.StageResult.QteCheck = true;
            }
            outerLine.localScale = new Vector2(1f, 1f);
            isHolding = true;
        }

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
