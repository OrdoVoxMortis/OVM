using UnityEngine;
using UnityEngine.UI;

public class UI_PlayRhythm : BaseUI
{
    public Button playBtn;
    protected virtual void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Hide();
        playBtn.onClick.AddListener(StartRhythm);
    }

    private void StartRhythm()
    {
        Debug.Log("암살 시작!");
    }
}
