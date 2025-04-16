
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Volume : BaseUI
{
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public AudioMixer audioMixer;
    public Button backBtn;
    public Button quitBtn;
  

    protected override void Awake()
    {
        base.Awake();
        masterSlider.value = SoundManager.Instance.GetMasterVolume();
        bgmSlider.value = SoundManager.Instance.GetBGMVolume();
        sfxSlider.value = SoundManager.Instance.GetSFXVolume();

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        quitBtn.onClick.AddListener(GameQuit);
        backBtn.onClick.AddListener(Hide);
    }

    public void SetMasterVolume(float value)
    {
        SoundManager.Instance.SetMasterVolume(value);
    }

    public void SetBgmVolume(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }

    public void GameQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
   

   
}