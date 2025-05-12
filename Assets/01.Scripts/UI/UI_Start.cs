using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class UI_Start: BaseUI
{
    [SerializeField] private Button closeBtn; // 연결할 버튼
    [SerializeField] private GameObject lobbyCam; // 로비캠
    private Player player;
    private PlayableDirector playerTimeline;
    private PlayerController playerController;
    private CinemachineInputProvider inputProvider;

    protected override void Awake()
    {
        base.Awake();

        if(closeBtn != null)
            closeBtn.onClick.AddListener(OnStartClick);


    }
    private void Start()
    {
        if (GameManager.Instance.gameStarted == true)
        {
            // 이미 시작된 상태라면 UI 끄고 조작만 활성화
            gameObject.SetActive(false);
            UIManager.Instance.UIDeactive();
            lobbyCam.SetActive(false);
            playerController = GameManager.Instance.Player.Input;
            playerController.playerActions.Enable();
            return;
        }
        playerController = GameManager.Instance.Player.Input;
        playerController.playerActions.Disable();
        UIManager.Instance.UIActive();
        player = GameManager.Instance.Player;
        playerTimeline = player.gameObject.GetComponent<PlayableDirector>();
        inputProvider = GameManager.Instance.Player.Input.playerCamera.GetComponent<CinemachineInputProvider>();
    }

    void OnStartClick()
    {
        gameObject.SetActive(false);
        playerTimeline.stopped += OnTimelineEnd;
        Cursor.lockState = CursorLockMode.Locked;
        playerTimeline.Play();
        GameManager.Instance.gameStarted = true;
        if (inputProvider != null)
            inputProvider.enabled = false;
    }

    private void OnTimelineEnd(PlayableDirector pd)
    {
        playerTimeline.stopped -= OnTimelineEnd;
        SoundManager.Instance.PlayBGM("Background");
        UIManager.Instance.UIDeactive();
        playerController.playerActions.Enable();

        if (inputProvider != null)
            inputProvider.enabled = true;

    }

}
