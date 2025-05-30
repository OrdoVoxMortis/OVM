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
        InitText();
    }
    private void InitText()
    {
        missionDialog.transform.parent.gameObject.SetActive(true);
        planDialog.transform.parent.gameObject.SetActive(true);
        eventDialog.transform.parent.gameObject.SetActive(true);
        rhythmDialog.transform.parent.gameObject.SetActive(true);
    }
    public void SetText()
    {
        stage = StageManager.Instance.StageResult;

        missionDialog.text = stage.missionDialog;
        if (string.IsNullOrEmpty(missionDialog.text))
        {
            missionDialog.transform.parent.gameObject.SetActive(false);
        }

        planDialog.text = stage.planDialog;
        if (string.IsNullOrEmpty(planDialog.text))
        {
            planDialog.transform.parent.gameObject.SetActive(false);
        }

        eventDialog.text = stage.eventDialog;
        if (string.IsNullOrEmpty(eventDialog.text))
        {
            eventDialog.transform.parent.gameObject.SetActive(false);
        }

        rhythmDialog.text = stage.rhythmDialog;
        if (string.IsNullOrEmpty(rhythmDialog.text))
        {
            rhythmDialog.transform.parent.gameObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameClear -= SetText;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnGameClear -= SetText;
    }
    private void BackToLobby()
    {
        Debug.Log("로비씬으로 돌아갑니다!");
        UIManager.Instance.ClearUI();
        GameManager.Instance.LoadScene("Lobby_Scene");
        Hide();
    }
}
