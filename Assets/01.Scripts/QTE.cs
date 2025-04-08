using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTE : MonoBehaviour
{
    public float outerLineSize;
    public RectTransform outerLine;

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
        outerLineSize -= Time.deltaTime;

        if(outerLineSize <= 0.6f)
        {
            QTEManager.Instance.CheckQTE();
            Destroy(gameObject);
        }

        outerLine.localScale = new Vector2(outerLineSize, outerLineSize);
    }

    public void CheckJudge()
    {
        if (outerLineSize < 1.2f && 0.8f < outerLineSize)
        {
            Debug.Log("완벽!");
        }
        else
        {
            Debug.Log("이런!");
        }

        Destroy(gameObject);
    }
}
