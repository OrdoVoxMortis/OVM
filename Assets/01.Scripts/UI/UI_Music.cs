using UnityEngine;
using UnityEngine.UI;
public class UI_Music : BaseUI
{
    [SerializeField] private Button backBtn;
    [SerializeField] private Music_Button musicBtn;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private Music_Image musicImage;
    [SerializeField] private Transform imageParent;

    private Music_Image currentImage;
    protected override void Awake()
    {
        base.Awake();
        if (backBtn != null)
            backBtn.onClick.AddListener(OnClickBack);
    }

    private void Start()
    {
        gameObject.SetActive(false);
        CreateMusicButtons();
    }

    private void OnClickBack()
    {
        Hide();
    }

    private void CreateMusicButtons()
    {
        foreach (var bgmEntry in ResourceManager.Instance.BgmList) // 리소스 매니저에 있는 딕셔너리 에서 키값을 참조해서 bgmName으로 저장
        {
            string bgmName = bgmEntry.Key;

           
            Music_Button newButton = Instantiate(musicBtn, buttonParent);
            newButton.SetMusicButton(bgmName,( ) => OnClickMusicButton(bgmName));
        }
    }

    private void OnClickMusicButton(string bgmName)
    {
        SoundManager.Instance.PlayBGM(bgmName);
        Debug.Log($"Playing BGM: {bgmName}");
        SoundManager.Instance.SetSelectedBGM(bgmName);
        if(currentImage!=null)
            currentImage.gameObject.SetActive(false);

        currentImage = Instantiate(musicImage, imageParent);

        string imagePath = $"MusicImages/{bgmName}";
        Sprite musicSprite = Resources.Load<Sprite>(imagePath);

        if (musicSprite == null)
        {
            Debug.LogWarning($"[Warning] Sprite not found for {bgmName} at {imagePath}");
        }
        else
        {
            currentImage.SetImage(musicSprite);
        }
    }

}
