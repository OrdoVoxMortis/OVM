using UnityEngine;

public interface IClickable
{
    void OnClick();
}
public class MissionNote : MonoBehaviour, IClickable
{
    [SerializeField] private string id; // 의뢰서 고유 id
    public string StageId {  get; private set; } // 스테이지 id
    public string Description {  get; private set; } // 의뢰 내용
    public string ImageName {  get; private set; } // 의뢰 이미지

    private void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        var data = DataManager.Instance.missionDict[id];
        StageId = data.stageId;
        Description = data.description;
        ImageName = data.filePath;
    }

    public void OnClick()
    {
        var questUI = FindObjectOfType<UI_Quest>();
        if(questUI != null)
        {
            Sprite image = null;
            if (!string.IsNullOrEmpty(ImageName))
            {
                image = Resources.Load<Sprite>(ImageName);
            }
            questUI.SetQuest(Description, image, "");
            questUI.Show();
        }
    }
}
