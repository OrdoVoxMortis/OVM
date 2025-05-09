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
    public Dictionary<string, AnimationClip> AnimationClipList = new(); 
    public Dictionary<string, Material> MaterialList = new(); 
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

    public GameObject InstantiatePrefab(string name, Transform trans = null)
    {
        GameObject prefab = Resources.Load<GameObject>(name);
        return Instantiate(prefab, trans);
    }

    public Sprite LoadImage(string name)
    {
        if (ImageList.TryGetValue(name, out var cacheImage))
        {
            return cacheImage as Sprite;
        }
        
        var image = Resources.Load<Sprite>($"Image/{name}");
        ImageList[name] = image;

        return image;
    }

    public AnimationClip LoadAnimationClip(string name)
    {
        if(AnimationClipList.TryGetValue(name, out var cacheAnim))
        {
            return cacheAnim as AnimationClip;
        }

        var anim = Resources.Load<AnimationClip>($"Animation/{name}");
        AnimationClipList[name] = anim;
        return anim;
    }

    public Material LoadMaterial(string name)
    {
        if(MaterialList.TryGetValue(name, out var cacheMaterial))
        {
            return cacheMaterial;
        }

        var mat = Resources.Load<Material>($"Material/{name}");
        MaterialList[name] = mat;
        return mat;
    }
}
