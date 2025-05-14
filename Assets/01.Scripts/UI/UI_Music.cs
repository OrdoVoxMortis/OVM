using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class UI_Music : BaseUI
{
    [SerializeField] private Button backBtn;
    [SerializeField] private Music_Button musicBtn;
    [SerializeField] private Button nextMusic;
    [SerializeField] private Button prevMusic;
    [SerializeField] private Button playMusic;
    [SerializeField] private Button volMusic;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private Music_Image musicImage;
    [SerializeField] private Transform imageParent;

    public List<AudioClip> mp3BgmList = new List<AudioClip>();
    public int currentBGM;

    private Music_Image currentImage;
    protected override void Awake()
    {
        base.Awake();
       
        currentBGM = 0;
       
        if (backBtn != null)
            backBtn.onClick.AddListener(OnClickBack);
        if (playMusic != null)
            playMusic.onClick.AddListener(OnClickMusicButton);
        if (nextMusic != null)
            nextMusic.onClick.AddListener(OnclickNextMusic);
        if(prevMusic != null)
            prevMusic.onClick.AddListener(OnclickPrevMusic);
    }

    private void Start()
    {
        mp3BgmList = ResourceManager.Instance.BgmList.Values.ToList();
        UIManager.Instance.UIActive();
    }
    private void OnEnable()
    {
        PlayBGM();
    }
    private void OnClickBack()
    {
        Hide();
    }

    private void OnClickMusicButton()
    {
        PlayBGM();
        SoundManager.Instance.SetSelectedBGM(mp3BgmList[currentBGM].name);
    }

    private void OnclickNextMusic()
    {
        currentBGM++;
        PlayBGM();
       
    }

    private void OnclickPrevMusic()
    {
        currentBGM--;
        PlayBGM();
    }

    private void VolumeUp()
    {

    }

    private void PlayBGM()
    {
        SoundManager.Instance.PlayBGM(mp3BgmList[currentBGM].name);
        if (currentImage != null)
            currentImage.gameObject.SetActive(false);
        currentImage = Instantiate(musicImage, imageParent);
        string imagePath = $"MusicImages/{mp3BgmList[currentBGM].name}";
        Sprite musicSprite = Resources.Load<Sprite>(imagePath);
        currentImage.SetImage(musicSprite);
    }



}
