using UnityEngine;

public class BaseUI : MonoBehaviour
{
    public virtual bool IsPopup => false;

    protected virtual void Awake()
    {
       
        Cursor.lockState = CursorLockMode.None;
    }

    public virtual void Show() // ui 보여주기
    {
        gameObject.SetActive(true);
        UIManager.Instance.UIActive();
    }

    public virtual void Hide()
    {
        Debug.Log($"[BaseUI] {this.name} Hide 호출됨, ActiveSelf: {gameObject.activeSelf}");
        gameObject.SetActive(false);
        Debug.Log($"[BaseUI] {this.name} Hide 완료 후, ActiveSelf: {gameObject.activeSelf}");
        UIManager.Instance.UIDeactive();
    }

    public virtual void Close()
    {
       // TODO: UImanager를 경유하여, 해당 UI가 중복으로 존재해선 안될경우 삭제!
       Destroy(gameObject); // 임시로 Close 함수 호출하면, 전부다 부셔버리기
    }
}
