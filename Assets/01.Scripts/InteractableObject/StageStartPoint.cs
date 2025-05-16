using UnityEngine;


public class StageStartPoint : MonoBehaviour, IInteractable
{
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

    public void OnInteract()
    {
        if (GameManager.Instance.SelectedBGM == null)
        {
            UIManager.Instance.SpawnStandaloneUI<UI_Music>("MP3");
            UIManager.Instance.UIActive();
            isInteracted = true;
        }
        else
        {
            Debug.Log("게임 시작");
           
        }
        Camera camera = Camera.main;
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
