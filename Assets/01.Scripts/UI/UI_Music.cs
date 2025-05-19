using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_Music : BaseUI
{
    [SerializeField] private Button backBtn;
    [SerializeField] private Button nextMusic;
    [SerializeField] private Button prevMusic;
    [SerializeField] private Button playMusic;
    [SerializeField] private Button volMusic;
    [SerializeField] private Music_Image musicImage;
    [SerializeField] private Timer_Text timerText;
    [SerializeField] private TextMeshProUGUI currentMusicText;
    [SerializeField] private TextMeshProUGUI musicNameText;
    [SerializeField] private TextMeshProUGUI musicBpmText;
    [SerializeField] private GameObject mp3Model;
    private float currentVolume = 1.0f;
    public List<AudioClip> mp3BgmList = new List<AudioClip>();
    public int currentBGM;

   
    protected override void Awake()
    {
        base.Awake();
        mp3BgmList = ResourceManager.Instance.InGameBGMDict.Values.ToList();
        currentBGM = 0;
       
        if (backBtn != null)
            backBtn.onClick.AddListener(OnClickBack);
        if (playMusic != null)
            playMusic.onClick.AddListener(OnClickMusicButton);
        if (nextMusic != null)
            nextMusic.onClick.AddListener(OnclickNextMusic);
        if(prevMusic != null)
            prevMusic.onClick.AddListener(OnclickPrevMusic);
        if (volMusic != null)
            volMusic.onClick.AddListener(VolumeUp);
    }
    private void Start()
    {
        UIManager.Instance.UIActive();
        GameManager.Instance.Player.Input.playerCamera.enabled = false;
        SoundManager.Instance.PlaySfx("Effect_Dummy");
        PlayBGM();

    }

    private void OnClickBack()
    {
        Hide();
        UIManager.Instance.DeactivateStandaloneUI("Mp3_Player");
        GameManager.Instance.Player.Input.playerCamera.enabled = true;
        mp3Model.SetActive(false);
    }

    private void OnClickMusicButton()
    {
        SoundManager.Instance.PlaySfx("Effect_Dummy");
        PlayBGM();
        SoundManager.Instance.SetSelectedBGM(mp3BgmList[currentBGM].name);
    }

    private void OnclickNextMusic()
    {
        SoundManager.Instance.PlaySfx("Effect_Dummy");
        currentBGM++;
        if (currentBGM >= mp3BgmList.Count)
        {
            currentBGM--; 
        }
        PlayBGM();
        SoundManager.Instance.SetSelectedBGM(mp3BgmList[currentBGM].name);
    }

    private void OnclickPrevMusic()
    {
        SoundManager.Instance.PlaySfx("Effect_Dummy");
        currentBGM--;
        if (currentBGM < 0)
        {
            currentBGM = 0;
        }
        PlayBGM();
        SoundManager.Instance.SetSelectedBGM(mp3BgmList[currentBGM].name);
    }

    private void VolumeUp()
    {
        SoundManager.Instance.PlaySfx("Effect_Dummy");
        currentVolume += 0.1f;

        if (currentVolume > 1f) 
            currentVolume = 0f;

        SoundManager.Instance.SetBGMVolume(currentVolume);
    }

    private void PlayBGM()
    {
        SoundManager.Instance.PlayBGM(mp3BgmList[currentBGM].name);

        if (timerText != null)
            timerText.ResetTimer(61f);

        string imagePath = $"MusicImages/{mp3BgmList[currentBGM].name}";
        Sprite musicSprite = Resources.Load<Sprite>(imagePath);
        musicImage.SetImage(musicSprite);

        UpdateMusicUI();
    }

    private void UpdateMusicUI()
    {
        if(currentMusicText != null)
        {
            currentMusicText.text = $"{currentBGM + 1} / {mp3BgmList.Count}";
        }

        if(musicNameText != null)
        {
            musicNameText.text = $"{mp3BgmList[currentBGM].name}";
        }
    }


}
