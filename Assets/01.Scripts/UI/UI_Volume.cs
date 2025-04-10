
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class UI_Volume : BaseUI
{
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public AudioMixer audioMixer;

    protected override void Awake()
    {
        base.Awake();
        masterSlider.value = SoundManager.Instance.GetMasterVolume();
        bgmSlider.value = SoundManager.Instance.GetBGMVolume();
        sfxSlider.value = SoundManager.Instance.GetSFXVolume();

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
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
}