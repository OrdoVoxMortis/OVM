using UnityEngine;

public interface IInteractable
{
    public void OnInteract();
    public void Deactive();
    public string GetInteractComponent();
    public void SetInteractComponenet(string newText);
}
public class MissionNote : MonoBehaviour, IInteractable
{
    [SerializeField] private string id; // 의뢰서 고유 id
    public string StageId {  get; private set; } // 스테이지 id
    public string StageName { get; private set; } // 씬 이름 (씬 로드 될때 사용함)
    public string Description {  get; private set; } // 의뢰 내용
    public string ImageName {  get; private set; } // 의뢰 이미지
    public string DialogText { get; private set; } // 의로 대사 이미지
    public UI_Quest questUI;
    private string interactText = "E키를 눌러 상호작용";

    private void Start()
    {
        LoadData();
        if (DataManager.Instance.IsMissionNoteOnInteract(id) && GameManager.Instance.isClear == true)
        {
            Deactive();
            return;
        }
    }

    public void LoadData()
    {
        var data = DataManager.Instance.missionDict[id];
        StageId = data.stageId;
        Description = data.description;
        ImageName = data.filePath;
        DialogText = data.dialog;
        if (DataManager.Instance.stageDict.TryGetValue(StageId, out var stageData))
        {
            StageName = stageData.stageName;
        }
    }

    public void OnInteract()
    {  
        if (!UIManager.Instance.isUIActive)
        {
            
            Sprite image = null;
            if (!string.IsNullOrEmpty(ImageName))
            {
                image = ResourceManager.Instance.LoadImage(ImageName);
            }

            if (string.IsNullOrEmpty(DialogText))
            {
                Debug.Log("대사 없음: textBox는 비활성화 상태로 설정된다");
            }
            questUI.SetQuest(id,Description, image, DialogText, StageName);
            UIManager.Instance.UIActive();
            questUI.gameObject.SetActive(true);
            UIManager.Instance.ShowUI<UI_Quest>("UI_Quest");
            DataManager.Instance.OnInteractMissionNote(id);
            return;
        }
        Debug.Log("UI가 이미 켜져있음");
        
    }

    public string GetInteractComponent()
    {
        return interactText;
    }

    public void SetInteractComponenet(string newText)
    {
        interactText = newText;
    }

    public void Deactive()
    {
        gameObject.SetActive(false);
    }
}
