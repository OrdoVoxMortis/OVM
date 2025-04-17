using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : SingleTon<RhythmManager>
{
    //public AudioSource audioSource;
    public string bgmName;
    private double musicStartTime;
    private double musicNowTime;
    public double syncTime; //싱크 맞추는 용도
    public float bpm;
    public List<float> beats;
    private double measure; //한 마디 
    //private double timeInMeasure; //한 마디마다 실행 될 수 있게 조절해줄 역할

    public QTEManager qteManager;

    private AudioClip beepClip;

    private AudioSource beepAudioSource;

    public AnimationCurve curve;

    public void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        measure = 60f / bpm * 4 * 4; //4박자가 한 마디 //4마디로 한 줄

        qteManager = GetComponent<QTEManager>();
        qteManager.bpm = bpm;

        //테스트용 비프음
        beepClip = CreateBeepClip();
        beepAudioSource = gameObject.AddComponent<AudioSource>();
        beepAudioSource.clip = beepClip;

        bgmName = "Song1";
    }

    public void StartMusic()
    {
        musicStartTime = AudioSettings.dspTime;
        //audioSource.Play();
        SoundManager.Instance.PlayBGM(bgmName);
    }

    public void StartBeep()
    {
        musicStartTime = AudioSettings.dspTime;
        //audioSource.Play();
        SoundManager.Instance.PlayBGM(bgmName);
        InvokeRepeating("PlayBeep", (float)syncTime, 60f / bpm);
    }


    public void RhythmAction()
    {
        musicNowTime = AudioSettings.dspTime - musicStartTime - 1f; //노래 흐른 시간 // -1은 노트가 100퍼 맞추는 시간
        double delay;

        //빼기 반복은 조금 비효율적인 거 같음
        while (true)
        {
            if (musicNowTime < measure)
                break;

            musicNowTime -= measure;
        }

        //한마디 - 남은 시간 만큼 딜레이를 주고 실행
        delay = musicNowTime;

        Invoke("QTEMake", (float)(measure - delay));
    }

    public void QTEMake()
    {
        qteManager.SetBeatList(beats, bpm);
    }

    void PlayBeep()
    {
        beepAudioSource.Play();
    }

    AudioClip CreateBeepClip(float frequency = 880f, float duration = 0.05f)
    {
        int sampleRate = 44100;
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * i / sampleRate);
        }

        AudioClip clip = AudioClip.Create("Beep", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
