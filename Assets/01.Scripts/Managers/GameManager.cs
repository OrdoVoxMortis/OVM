using System.Collections;
using System.Collections.Generic;
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
        SceneManager.LoadScene(sceneName);
    }

    public void SetSelectedBGM(AudioClip clip)
    {
        SelectedBGM = clip;
    }
}
