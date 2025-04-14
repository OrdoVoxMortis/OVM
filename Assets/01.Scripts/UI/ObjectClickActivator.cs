using UnityEngine;
using UnityEngine.UI;

public class ObjectClickActivator : MonoBehaviour
{
    private void OnMouseDown() // 오브젝트 클릭했을 때 자동 호출되는 함수
    {
        var clickable = GetComponent<IClickable>();
        if (clickable != null)
        {
            clickable.OnClick();
        }
        else Debug.Log("Clickable object x");
    }

}
