using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashcan : MonoBehaviour, IInteractable
{
    private UI_SaveLoad saveUI;
    private void Start()
    {
        saveUI = FindObjectOfType<UI_SaveLoad>();
    }
    public string GetInteractComponent()
    {
        return "E키를 눌러 상호작용";
    }

    public void OnInteract()
    {
        if (!UIManager.Instance.isUIActive)
        {
            UIManager.Instance.ShowUI<UI_SaveLoad>("UI_SaveLoad");
            UIManager.Instance.UIActive();
        }
    }

    public void Deactive()
    {
        throw new System.NotImplementedException();
    }
}
