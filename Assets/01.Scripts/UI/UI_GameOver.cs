using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : BaseUI
{
    public Button lobbyBtn;
  protected override void Awake()
    {
        base.Awake();
        if (lobbyBtn != null)
            lobbyBtn.onClick.AddListener(BackToLobby);
    }

    private void BackToLobby()
    {
        Debug.Log("로비씬으로 돌아갑니다!");
        GameManager.Instance.LoadScene("Lobby_Scene");
        Hide();
    }
}
