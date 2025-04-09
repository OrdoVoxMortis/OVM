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

        musicBtn.onClick.RemoveAllListeners();
        musicBtn.onClick.AddListener(()=>action());
    }
}
