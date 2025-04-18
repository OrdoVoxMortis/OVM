using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class ResourceManager : SingleTon<ResourceManager>
{
    public Dictionary<string, BaseUI> UIList = new();
    public Dictionary<string, AudioClip> BgmList = new();
    public Dictionary<string, AudioClip> SfxList = new();
    public Dictionary<string, Sprite> ImageList = new(); 
    public Dictionary<string, Animation> AnimationList = new(); 
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

    public void InstantiatePrefab(string name)
    {
        GameObject prefab = Resources.Load<GameObject>(name);
        Instantiate(prefab);
    }

    public Sprite LoadImage(string name)
    {
        if (ImageList.TryGetValue(name, out var cacheImage))
        {
            return cacheImage as Sprite;
        }
        
        var image = Resources.Load<Sprite>($"Image/{name}");
        ImageList[name] = image;
        Debug.Log(name);
        Debug.Log(image);
        return image;
    }

    public Animation LoadAnimation(string name)
    {
        if(AnimationList.TryGetValue(name, out var cacheAnim))
        {
            return cacheAnim as Animation;
        }

        var anim = Resources.Load<Animation>($"Animation/{name}");
        AnimationList[name] = anim;
        return anim;
    }
}
