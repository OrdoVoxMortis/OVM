using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameClear : BaseUI
{
    StageResult stage;

    [SerializeField] private TextMeshProUGUI missionDialog;
    [SerializeField] private TextMeshProUGUI planDialog;
    [SerializeField] private TextMeshProUGUI eventDialog;
    [SerializeField] private TextMeshProUGUI rhythmDialog;


    public Button lobbyBtn;
    protected override void Awake()
    {
        GameManager.Instance.OnGameClear += SetText;
        if (lobbyBtn != null)
            lobbyBtn.onClick.AddListener(BackToLobby);
    }

    public void SetText()
    {
        stage = StageManager.Instance.StageResult;
        Debug.Log(stage.missionDialog);
        Debug.Log(stage.planDialog);
        Debug.Log(stage.eventDialog);
        missionDialog.text = stage.missionDialog;
        if (string.IsNullOrEmpty(missionDialog.text))
        {
            Destroy(missionDialog.transform.parent.gameObject);
        }

        planDialog.text = stage.planDialog;
        if (string.IsNullOrEmpty(planDialog.text))
        {
            Destroy(planDialog.transform.parent.gameObject);
        }

        eventDialog.text = stage.eventDialog;
        if (string.IsNullOrEmpty(eventDialog.text))
        {
            Destroy(eventDialog.transform.parent.gameObject);
        }

        rhythmDialog.text = stage.rhythmDialog;
        if (string.IsNullOrEmpty(rhythmDialog.text))
        {
            Destroy(rhythmDialog.transform.parent.gameObject);
        }
    }

    private void BackToLobby()
    {
        Debug.Log("로비씬으로 돌아갑니다!");
        UIManager.Instance.ClearUI();
        GameManager.Instance.LoadScene("Lobby_Scene");
        Hide();
    }
}
