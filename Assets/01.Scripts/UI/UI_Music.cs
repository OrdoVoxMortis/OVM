using UnityEngine;
using UnityEngine.UI;
public class UI_Music : BaseUI
{
    [SerializeField] private Button backBtn;
    [SerializeField] private Music_Button musicBtn;
    [SerializeField] private Sprite musicImage;
    [SerializeField] private Transform buttonParent;
    protected override void Awake()
    {
        base.Awake();
        if (backBtn != null)
            backBtn.onClick.AddListener(OnClickBack);
    }

    private void Start()
    {
        CreateMusicButtons();
    }

    private void OnClickBack()
    {
        gameObject.SetActive(false);
        SoundManager.Instance.StopBGM();
        
    }

    private void CreateMusicButtons()
    {
        foreach (var bgmEntry in ResourceManager.Instance.BgmList) // 리소스 매니저에 있는 딕셔너리 에서 키값을 참조해서 bgmName으로 저장
        {
            string bgmName = bgmEntry.Key;

            Music_Button newButton = Instantiate(musicBtn, buttonParent);
            newButton.SetMusicButton(bgmName, musicImage, ( ) => OnClickMusicButton(bgmName));
        }
    }

    private void OnClickMusicButton(string bgmName)
    {
        SoundManager.Instance.PlayBGM(bgmName);
        Debug.Log($"Playing BGM: {bgmName}");

        Sprite newSprite = Resources.Load<Sprite>("MusicImages/" + bgmName);
        if (newSprite != null && musicImage != null)
        {
            musicImage = newSprite;
        }
    }

}
