using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class UI_Start: BaseUI
{
    [SerializeField] private Button closeBtn; // 연결할 버튼
    private Player player;
    private PlayableDirector playerTimeline;

    protected override void Awake()
    {
        base.Awake();

        if(closeBtn != null)
            closeBtn.onClick.AddListener(OnStartClick);


    }
    private void Start()
    {
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
        UIManager.Instance.UIDeactive();
    }

}
