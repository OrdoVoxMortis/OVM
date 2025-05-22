using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    private static RhythmManager _instance;
    public static RhythmManager Instance { get { return _instance; } }
    [HideInInspector]
    public string bgmName;
    private double musicStartTime;
    private double musicNowTime;
    public double syncTime; //싱크 맞추는 용도

    [HideInInspector]
    public float bpm;
    private double measure; //한 마디 
    public bool isDelayed;  //delay 필요 여부
    public bool isFixed;    //fixed time이 있는 경우
    public float fixedTime;
    //private double timeInMeasure; //한 마디마다 실행 될 수 있게 조절해줄 역할

    //public QTEManager qteManager;

    private AudioClip beepClip;

    private AudioSource beepAudioSource;

    public AnimationCurve curve; //사운드 커브 
    [HideInInspector]
    public float totalMusicTime;
    [HideInInspector] 
    public float curMusicTime;

    private bool isQTE;

    public TextMeshProUGUI checkJudgeText;

    public List<IRhythmActions> rhythmActions;
    private int index = 0;

    public bool isPlaying; //qte, ghost매니저가 끝날 때, false로 변경
    public bool isFinished = false;

    //카메라
    private TimelineCamera timelineCamera;
    private int tlCIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        rhythmActions = new List<IRhythmActions>();
        checkJudgeText.gameObject.SetActive(false);
       
    }

    public void Start()
    {
        measure = 60f / bpm * 4; //4박자가 한 마디 //4마디로 한 줄

        //테스트용 비프음
        beepClip = CreateBeepClip();
        beepAudioSource = gameObject.AddComponent<AudioSource>();
        beepAudioSource.clip = beepClip;
        isPlaying = true;

        checkJudgeText.transform.SetAsLastSibling();
        
    }

    private void Update()
    {
        if (isFinished) return;

        if(!isQTE)
            curMusicTime += Time.deltaTime;
        
        if (isPlaying) return;
      

        if (index >= rhythmActions.Count)
        {
            OnRhythmSequenceComplete();
            return;
        }

        RhythmAction();
    }

    private void InitTime()
    {
        totalMusicTime = 0;
        curMusicTime = 0;
        isQTE = false;

        foreach (var action in rhythmActions)
        {
            if (action is QTEManager) continue;

            totalMusicTime += ((GhostManager)action).checkTimes[((GhostManager)action).checkTimes.Count - 1];
        }
    }
    
    public void StartMusic()
    {
        checkJudgeText.gameObject.SetActive(true);
        checkJudgeText.text = "";
        musicStartTime = AudioSettings.dspTime;
        
        isPlaying = false;
        InitTime();
        SoundManager.Instance.PlayBGM(bgmName);
    }

    public void StartBeep()
    {
        musicStartTime = AudioSettings.dspTime;

        isPlaying = false;
        SoundManager.Instance.PlayBGM(bgmName);
        InvokeRepeating("PlayBeep", (float)syncTime, 60f / bpm);
    }

    public void RhythmAction()
    {
        isPlaying = true;


        if (!isDelayed) //delay 없이 실행
        {
            RhythmMake();
            return;
        }

        if(isFixed) //고정된 시간 이후로 실행
        {
            Invoke("RhythmMake", fixedTime);
            return;
        }

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

        Invoke("RhythmMake", (float)(measure - delay));
    }

    public void RhythmMake()
    {
        UIManager.Instance.CurrentUIHide();
        rhythmActions[index].StartRhythmAction();

        if (rhythmActions[index] is QTEManager)
        {
            SoundManager.Instance.PauseBGM();
        } 
        else
        {
            SoundManager.Instance.UnPauseBGM();
        }

        
        //카메라 타인라인
        if (tlCIndex >= 0)
            timelineCamera.DisableCamera(TimelineManager.Instance.PlacedBlocks[tlCIndex].id, rhythmActions[index]);
        
        tlCIndex++;
        if (tlCIndex >= TimelineManager.Instance.PlacedBlocks.Count)
            tlCIndex = 0;

        timelineCamera.EnableCamera(TimelineManager.Instance.PlacedBlocks[tlCIndex].id, rhythmActions[index >= rhythmActions.Count?0:index]);
        /*
        if (rhythmActions[index] is QTEManager)
            tlCIndex--;
        */
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
        UIManager.Instance.CurrentUIHide();
        isFinished = true;
        GameManager.Instance.GameClear();
    }

    public void RegisterTimelineCamera(TimelineCamera camera)
    {
        tlCIndex = -1;
        timelineCamera = camera;
        Debug.Log($"[SoundManager] TimellineCamera register {camera.name}");
    }

}
