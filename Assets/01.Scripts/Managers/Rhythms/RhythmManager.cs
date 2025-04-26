using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RhythmManager : SingleTon<RhythmManager>
{
    //public AudioSource audioSource;
    public string bgmName;
    private double musicStartTime;
    private double musicNowTime;
    public double syncTime; //싱크 맞추는 용도
    public float bpm;
    private double measure; //한 마디 
    //private double timeInMeasure; //한 마디마다 실행 될 수 있게 조절해줄 역할

    //public QTEManager qteManager;

    private AudioClip beepClip;

    private AudioSource beepAudioSource;

    public AnimationCurve curve;

    public List<IRhythmActions> rhythmActions;
    private int index = 0;

    public bool isPlaying; //qte, ghost매니저가 끝날 때, false로 변경
    public bool isFinished = false;

    //
    private TimelineCamera timelineCamera;
    private int tlCIndex;

    protected override void Awake()
    {
        base.Awake();

        rhythmActions = new List<IRhythmActions>();
    }

    public void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        measure = 60f / bpm * 4; //4박자가 한 마디 //4마디로 한 줄

        //qteManager = GetComponent<QTEManager>();
        //qteManager.bpm = bpm;

        //테스트용 비프음
        beepClip = CreateBeepClip();
        beepAudioSource = gameObject.AddComponent<AudioSource>();
        beepAudioSource.clip = beepClip;

        isPlaying = true;
    }

    private void Update()
    {
        if (isPlaying) return;
        if (isFinished) return;
      

        if (index >= rhythmActions.Count)
        {
            OnRhythmSequenceComplete();
            return;
        }

        RhythmAction();
    }

    public void StartMusic()
    {
        musicStartTime = AudioSettings.dspTime;
        //audioSource.Play();
        isPlaying = false;
        SoundManager.Instance.PlayBGM(bgmName);
    }

    public void StartBeep()
    {
        musicStartTime = AudioSettings.dspTime;
        //audioSource.Play();
        SoundManager.Instance.PlayBGM(bgmName);
        isPlaying = false;
        InvokeRepeating("PlayBeep", (float)syncTime, 60f / bpm);
    }

    public void RhythmAction()
    {
        isPlaying = true;
        musicNowTime = AudioSettings.dspTime - musicStartTime; //노래 흐른 시간 // QTE의 경우는 -1f를 추가로 해줘야한다
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

        //ToDo 끝났을 때
        tlCIndex++;
        if (tlCIndex >= TimelineManager.Instance.PlacedBlocks.Count)
            tlCIndex = 0;
        timelineCamera.EnableCamera(TimelineManager.Instance.PlacedBlocks[tlCIndex].id);
        


        Invoke("RhythmMake", (float)(measure - delay));
    }

    public void RhythmMake()
    {
        rhythmActions[index].StartRhythmAction();
        //ToDo 여기서 다음 리듬액션 시작
        timelineCamera.DisableCamera(TimelineManager.Instance.PlacedBlocks[tlCIndex].id);
        index++;
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

    private void OnRhythmSequenceComplete()
    {

        isFinished = true;
        GameManager.Instance.GameClear();
        Debug.Log("모든 리듬 액션 완료! 게임 종료 처리");

        // 게임 종료 로직 추가0
    }

    public void RegisterTimelineCamera(TimelineCamera camera)
    {
        tlCIndex = -1;
        timelineCamera = camera;
        Debug.Log($"[SoundManager] TimellineCamera register {camera.name}");
    }

}
