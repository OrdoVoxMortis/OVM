using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_Music : BaseUI
{
    [SerializeField] private Button backBtn;
    [SerializeField] private Button musicBtn;
    [SerializeField] private TextMeshProUGUI musicName;
    [SerializeField] private Image musicImage;
    [SerializeField] private Transform buttonParent;
    // [SerializeField] private List<ButtonSoundData> buttonSoundList = new List<ButtonSoundData>();
    public int sfxIndex;
    private int currentIndex = 0;
    private List<Button> createdButtons = new List<Button>();
    protected override void Awake()
    {
        base.Awake();
        if (backBtn != null)
            backBtn.onClick.AddListener(OnclickBack);
        if (musicBtn != null)
            musicBtn.onClick.AddListener(() => SpawnMusicButton(musicBtn));

        //for(int i = 0; i < buttonSoundList.Count; i++)
        //{
        //    ButtonSoundData data = buttonSoundList[i];
        //    if (data.musicBtn != null)
        //    {
        //        int index = data.sfxIndex;
        //        data.musicBtn.onClick.AddLisner(() => PlaySound());
        //    }               
        //}
    }

    private void PlaySoundByIndex(int index)
    {
        //if (SoundManager.Instance != null && index >= 0 && index < SoundManager.Instance.sfxList.Count)
        //{
        //    AudioClip clip = SoundManager.Instance.sfxList[index];
        //    SoundManager.Instance.PlaySFX(clip);
        //}
    }

    private void OnclickBack()
    {
        gameObject.SetActive(false);
    }

    private void SpawnMusicButton(Button sourceButton)
    {
        // 원본 버튼(sourceButton)을 복제
        Button newButton = Instantiate(sourceButton, buttonParent);
        newButton.gameObject.SetActive(true); // 복제본 활성화

        // 새로 생성된 버튼에도 자신을 복제하는 기능 추가
        newButton.onClick.RemoveAllListeners();
        newButton.onClick.AddListener(() => SpawnMusicButton(newButton));
    }

}
