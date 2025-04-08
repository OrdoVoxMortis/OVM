using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //private Dictionary<string, BaseUI> activeUIs = new(); // 활성화된 UI
    //public T ShowUI<T>() where T : BaseUI
    //{
    //    var ui = ResourceManager.Instance.LoadUI<T>();
    //    var inst = Instantiate(ui);
    //    activeUIs[typeof(T).Name] = inst;
    //    return inst;
    //}

    //public void HideUI<T>() where T : BaseUI
    //{
    //    string name = typeof(T).Name;
    //    if (activeUIs.TryGetValue(name, out var ui)) {
    //        ui.Hide();
    //        activeUIs.Remove(name);
    //    }
    //}

    //public void ClearUI()
    //{
    //    foreach(BaseUI ui in ResourceManager.instance.UIList)
    //    {
    //        Destroy(ui.gameObject);
    //    }
    //    ResourceManager.Instance.Clear();
    //    activeUIs.Clear();
    //}

}
