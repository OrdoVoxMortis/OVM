using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    [field: SerializeField] public NPCSO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    private TargetStateMachine stateMachine;

    public bool IsNotified { get; set; } = false;

    [Header("Route")]
    public GameObject startBlock;
    public GameObject[] blocks;
    public GameObject turningBlock;
    public GameObject safeZone;
    public int BlockNumber { get; set; } = 0;
    public GameObject[] route;

    public GameObject player;
    public Collider playerCollider;

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

    }

    // Update is called once per frame
    void Update()
    {
        if (IsNotified)
        {
            stateMachine.ChangeState(stateMachine.NotifiedState);
            IsNotified = false;
            return;
        }

        stateMachine.HandleInput();
        stateMachine.Update();
    }

    

}
