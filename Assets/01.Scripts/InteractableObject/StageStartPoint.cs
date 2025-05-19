using UnityEngine;


public class StageStartPoint : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject stage_Start_Point;
    private bool isInteracted = false;
    private UI_Music musicUI;
    private void Awake()
    {
        musicUI = FindObjectOfType<UI_Music>(); 
    }
    public string GetInteractComponent()
    {
        if (!isInteracted) return "E키를 눌러 상호작용";
        else return " ";
    }
    private void OnEnable()
    {
        GameManager.OnSelectedBGMSet += StartGame;
    }

    private void OnDisable()
    {
        GameManager.OnSelectedBGMSet -= StartGame;
    }
    public void OnInteract()
    {
        if (GameManager.Instance.SelectedBGM == null)
        {
            UIManager.Instance.SpawnStandaloneUI<UI_Music>("Mp3_Player");
            UIManager.Instance.UIActive();
            isInteracted = true;
        }
        Camera camera = Camera.main;
    }

    public void StartGame()
    {
        if (stage_Start_Point != null)
            stage_Start_Point.SetActive(false); // 게임 매니저에 음악할당되면 꺼주기, 버그 나면 GameManager OnselectedBGM 확인
        else
            Debug.LogWarning("stage_Start_Point가 비어 있음!");
    }

    public void Deactive()
    {
        throw new System.NotImplementedException();
    }

    public void SetInteractComponenet(string newText)
    {
        throw new System.NotImplementedException();
    }
}
