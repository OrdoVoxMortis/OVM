using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingleTon<UIManager>
{
    private Dictionary<string, BaseUI> activeUIs = new(); // 활성화된 UI
    public bool isUIActive = false;
    [SerializeField] private BaseUI currentUI = null;
    private Canvas canvas;
    public static event System.Action popupSetting;

    protected override void Awake()
    {
        base.Awake();
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            popupSetting?.Invoke();
            Debug.Log("메뉴출력");
        }
    }

    public T ShowUI<T>(string name) where T : BaseUI
    {
        GetCanvas();
        if(activeUIs.TryGetValue(name, out var ui))
        {
            ui.Show();
            currentUI = ui;
            Debug.Log("현재Ui창 할당됨!");
            return (T) ui;
        }
        else
        {
            ui = ResourceManager.Instance.LoadUI<T>(name);
            if (ui == null) return null;
            var inst = Instantiate(ui, canvas.transform);
            activeUIs[typeof(T).Name] = inst;
            currentUI = inst;
            return (T)inst;
        }
  
    }

    public void HideUI<T>() 
    {
        string name = typeof(T).Name;
        if (activeUIs.TryGetValue(name, out var ui))
        {
            ui.Hide();
            currentUI = null;
            //activeUIs.Remove(name);
        }

    }

    public void ClearUI()
    {
        foreach (BaseUI ui in activeUIs.Values)
        {
            if(ui != null)
                Destroy(ui.gameObject);
        }
        activeUIs.Clear();
    }

    public void UIActive()
    {
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("UI Active: 커서 Unlock 처리됨");
        Cursor.visible = true;

        if (GameManager.Instance.Player.Input.playerCamera != null)
            GameManager.Instance.Player.Input.playerCamera.enabled = false;

        isUIActive = true;
    }

    public void UIDeactive()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("UI DeActive: 커서 lock 처리됨");
        Cursor.visible = false;

        if (GameManager.Instance.Player.Input.playerCamera != null)
            GameManager.Instance.Player.Input.playerCamera.enabled = true;

        isUIActive = false;
    }

    public void CurrentUIHide()
    {
        if (currentUI == null)
        {
            Debug.LogWarning("currentUI가 null입니다!");
            return;
        }

        Debug.Log($"현재 숨기려는 UI: {currentUI.name}");

        currentUI.Hide();
        isUIActive = false;
    }


    private void GetCanvas()
    {
        if(canvas == null)
        {
            canvas = FindObjectOfType<Canvas>();
        }
    }

}
