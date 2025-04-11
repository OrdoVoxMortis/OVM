
using UnityEngine;
using UnityEngine.EventSystems;

public class Scene : MonoBehaviour
{
    protected void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if(FindObjectOfType<EventSystem>() == null) ResourceManager.Instance.InstantiatePrefab("UI/EventSystem");
    }

}
