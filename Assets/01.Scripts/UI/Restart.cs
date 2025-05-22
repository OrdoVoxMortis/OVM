using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    [SerializeField] private Button restart;
    void Start()
    {
        if(GameManager.Instance.gameStarted == true)
        {
            this.gameObject.SetActive(true);
        }

        if (restart != null)
            restart.onClick.AddListener(RestartTest);
    }

    public void RestartTest()
    {
        UIManager.Instance.ClearUI();
        GameManager.Instance.LoadScene("Lobby_Scene");
    }
   
}
