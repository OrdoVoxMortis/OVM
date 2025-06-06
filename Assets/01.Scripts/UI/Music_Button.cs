using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Music_Button : MonoBehaviour
{
    [SerializeField] private Button musicBtn;
    [SerializeField] private TextMeshProUGUI musicText;
   

    public void SetMusicButton(string musicString, Action action)
    {
        if (musicText != null)
        {
            musicText.text = musicString;
        }


        musicBtn.onClick.RemoveAllListeners(); // 해당 함수를 통해 지정해주었던, 무명함수를 초기화 시켜준다
        musicBtn.onClick.AddListener(()=>action()); // 버튼 클릭하면 매개변수로 받은 액션 실행
    }
}
