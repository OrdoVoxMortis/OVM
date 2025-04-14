using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingleTon<UIManager>
{
    private Dictionary<string, BaseUI> activeUIs = new(); // 활성화된 UI
    public bool isUIActive = false;
    public CinemachineFreeLook playerCamera;

    protected override void Awake()
    {
        base.Awake();
    }
    public T ShowUI<T>(string name) where T : BaseUI
    {
        var ui = ResourceManager.Instance.LoadUI<T>(name);
        if (ui == null) return null;
        var inst = Instantiate(ui);
        activeUIs[typeof(T).Name] = inst;
        return inst;
    }

    public void HideUI<T>() 
    {
        string name = typeof(T).Name;
        if (activeUIs.TryGetValue(name, out var ui))
        {
            ui.Hide();
            activeUIs.Remove(name);
        }
    }

    public void ClearUI()
    {
        foreach (BaseUI ui in ResourceManager.Instance.UIList.Values)
        {
            Destroy(ui.gameObject);
        }
        ResourceManager.Instance.UIList.Clear();
        activeUIs.Clear();
    }

    public void UIActive()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerCamera != null)
            playerCamera.enabled = false;

        isUIActive = true;
    }

    public void UIDeactive()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerCamera != null)
            playerCamera.enabled = true;

        isUIActive = false;
    }

}
