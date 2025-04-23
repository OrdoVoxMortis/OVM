using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public Player Player { get; private set; }
    public int Bpm { get; private set; }
    public AudioClip SelectedBGM {  get; private set; }
    public static event Action OnSelectedBGMSet; // 추가
    public Action OnGameOver;
    public bool SimulationMode { get; set; }
    public float playTime;
    public bool isEnd = false;
    protected override void Awake()
    {
        base.Awake();
        Player = FindObjectOfType<Player>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Player = FindObjectOfType<Player>();

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetSelectedBGM(AudioClip clip)
    {
        SelectedBGM = clip;
        if(DataManager.Instance.musicDict.TryGetValue(clip.name, out var bgm))
        {
            Bpm = bgm.BPM;
        }
        Debug.Log("스테이지 음악 할당됨!");
        OnSelectedBGMSet?.Invoke(); // 이벤트 발동!!
    }

    public void LoadScene(string sceneName)
    {
        //UIManager.Instance.ClearUI();
        SceneManager.LoadScene(sceneName);
    }

    public void GameOver()
    {
        if (!isEnd)
        {
            UIManager.Instance.ShowUI<UI_GameClear>("GameClear_UI");
            OnGameOver?.Invoke();
            isEnd = true; 
        }
    }

}
