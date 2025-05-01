using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class UI_Start: BaseUI
{
    [SerializeField] private Button closeBtn; // 연결할 버튼
    private Player player;
    private PlayableDirector playerTimeline;
    private PlayerController playerController;

    protected override void Awake()
    {
        base.Awake();

        if(closeBtn != null)
            closeBtn.onClick.AddListener(OnStartClick);


    }
    private void Start()
    {
        playerController = GameManager.Instance.Player.Input;
        playerController.playerActions.Disable();
        UIManager.Instance.UIActive();
        player = GameManager.Instance.Player;
        playerTimeline = player.gameObject.GetComponent<PlayableDirector>();
    }

    void OnStartClick()
    {
        gameObject.SetActive(false);
        playerTimeline.stopped += OnTimelineEnd;
        Cursor.lockState = CursorLockMode.Locked;
        playerTimeline.Play();
    }

    private void OnTimelineEnd(PlayableDirector pd)
    {
        playerTimeline.stopped -= OnTimelineEnd;
        SoundManager.Instance.PlayBGM("Background");
        UIManager.Instance.UIDeactive();
        playerController.playerActions.Enable();
    }

}
