using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTE : MonoBehaviour
{
    public QTEManager manager;
    public float outerLineSize;
    public RectTransform outerLine;
    public Image innerImage;

    public Gradient gradient;

    public GameObject particle;

    private bool isChecked = false;
    public bool isPointNotes = false;

    public bool isOverGood;

    private float[] judges = new float[3] { 0.2f, 0.3f, 0.4f }; //perfect, good, miss, 0.4이후론 fail

    // Start is called before the first frame update
    void Start()
    {
        int width = Random.Range(Screen.width / 4, Screen.width * 3 / 4);
        int height = Random.Range(Screen.height / 4, Screen.height * 3 / 4);

        transform.position = new Vector2(width, height);

        outerLineSize = 2.0f;
        outerLine.localScale = new Vector2(2.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isChecked)
            return;
        
        outerLineSize -= Time.deltaTime;

        if(outerLineSize <= 1 - judges[2])
        {
            manager.CheckQTE();
            CheckJudge();
        }

        outerLine.localScale = new Vector2(outerLineSize, outerLineSize);
        innerImage.color = gradient.Evaluate(1 - Mathf.Abs(1 - outerLineSize));
    }

    public void CheckJudge()
    {
        StageManager.Instance.StageResult.QteCheck = true;
        float timing = Mathf.Abs(1f - outerLineSize);
        if (timing < judges[0])
        {
            Debug.Log("Perfect!");
            isOverGood = true;
        }
        else if (timing < judges[1])
        {
            Debug.Log("Good!");
            isOverGood = true;
            StageManager.Instance.StageResult.QteCheck = false;
        }
        else if (timing < judges[2])
        {
            Debug.Log("Miss!");
            isOverGood = false;
            StageManager.Instance.StageResult.QteCheck = false;
        } 
        else
        {
            Debug.Log("Fail!");
            isOverGood = false;
            StageManager.Instance.StageResult.QteCheck = false;
        }
        isChecked = true;

        if (timing < judges[1]) //Good 이상인 경우 파티클
            particle.SetActive(true);

        innerImage.gameObject.SetActive(false);
        outerLine.gameObject.SetActive(false);

        Invoke("DestroyObject", 0.5f);
        
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
