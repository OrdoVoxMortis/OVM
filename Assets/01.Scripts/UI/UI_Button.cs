using UnityEngine;

public class UI_Button : BaseUI
{
    [SerializeField] private BaseUI[] uiToHide; // 숨겨줄 ui 할당해주기

    public void OnButtonHide() // 버튼 누르면 숨겨주는 함수
    {
        for(int i = 0; i < uiToHide.Length; i++)
        {
            if(uiToHide[i] != null)
            {
                uiToHide[i].Hide();
            }
        }
    }
}
