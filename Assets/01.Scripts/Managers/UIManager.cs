using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingleTon<UIManager>
{
    private Dictionary<string, BaseUI> activeUIs = new(); // 활성화된 UI
    public bool isUIActive = false;
    [SerializeField] private BaseUI currentUI = null;
    [SerializeField] private Stack <BaseUI> uiStack = new();
    private Canvas canvas;
    public static event System.Action popupSetting;
    private Dictionary<string, BaseUI> standaloneUIs = new();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
           ShowUI<UI_Volume>("UI_Volume", allowDuplicate: true);
           UIActive();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            CloseTopPopup();
        }
    }

    public T ShowUI<T>(string name, bool allowDuplicate = false) where T : BaseUI
    {
        GetCanvas();

        BaseUI ui = null;
        if(!allowDuplicate && activeUIs.TryGetValue(name, out ui))
        {
            ui.Show();
        }
        else
        {
            ui = ResourceManager.Instance.LoadUI<T>(name);
            if (ui == null) return null;
            ui = Instantiate(ui, canvas.transform);
            if (!allowDuplicate) 
                activeUIs[name] = ui;
        }

        if (ui.IsPopup)
        {
            uiStack.Push(ui);
        }
        else
        {
            currentUI = ui;
        }
        ui.Show();
        isUIActive = true;
        Debug.Log("현재Ui창 스택됨!");
        return (T)ui;
        //else
        //{
        //    ui = ResourceManager.Instance.LoadUI<T>(name);
        //    if (ui == null) return null;
        //    var inst = Instantiate(ui, canvas.transform);
        //    activeUIs[typeof(T).Name] = inst;
        //    currentUI = inst;
        //    return (T)inst;
        //}

    }

    public T SpawnStandaloneUI<T>(string name) where T : BaseUI
    {
        string key = typeof(T).Name;
        if (standaloneUIs.TryGetValue(name, out var existingUI))
        {
            if(existingUI != null)
            {
                Debug.LogWarning($"Standalone UI {name} 은(는) 이미 존재합니다.");
                Destroy(existingUI.gameObject);
            }
          
            standaloneUIs.Remove(name);
        }
        T prefab = ResourceManager.Instance.LoadUI<T>(name);
        if (prefab == null)
        {
            Debug.LogError($"프리팹 {name} 을(를) Resources에서 찾을 수 없습니다.");
            return null;
        }

        Transform mainCameraTransform = Camera.main?.transform;
        if (mainCameraTransform == null)
        {
            Debug.LogError("Main Camera를 찾을 수 없습니다.");
            return null;
        }

        T instance = GameObject.Instantiate(prefab, mainCameraTransform, false);
        instance.Show();
        standaloneUIs[name] = instance; // 딕셔너리에서 등록을 해주어야 한다
        Debug.Log($"Standalone UI {name} 생성 완료!");
        return instance;
    }

    public void HideUI<T>() 
    {
        //string name = typeof(T).Name;
        //if (activeUIs.TryGetValue(name, out var ui))
        //{
        //    ui.Hide();
        //    currentUI = null;
        //    //activeUIs.Remove(name);
        //}

        if (uiStack.Count == 0) return;

        var topUI = uiStack.Pop();
        topUI.Hide();

        isUIActive = uiStack.Count > 0;

    }

    public void DeactivateStandaloneUI(string name)
    {
        if (standaloneUIs.TryGetValue(name, out var ui))
        {
            Destroy(ui.gameObject);
            Debug.Log($"Standalone UI {name} 비활성화됨.");
        }
        else
        {
            Debug.LogWarning($"Standalone UI {name} 을(를) 찾을 수 없습니다.");
        }
    }

    public void ClearUI()
    {
        foreach (BaseUI ui in activeUIs.Values)
        {

            if (ui != null && ui.gameObject.scene.IsValid()) // 씬 인스턴스만 Destroy
            {
                Destroy(ui.gameObject);
            }
        }

        activeUIs.Clear();
        uiStack.Clear();
        isUIActive = false;
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

    public void CloseTopPopup()
    {
        if (uiStack.Count == 0) return;

        var topUI = uiStack.Peek();
        if (topUI.IsPopup)
        {
            uiStack.Pop().Hide(); // 팝업은 Hide -> Destroy
        }
    }


    private void GetCanvas()
    {
        if (canvas == null)
        {
            // 모든 Canvas 찾기
            Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>(true);

            // sortingOrder가 가장 낮은 Canvas 가져오기
            Canvas lowestOrderCanvas = null;
            int lowestOrder = int.MaxValue;

            foreach (Canvas c in canvases)
            {
                if (c.renderMode != RenderMode.WorldSpace && c.sortingOrder < lowestOrder)
                {
                    lowestOrder = c.sortingOrder;
                    lowestOrderCanvas = c;
                }
            }

            if (lowestOrderCanvas != null)
            {
                canvas = lowestOrderCanvas;
                Debug.Log($"UIManager: sortingOrder {lowestOrder} Canvas 선택됨");
            }
            else
            {
                Debug.LogWarning("UIManager: sortingOrder 0 Canvas를 찾지 못했습니다. 기본 Canvas 사용.");
                canvas = FindObjectOfType<Canvas>(); // fallback
            }
        }
    }

    public void OnEscPressed()
    {
        UIDeactive();
        DeactivateStandaloneUI("Mp3_Player");
        if (uiStack.Count > 0)
        {
           CloseTopPopup();
        }
        else if(currentUI != null)
        {
            currentUI.Hide();
           
            currentUI = null;
        }
    }
}
