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
        saveUI.Show();
    }
}
