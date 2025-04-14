using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Quest : BaseUI
{
    [SerializeField] private Button backBtn;
    [SerializeField] private Button acceptBtn;
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private Image targetImage;
    [SerializeField] private TextMeshProUGUI dialogText;

    protected override void Awake()
    {
        base.Awake();

        if (backBtn != null)
            backBtn.onClick.AddListener(Hide); // 뒤로가는 버튼 할당해주기
        if (acceptBtn != null)
            acceptBtn.onClick.AddListener(OnClickAccept); // 퀘스트 수락 하는 버튼 할당해주기
    }
    private void Start()
    {
        Hide();
    }
    private void OnClickAccept()
    {
        Debug.Log("퀘스트 수락!"); // TODO 실제로 넘어가는 암살의뢰 UI 연결해주기
        Hide();
        GameManager.Instance.LoadScene("Stage_Scene");
    }

    public void SetQuest(string questDescription, Sprite targetSprite, string dialog)
    {
        // 해당 함수를 통하여, 퀘스트 UI의 필드요소들을 동적으로 만들어준다
        if (questText != null)
            questText.text = questDescription;

        if (targetImage != null)
            targetImage.sprite = targetSprite;

        if (dialogText != null)
            dialogText.text = dialog;
    }
}