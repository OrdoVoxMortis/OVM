using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    //private Player player;
    //public Player Player {  get { return player; } }

    protected override void Awake()
    {
        base.Awake();
    }
    public void LoadScene(string sceneName)
    {
        UIManager.Instance.ClearUI();
        SceneManager.LoadScene(sceneName);
    }
}
