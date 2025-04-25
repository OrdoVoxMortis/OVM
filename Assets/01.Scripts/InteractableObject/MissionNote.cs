using UnityEditor.Search;
using UnityEngine;

public interface IInteractable
{
    public void OnInteract();
    public void Deactive();
    public string GetInteractComponent();
}
public class MissionNote : MonoBehaviour, IInteractable
{
    [SerializeField] private string id; // 의뢰서 고유 id
    public string StageId {  get; private set; } // 스테이지 id
    public string Description {  get; private set; } // 의뢰 내용
    public string ImageName {  get; private set; } // 의뢰 이미지
    public string DialogText { get; private set; } // 의로 대사 이미지
    private UI_Quest questUI;

    private void Start()
    {
        questUI = FindObjectOfType<UI_Quest>();
        LoadData();
    }

    public void LoadData()
    {
        var data = DataManager.Instance.missionDict[id];
        StageId = data.stageId;
        Description = data.description;
        ImageName = data.filePath;
        DialogText = data.dialog;
    }

    public void OnInteract()
    {  
        if(!UIManager.Instance.isUIActive)
        {
            
            Sprite image = null;
            if (!string.IsNullOrEmpty(ImageName))
            {
                Debug.Log(ImageName);
                image = ResourceManager.Instance.LoadImage(ImageName);
                Debug.Log(image);
            }

            if (string.IsNullOrEmpty(DialogText))
            {
                Debug.Log("대사 없음: textBox는 비활성화 상태로 설정된다");
            }
            questUI.SetQuest(Description, image, DialogText);
            UIManager.Instance.UIActive();
            UIManager.Instance.ShowUI<UI_Quest>("UI_Quest");
            return;
        }
        Debug.Log("UI가 이미 켜져있음");
        
    }

    public string GetInteractComponent()
    {
        return "E키를 눌러 상호작용";
    }

    public void Deactive()
    {
        gameObject.SetActive(false);
    }
}
