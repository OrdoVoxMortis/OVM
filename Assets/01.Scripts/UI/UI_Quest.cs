using TMPro;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class UI_Quest : BaseUI
{
    [SerializeField] private Button backBtn;
    [SerializeField] private Button acceptBtn;
    [SerializeField] private string currentQuestId;
    [SerializeField] private string sceneNameToLoad;
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private Image targetImage;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private GameObject textBox;

    private string fullDialog; // 실제 대사 저장
    private bool isDialogShown = false; // 대사가 출력되었는지?

    protected override void Awake()
    {
        base.Awake();

        if (backBtn != null)
            backBtn.onClick.AddListener(Hide); // 뒤로가는 버튼 할당해주기
        if (acceptBtn != null)
            acceptBtn.onClick.AddListener(QuestAcceptable); // 퀘스트 수락 하는 버튼 할당해주기
    }
    private void Start()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        textBox.SetActive(false); // 대화창도 기본적으로 비활성화
    }
    private void Update()
    {
        acceptBtn.interactable = true;
    }
    private void OnClickAccept()
    {
        if (!isDialogShown) 
        {
            Debug.Log("퀘스트 수락!"); // TODO 실제로 넘어가는 암살의뢰 UI 연결해주기
            Hide();
            //var sendEvent = new CustomEvent("mission_accepted")
            //{
            //    ["stage_id"] = currentQuestId
            //};
            //AnalyticsService.Instance.RecordEvent(sendEvent);
            LoadSceneManager.Instance.LoadSceneWithLoading(sceneNameToLoad);
            //var sendEventstage = new CustomEvent("stage_entered")
            //{
            //    ["stage_id"] = currentQuestId
            //};
            //AnalyticsService.Instance.RecordEvent(sendEventstage);
        }
    }

    private void QuestAcceptable() // 의뢰 대사 내용이 할당되어 있지 않으면 의뢰 씬으로 넘어가게 해줌
    {
        if (string.IsNullOrEmpty(fullDialog))
        {
            // 대사가 없으면 바로 씬 이동
            OnClickAccept();
        }
        else if (!isDialogShown)
        {
            textBox.SetActive(true);
            dialogText.text = fullDialog;
            isDialogShown = true;
            acceptBtn.interactable = false;
        }
        else if (isDialogShown) 
        {
            textBox.SetActive(false);
            isDialogShown= false;
        }
    }

    public void SetQuest(string id, string questDescription, Sprite targetSprite, string dialog, string sceneName)
    {
        // 해당 함수를 통하여, 퀘스트 UI의 필드요소들을 동적으로 만들어준다
        currentQuestId = id; // 스테이지 id
        sceneNameToLoad = sceneName; // 씬 이름 값

        if (questText != null)
            questText.text = questDescription;

        if (targetImage != null)
            targetImage.sprite = targetSprite;

        if (dialogText != null)
        {
            fullDialog = dialog; 
            isDialogShown = false; // 초기화
            if (string.IsNullOrEmpty(dialog))
            {
                dialogText.text = "";
                textBox.SetActive(false); // 대사가 없으면 대사창 숨기기
            }
        }
    }
}