using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UI_Start: BaseUI
{
    [SerializeField] private Button closeBtn; // 연결할 버튼
    [SerializeField] private GameObject lobbyCam; // 로비캠
    [SerializeField] private LobbyScene lobbyScene;
    [SerializeField] private Canvas canvas;
    public bool isEnabled = true;
    public Volume volume;
    private Player player;
    private PlayableDirector playerTimeline;
    private PlayerController playerController;
    private CinemachineInputProvider inputProvider;
    private ColorAdjustments colorAdjustments; // 볼륨의 color adjustment 값 가져오기
    public CinemachineVirtualCamera dollyCam;

    public GameObject playerCollider;

    public static event Action OnStartButtonPressed;        // Start 버튼이 눌린것을 알려줄 이벤트

    protected override void Awake()
    {
        base.Awake();

        if(closeBtn != null)
            closeBtn.onClick.AddListener(OnStartClick);
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            colorAdjustments.saturation.value = -100f;
        }
    }
    private void Start()
    {
        if (GameManager.Instance.gameStarted == true)
        {
            // 이미 시작된 상태라면 UI 끄고 조작만 활성화
            Camera.main.cullingMask = -1;
            gameObject.SetActive(false);
            UIManager.Instance.UIDeactive();
            lobbyCam.SetActive(false);
            playerController = GameManager.Instance.Player.Input;
            playerController.playerActions.Enable();
            colorAdjustments.saturation.value = 0f;
            return;
        }

        playerCollider.SetActive(false);
        Interaction inter = playerCollider.GetComponent<Interaction>();
        inter.interactText.gameObject.SetActive(false);

        playerController = GameManager.Instance.Player.Input;
        playerController.playerActions.Disable();
        UIManager.Instance.UIActive();
        player = GameManager.Instance.Player;
        playerTimeline = player.gameObject.GetComponent<PlayableDirector>();
        inputProvider = GameManager.Instance.Player.Input.playerCamera.GetComponent<CinemachineInputProvider>();
        //Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = isEnabled;
    
    }

    void OnStartClick()
    {
        Camera.main.cullingMask = -1;
        lobbyScene.StartSaturation(LerpSaturation(-100f, 0f, 5));
        playerTimeline.stopped += OnTimelineEnd;
        Cursor.lockState = CursorLockMode.Locked;
        playerTimeline.Play();

        OnStartButtonPressed?.Invoke();     // 이벤트 발행

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        GameManager.Instance.gameStarted = true;
        if (inputProvider != null)
            inputProvider.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnTimelineEnd(PlayableDirector pd)
    {
        playerTimeline.stopped -= OnTimelineEnd;
        SoundManager.Instance.PlayBGM("Background");
        UIManager.Instance.UIDeactive();
        playerController.playerActions.Enable();
        dollyCam.Priority = 0;
        playerCollider.SetActive(true);

        if (inputProvider != null)
            inputProvider.enabled = true;

    }

   public IEnumerator LerpSaturation(float from, float to, float duration) // 포스트프로세싱 볼륨에 해당된 값을 부드럽게 처리하기 위한 코루틴
    {
        float elapsed = 0f; // 경과한 시간 초기화
        while (elapsed < duration)
        {
            float t = elapsed / duration; // 경과 시간을 비율로 계산
            colorAdjustments.saturation.value = Mathf.Lerp(from, to, t); // 시작값과 끝나는 값 보간하여 점차 증가함
            elapsed += Time.deltaTime; // 경과시간 프레임마다 증가
            yield return null;
        }
        colorAdjustments.saturation.value = to;
    }
}
