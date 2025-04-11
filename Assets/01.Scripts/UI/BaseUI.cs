using UnityEngine;

public class BaseUI : MonoBehaviour
{
    GameObject panel;

    protected virtual void Awake()
    {
        panel = transform.GetChild(0).gameObject; // 패널 호출해주는데, 만약 패널이 호출되지 않았다면 첫 번째 자식을 패널이라고 설정해주기
        Cursor.lockState = CursorLockMode.None;
    }

    public virtual void Show() // ui 보여주기
    {
        panel.SetActive(true);
    }

    public virtual void Hide() // ui 숨겨주기
    {
        panel.SetActive(false);
    }
    public virtual void Close()
    {
       // TODO: UImanager를 경유하여, 해당 UI가 중복으로 존재해선 안될경우 삭제!
       Destroy(gameObject); // 임시로 Close 함수 호출하면, 전부다 부셔버리기
    }
}
