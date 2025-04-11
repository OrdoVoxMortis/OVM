using UnityEngine;

public class BaseUI : MonoBehaviour
{
    

    protected virtual void Awake()
    {
       
        Cursor.lockState = CursorLockMode.None;
    }

    public virtual void Show() // ui 보여주기
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide() // ui 숨겨주기
    {
        gameObject.SetActive(false);
    }
    public virtual void Close()
    {
       // TODO: UImanager를 경유하여, 해당 UI가 중복으로 존재해선 안될경우 삭제!
       Destroy(gameObject); // 임시로 Close 함수 호출하면, 전부다 부셔버리기
    }
}
