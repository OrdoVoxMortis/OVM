using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field:SerializeField] public PlayerSO Data { get; private set; }

    [field:Header("Animations")]
    [field:SerializeField]public PlayerAnimationData AnimationData {  get; private set; }

    public Animator Animator { get; private set; }
    public PlayerController Input {  get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Interaction Interaction { get; private set; }

    public PlayerStateMachine stateMachine;
    public CinemachineComposer composer;

    public bool isSimulMode = false;
    public bool isSit = false;
    public bool isSquat = false;
    public bool isLockpick = false; // true == 락픽 애니메이션 재생 중

    [Header("첫 앉기 설정")]
    [SerializeField] private bool startSittingOnLoad = false;
    [SerializeField] private InteractionChair initalChair = null;

    private bool _hasAppliedStartSit;




    private void Awake()
    {
        AnimationData.Initialize();

        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerController>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Interaction = GetComponent<Interaction>();

        stateMachine = new PlayerStateMachine(this);

    }

    private void OnEnable()
    {
        var cur = stateMachine.CurrentState();
        cur?.Enter();
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStart -= DisablePlayer;
        var cur = stateMachine.CurrentState();
        cur?.Exit();
    }


    // Start is called before the first frame update
    void Start()
    {
        // 초기 상태 세팅
        // 현재 초기 세팅은 Idle

        if (startSittingOnLoad && !_hasAppliedStartSit && initalChair != null)
        {
            _hasAppliedStartSit = true;
            startSittingOnLoad = false;

            if (!GameManager.Instance.gameStarted)
            {
                stateMachine.ChangeState(new PlayerInterationSitState
                (stateMachine, initalChair, initalChair.SeatPoint, initalChair.SitDownDuration, initalChair.StandUpDuration, true));
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }

            
        }
        else
        {
            stateMachine.ChangeState(stateMachine.IdleState);

        }
        GameManager.Instance.OnStart += DisablePlayer;
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어 입력처리, 상태별 로직
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    private void DisablePlayer()
    {
        GameManager.Instance.OnStart -= DisablePlayer;
        gameObject.SetActive(false);
    }
}
