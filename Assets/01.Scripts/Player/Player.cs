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

    public bool isLockpick = false; // true == 락픽 애니메이션 재생 중




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
        var cur = stateMachine.CurrentState();
        cur?.Exit();
    }


    // Start is called before the first frame update
    void Start()
    {
        // 초기 상태 세팅
        // 현재 초기 세팅은 Idle
        stateMachine.ChangeState(stateMachine.IdleState);
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
}
