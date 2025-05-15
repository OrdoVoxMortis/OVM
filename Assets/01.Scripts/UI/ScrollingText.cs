using UnityEngine;
using TMPro;
using System.Collections;

public class ScrollingText : MonoBehaviour
{
    [Header("이동 속도")]
    [SerializeField]
    private float moveSpeed = 0.1f;

    public RectTransform imageRectTransform;
    public RectTransform textRectTransform;
    public TMP_Text text;

    private Vector2 starPos;
    private Vector2 direction;
    private Vector2 endPos;

    private IEnumerator MoveTextCor;

    private void Start()
    {
        MoveTextCor = MoveText();
    }

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        StopCoroutine(MoveTextCor);
    }

    private void Init()
    {
        if (text)
        {
            text.enableWordWrapping = false;
            text.text = text.text.Replace('\n', ' ');
            text.autoSizeTextContainer = true;
        }

        Invoke("SettingPos", .1f);
    }

    private void SettingPos()
    {
        float imageRectHalf = imageRectTransform.rect.width / 2;
        float textRectHalf = textRectTransform.rect.width / 2;
        float fixY = textRectTransform.anchoredPosition.y;
        starPos = new Vector2(textRectHalf  +  imageRectHalf, fixY);
        endPos = new Vector2(-(textRectHalf + imageRectHalf),fixY);
        direction = (endPos - starPos).normalized;

        textRectTransform.anchoredPosition = starPos;

        StartCoroutine(MoveTextCor);
    }

    private IEnumerator MoveText()
    {
        while(true)
        {
            textRectTransform.Translate(direction * moveSpeed * Time.deltaTime);
            if (IsEndPos())
            {
                textRectTransform.anchoredPosition = starPos;
            }
            yield return null;
        }
    }

    private bool IsEndPos()
    {
        return textRectTransform.anchoredPosition.x < endPos.x;
    }

}