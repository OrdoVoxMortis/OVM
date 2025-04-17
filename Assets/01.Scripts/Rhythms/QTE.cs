using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTE : MonoBehaviour
{
    public float outerLineSize;
    public RectTransform outerLine;
    public Image innerImage;

    public Gradient gradient;

    public GameObject particle;

    private bool isChecked = false;
    public bool isPointNotes = false;

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

        if(outerLineSize <= 0.6f)
        {
            RhythmManager.Instance.qteManager.CheckQTE();
            CheckJudge();
            Invoke("DestroyObject", 0.5f);
        }

        outerLine.localScale = new Vector2(outerLineSize, outerLineSize);
        innerImage.color = gradient.Evaluate(1 - Mathf.Abs(1 - outerLineSize));
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
        isChecked = true;

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
