
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseScene : MonoBehaviour
{
    protected void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
           var go = ResourceManager.Instance.InstantiatePrefab("UI/EventSystem");
        }
    }

}
