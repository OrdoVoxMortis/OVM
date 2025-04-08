using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : SingleTon<SoundManager>
{
    [SerializeField] private Dictionary<string, AudioClip> bgmDict = new();
    [SerializeField] private Dictionary<string, AudioClip> sfxDict = new();

    private List<AudioSource> sfxPlayers = new();
    private AudioSource bgmPlayer = new();

    private int maxPoolSize = 10;
    private Transform sfxPlayerGroup;
    protected override void Awake()
    {
        base.Awake();
        ResourceManager.Instance.LoadAudio();
        foreach (AudioClip clip in ResourceManager.Instance.BgmList.Values)
        {
            bgmDict[clip.name] = clip;
        }
        foreach (AudioClip clip in ResourceManager.Instance.SfxList.Values)
        {
            sfxDict[clip.name] = clip;
        }
    }

    void InitPlayers()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        GameObject bgmObject = new GameObject("BGMPlayer");
        bgmObject.transform.SetParent(transform);
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;

        sfxPlayerGroup = new GameObject("SfxPlayerGroup").transform;
        sfxPlayerGroup.SetParent(transform);

        for (int i = 0; i < maxPoolSize; i++)
        {
            GameObject sfxObject = new GameObject("SFXPlayer");
            sfxObject.transform.SetParent(sfxPlayerGroup);
            AudioSource sfxPlayer = sfxObject.AddComponent<AudioSource>();
            sfxPlayer.playOnAwake = false;
            sfxPlayer.loop = false;

            sfxPlayers.Add(sfxPlayer);
        }

        sfxPlayers = new List<AudioSource>(sfxPlayerGroup.GetComponentsInChildren<AudioSource>());  
    }

    public void PlayBGM(string bgm)
    {
        if (bgmDict.TryGetValue(bgm, out AudioClip clip))
        {
            bgmPlayer.clip = clip;
            bgmPlayer.Play();
        }
        else Debug.Log("bgm not found");
    }

    public void PlaySfx(string sfx)
    {
        if (sfxDict.TryGetValue(sfx, out AudioClip clip))
        {
            for (int i = 0; i < sfxPlayers.Count; i++)
            {
                if (!sfxPlayers[i].isPlaying)
                {
                    sfxPlayers[i].clip = clip;
                    sfxPlayers[i].Play();
                    return;
                }
            }

            GameObject sfxObject = new GameObject(name);
            sfxObject.transform.SetParent(sfxPlayerGroup);
            AudioSource newSource = sfxObject.AddComponent<AudioSource>();
            newSource.clip = clip;
            newSource.playOnAwake = false;
            newSource.loop = false;
            newSource.Play();
            sfxPlayers.Add(newSource);
            StartCoroutine(DestroyAudiosource(newSource));
        }
        else Debug.Log("sfx not found");
    }

    public void StopAllSfx()
    {
        foreach(AudioSource a in sfxPlayers)
        {
            a.Stop();
        }
    }
    
    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    IEnumerator DestroyAudiosource(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);

        sfxPlayers.Remove(source);
        Destroy(source.gameObject);
    }

    public void StopAll()
    {
        StopAllSfx();
        StopBGM();
    }
}
