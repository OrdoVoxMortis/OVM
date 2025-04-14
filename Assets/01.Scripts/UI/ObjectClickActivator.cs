using UnityEngine;

public class ObjectClickActivator : MonoBehaviour
{
    [SerializeField] private GameObject targetUI; // 띄워줄 UI

    private void OnMouseDown() // 오브젝트 클릭했을 때 자동 호출되는 함수
    {
        if (targetUI != null)
        {
            targetUI.SetActive(true); // UI 활성화
        }
        else
        {
            Debug.LogWarning("띄워줄 UI가 설정되지 않았습니다!");
        }
    }
}
