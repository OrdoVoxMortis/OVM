using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Door : MonoBehaviour, IInteractable
{
    private UI_Music musicUI;
    private void Awake()
    {
        musicUI = FindObjectOfType<UI_Music>();
    }
    public string GetInteractComponent()
    {
        return "E키를 눌러 상호작용";
    }

    public void OnInteract()
    {
        if (GameManager.Instance.SelectedBGM == null)
        {
            musicUI.Show();
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
}
