using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class ResourceManager : SingleTon<ResourceManager>
{
    public Dictionary<string, BaseUI> UIList = new();
    public Dictionary<string, AudioClip> BgmList = new();
    public Dictionary<string, AudioClip> SfxList = new();
    public AudioMixer audioMixer;
    protected override void Awake()
    {
        base.Awake();
    }
    public T LoadUI<T>(string name) where T : BaseUI
    {
        if (UIList.TryGetValue(name, out var cacheUi))
        {
            return cacheUi as T;
        }

        var ui = Resources.Load<BaseUI>($"UI/{name}") as T;
        if(ui == null)
        {
            Debug.Log("UI not found");
            return null;
        }
        UIList[name] = ui;
        return ui;
    }

    public void LoadAudio()
    {
        var bgms = Resources.LoadAll<AudioClip>("Sounds/BGM");
        foreach(var clip in bgms)
        {
            if (!BgmList.ContainsKey(clip.name))
            {
                BgmList[clip.name] = clip;
            }
        }

        var sfxs = Resources.LoadAll<AudioClip>("Sounds/SFX");
        foreach(var clip in sfxs)
        {
            if (!SfxList.ContainsKey(clip.name))
            {
                SfxList[clip.name] = clip;
            }
        }

        audioMixer = Resources.Load<AudioMixer>("Sounds/AudioMixer");

    }

}
