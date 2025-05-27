using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    [field: SerializeField] public NPCSO Data { get; private set; }

    public LayerMask targetLayer;

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }

    public NavMeshAgent Agent { get; private set; }
    private TargetStateMachine stateMachine;

    public bool IsNotified { get; set; } = false;
    public Vector3 FriendPosition { get; set;}

    [Header("Route")]
    public GameObject startBlock;
    public GameObject[] blocks;
    public GameObject turningBlock;
    public GameObject safeZone;
    public int BlockNumber { get; set; } = 0;
    public GameObject[] route;

    public GameObject player;
    public Collider playerCollider;

    //pause용 내부 상태
    bool isPause = false;
    bool prevAgentStopped;
    float prevAnimSpeed;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverUI;

    [Header("Target 동선 Gizmos 색상")]
    public Color lineColor = Color.magenta;
    public Color wrapLineColor = Color.green;

    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();

        List<GameObject> _route = new List<GameObject>();

        if (startBlock != null)
        {
            _route.Add(startBlock);
        }
        if (blocks != null && blocks.Length > 0)
        {
            _route.AddRange(blocks);
        }
        if (turningBlock != null)
        {
            _route.Add(turningBlock);
        }

        route = _route.ToArray();


        stateMachine = new TargetStateMachine(this);
        Agent.updateRotation = false;

    }



    // Start is called before the first frame update
    void Start()
    {
        if (startBlock != null)
        {

            Agent.Warp(startBlock.transform.position);
        }

        var idle = stateMachine.IdleState;
        var startInfo = startBlock?.GetComponent<TargetBlockInfo>();
        if (startInfo != null && startInfo.blockStateType == TargetBlockStateType.Idle)
        {
            idle.SetDuration(startInfo.stateDuration);
        }
        stateMachine.ChangeState(idle);

        player = GameManager.Instance.Player.gameObject;
        playerCollider = player.GetComponent<Collider>();

        var overrideController = new AnimatorOverrideController(Animator.runtimeAnimatorController);

        Animator.runtimeAnimatorController = overrideController;

        GameManager.Instance.OnStart += DisableTarget;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.SimulationMode)
        {
            if (!isPause)
            {
                //일시정지 하기 전의 상태를 저장
                prevAgentStopped = Agent.isStopped;
                prevAnimSpeed = Animator.speed;

                // Target의 이동을 멈춤
                Agent.isStopped = true;
                // 애니메이션을 멈춤
                Animator.speed = 0f;
                isPause = true;
            }
            return;
        }

        if (isPause)
        {
            Agent.isStopped = prevAgentStopped;
            Animator.speed = prevAnimSpeed;
            isPause = false;
        }

        if (IsNotified)
        {
            IsNotified = false;
            stateMachine.ChangeState(stateMachine.NotifiedState);
            return;
        }

        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.SimulationMode) return;
        stateMachine.PhysicsUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == safeZone)
        {
            GameManager.Instance.GameOver();
        }
    }

    private void OnDrawGizmos()
    {
        if (route == null || route.Length == 0) return;

        for (int i = 0; i < route.Length; i++)
        {
            GameObject gameObject = route[i];
            if (gameObject == null) continue;
            Vector3 pos = gameObject.transform.position;

            Vector3 nextPos;
            Color col;
            if (i < route.Length - 1)
            {
                nextPos = route[i + 1]?.transform.position ?? pos;
                col = lineColor;
            }
            else
            {
                nextPos = route[0]?.transform.position ?? pos;
                col = wrapLineColor;
            }

            Gizmos.color = col;
            Gizmos.DrawLine(pos, nextPos);
        }

    }

    private void DisableTarget()
    {
        GameManager.Instance.OnStart -= DisableTarget;
        this.gameObject.SetActive(false);
    }

}
