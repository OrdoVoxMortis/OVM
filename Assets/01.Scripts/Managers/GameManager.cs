using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public Player Player { get; private set; }
    public int bpm = 120;
    public AudioClip SelectedBGM {  get; private set; }
    public static event Action OnSelectedBGMSet; // 추가
    public Action OnStart;
    public Action OnGameOver;
    public Action OnGameClear;
    public bool gameStarted = false;
    public bool SimulationMode { get; set; }
    public Action OnSimulationMode;
    public bool isEnd = false;
    public StageStartPoint stageStartPoint;
    protected override void Awake()
    {
        base.Awake();
        Player = FindObjectOfType<Player>();
        stageStartPoint = FindObjectOfType<StageStartPoint>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Player = FindObjectOfType<Player>();

        SoundManager.Instance.StopBGM();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetSelectedBGM(AudioClip clip)
    {

        SelectedBGM = clip;
        OnSelectedBGMSet?.Invoke(); // 이벤트 발동!!
    }

    public void LoadScene(string sceneName)
    {
        UIManager.Instance.ClearUI();
        SceneManager.LoadScene(sceneName);
    }
    
    public void GameClear()
    {
        if (!isEnd)
        {
            SoundManager.Instance.StopBGM();

            OnGameClear?.Invoke();

            if (!SaveManager.Instance.eventReplay)
            {
                SaveManager.Instance.SaveGame();

                var ui = UIManager.Instance.ShowUI<UI_GameClear>("GameClear_UI");
                ui.SetText();
                UIManager.Instance.UIActive();
            }
            else
            {
                UIManager.Instance.ClearUI();
                LoadScene("Lobby_Scene");
            }
            SelectedBGM = null;
            isEnd = true;
        }
    }
    public void GameOver()
    {
        if (!isEnd)
        {
            SoundManager.Instance.StopBGM();
            UIManager.Instance.ShowUI<UI_GameOver>("GameOver_UI");
            UIManager.Instance.UIActive();
            OnGameOver?.Invoke();
            SelectedBGM = null;
            isEnd = true;
        }
    }

}
