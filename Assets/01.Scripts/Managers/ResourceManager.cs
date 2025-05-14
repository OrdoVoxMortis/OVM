using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class ResourceManager : SingleTon<ResourceManager>
{
    public Dictionary<string, BaseUI> UIDict = new();
    public Dictionary<string, AudioClip> LobbyBGMDict = new();
    public Dictionary<string, AudioClip> InGameBGMDict = new();
    public Dictionary<string, AudioClip> SfxDict = new();
    public Dictionary<string, Sprite> ImageDict = new(); 
    public Dictionary<string, AnimationClip> AnimationClipDict = new(); 
    public Dictionary<string, Material> MaterialDict = new(); 
    public AudioMixer audioMixer;
    protected override void Awake()
    {
        base.Awake();
    }
    public T LoadUI<T>(string name) where T : BaseUI
    {
        if (UIDict.TryGetValue(name, out var cacheUi))
        {
            return cacheUi as T;
        }

        var ui = Resources.Load<BaseUI>($"UI/{name}") as T;
        if(ui == null)
        {
            Debug.Log("UI not found");
            return null;
        }
        UIDict[name] = ui;
        return ui;
    }

    public void LoadAudio()
    {
        var inGamebgms = Resources.LoadAll<AudioClip>("Sounds/BGM/InGame");
        foreach(var clip in inGamebgms)
        {
            if (!InGameBGMDict.ContainsKey(clip.name))
            {
              
                InGameBGMDict[clip.name] = clip;
            }
        }

        var lobbybgms = Resources.LoadAll<AudioClip>("Sounds/BGM/Lobby");
        foreach(var clip in lobbybgms)
        {
            if (!LobbyBGMDict.ContainsKey(clip.name))
            {

                LobbyBGMDict[clip.name] = clip;
            }
        }

        var sfxs = Resources.LoadAll<AudioClip>("Sounds/SFX");
        foreach(var clip in sfxs)
        {
            if (!SfxDict.ContainsKey(clip.name))
            {
                SfxDict[clip.name] = clip;
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
        if (string.IsNullOrEmpty(name)) return null;
        if (ImageDict.TryGetValue(name, out var cacheImage))
        {
            return cacheImage as Sprite;
        }
        
        var image = Resources.Load<Sprite>($"Image/{name}");
        ImageDict[name] = image;

        return image;
    }

    public AnimationClip LoadAnimationClip(string name)
    {
        if(AnimationClipDict.TryGetValue(name, out var cacheAnim))
        {
            return cacheAnim as AnimationClip;
        }

        var anim = Resources.Load<AnimationClip>($"Animation/{name}");
        AnimationClipDict[name] = anim;
        return anim;
    }

    public Material LoadMaterial(string name)
    {
        if(MaterialDict.TryGetValue(name, out var cacheMaterial))
        {
            return cacheMaterial;
        }

        var mat = Resources.Load<Material>($"Material/{name}");
        MaterialDict[name] = mat;
        return mat;
    }
}
