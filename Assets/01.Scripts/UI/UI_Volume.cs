using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Volume : BaseUI
{
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public AudioMixer audioMixer;
    public Button backBtn;
    public Button quitBtn;
    public Button lobbyBtn;
     

    protected override void Awake()
    {
        base.Awake();
        masterSlider.value = SoundManager.Instance.GetMasterVolume();
        bgmSlider.value = SoundManager.Instance.GetBGMVolume();
        sfxSlider.value = SoundManager.Instance.GetSFXVolume();

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        quitBtn.onClick.AddListener(GameQuit);
        backBtn.onClick.AddListener(Hide);
        lobbyBtn.onClick.AddListener(BackToLobby);
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Lobby_Scene")
        {
            // 로비 씬에서는 quit만 표시하고, lobbyBtn 숨기기
            if(quitBtn !=  null) quitBtn.gameObject.SetActive(true);
            if(lobbyBtn != null) lobbyBtn.gameObject.SetActive(false);
        }

        if(currentScene == "Stage_Scene")
        {
            if (quitBtn != null) quitBtn.gameObject.SetActive(false);
            if (lobbyBtn != null) lobbyBtn.gameObject.SetActive(true);
        }
    }
    public void SetMasterVolume(float value)
    {
        SoundManager.Instance.SetMasterVolume(value);
    }

    public void SetBgmVolume(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }

    public void GameQuit()
    {
        Application.Quit();
#if UNITY_EDITOR // 유니티에서 해당 함수가 호출되면 에디터 플레이 모드 중단
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void BackToLobby()
    {
        GameManager.Instance.LoadScene("Lobby_Scene");
    }

}