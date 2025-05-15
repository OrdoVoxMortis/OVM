using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SoundManager : SingleTon<SoundManager>
{
    [SerializeField] private Dictionary<string, AudioClip> bgmDict = new();
    [SerializeField] private Dictionary<string, AudioClip> sfxDict = new();

    private List<AudioSource> activeSfxPlayers = new();
    private AudioSource bgmPlayer = new();

    private int maxPoolSize = 10;
    private Queue<AudioSource> sfxPool = new();
    private Transform sfxPlayerGroup;

    [SerializeField] private AudioMixer audioMixer;
    string Master_Mixer = "Master";
    string Bgm_Mixer = "BGM";
    string Sfx_Mixer = "SFX";

    protected override void Awake()
    {
        base.Awake();
        ResourceManager.Instance.LoadAudio();
        foreach (AudioClip clip in ResourceManager.Instance.InGameBGMDict.Values)
        {
            Debug.Log("음악추가됨!");
            bgmDict[clip.name] = clip;
        }
        foreach(AudioClip clip in ResourceManager.Instance.LobbyBGMDict.Values)
        {
            Debug.Log("음악추가됨!");
            bgmDict[clip.name] = clip;
        }
        foreach (AudioClip clip in ResourceManager.Instance.SfxDict.Values)
        {
            Debug.Log("효과음추가됨!");
            sfxDict[clip.name] = clip;
        }

        audioMixer = ResourceManager.Instance.audioMixer;

        InitPlayers();
    }
    private void Start()
    {
        LoadVolume();
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
        bgmPlayer.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/BGM")[0];

        sfxPlayerGroup = new GameObject("SfxPlayerGroup").transform;
        sfxPlayerGroup.SetParent(transform);

        for (int i = 0; i < maxPoolSize; i++)
        {
            AudioSource source = CreateSfxSource();
            sfxPool.Enqueue(source);
        }
    }

    AudioSource CreateSfxSource()
    {
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.SetParent(sfxPlayerGroup);
        AudioSource sfxPlayer = sfxObject.AddComponent<AudioSource>();
        sfxPlayer.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/SFX")[0];
        sfxPlayer.loop = false;
        sfxPlayer.playOnAwake = false;
        return sfxPlayer;

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
            AudioSource source = GetSfxSource();
            source.clip = clip;
            source.PlayOneShot(clip);
            StartCoroutine(ReturnAudiosource(source));
        }
        else Debug.Log("sfx not found");
    }

    private AudioSource GetSfxSource()
    {
        if(sfxPool.Count > 0)
        {
            AudioSource src = sfxPool.Dequeue();
            activeSfxPlayers.Add(src);
            return src;
        }
        else
        {
            AudioSource newSrc = CreateSfxSource();
            activeSfxPlayers.Add(newSrc);
            return newSrc;
        }
    }
    public void StopAllSfx()
    {
        foreach(AudioSource a in activeSfxPlayers)
        {
            a.Stop();
        }
    }
    
    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PauseBGM()
    {
        if(bgmPlayer.isPlaying)
            bgmPlayer.Pause();
    }

    public void UnPauseBGM()
    {
        if(!bgmPlayer.isPlaying && bgmPlayer.time > 0f && bgmPlayer.time < bgmPlayer.clip.length)
            bgmPlayer.UnPause();
    }

    IEnumerator ReturnAudiosource(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);

        source.clip = null;
        if(activeSfxPlayers.Contains(source)) activeSfxPlayers.Remove(source);

        if(sfxPool.Count < maxPoolSize) sfxPool.Enqueue(source);
        else Destroy(source.gameObject);
    }

    public void StopAll()
    {
        StopAllSfx();
        StopBGM();
    }

    public float GetMasterVolume()
    {
        audioMixer.GetFloat(Master_Mixer, out float volume);
        return Mathf.Pow(10, volume / 20);
    }

    public void SetMasterVolume(float volume)
    {
        volume = Mathf.Max(0.0001f, volume);
        audioMixer.SetFloat(Master_Mixer, Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("Master", GetMasterVolume());
    }

    public float GetBGMVolume()
    {
        audioMixer.GetFloat(Bgm_Mixer, out float volume);
        return Mathf.Pow(10, volume / 20);
    }

    public void SetBGMVolume(float volume)
    {
        volume = Mathf.Max(0.0001f, volume);
        audioMixer.SetFloat(Bgm_Mixer, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGM", GetBGMVolume());
    }
    public float GetSFXVolume()
    {
        audioMixer.GetFloat(Sfx_Mixer, out float volume);
        return Mathf.Pow(10, volume / 20);
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Max(0.0001f, volume);
        audioMixer.SetFloat(Sfx_Mixer, Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("SFX", GetSFXVolume());
    }

    public void LoadVolume()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("Master", 1f));
        SetBGMVolume(PlayerPrefs.GetFloat("BGM", 1f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFX", 1f));
    }

    public void SetSelectedBGM(string bgmName)
    {
        if (bgmDict.TryGetValue(bgmName, out AudioClip clip))
        {
            GameManager.Instance.SetSelectedBGM(clip);
        }
    }
}
