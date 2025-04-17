using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public Player Player { get; private set; }
    public int Bpm { get; private set; }
    public AudioClip SelectedBGM {  get; private set; }
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
    }
    public void LoadScene(string sceneName)
    {
        UIManager.Instance.ClearUI();
        SceneManager.LoadScene(sceneName);
    }

}
