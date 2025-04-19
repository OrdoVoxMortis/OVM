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
        Debug.Log("스테이지 음악 할당됨!");
        OnSelectedBGMSet?.Invoke(); // 이벤트 발동!!
    }
    public void LoadScene(string sceneName)
    {
        UIManager.Instance.ClearUI();
        SceneManager.LoadScene(sceneName);
    }

}
