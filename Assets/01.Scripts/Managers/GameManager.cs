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
    }
    public void LoadScene(string sceneName)
    {
        UIManager.Instance.ClearUI();
        StartCoroutine(LoadSceneCo(sceneName));
    }

    public void SetSelectedBGM(AudioClip clip)
    {
        SelectedBGM = clip;
    }

    private IEnumerator LoadSceneCo(string scene)
    {
        SceneManager.LoadScene(scene);
        yield return null;

        Player = FindObjectOfType<Player>();
    }
}
