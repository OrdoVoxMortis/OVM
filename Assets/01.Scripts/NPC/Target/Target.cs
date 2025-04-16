using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : NPC
{
    [field: SerializeField] public NPCSO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    private TargetStateMachine stateMachine;

    public bool IsNotified { get; set; } = false;

    public GameObject startBlock;
    public GameObject[] blocks;
    public GameObject turningBlock;
    public GameObject safeZone;
    public int BlockNumber { get; set; } = 0;

    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();

        List<GameObject> route = new List<GameObject>();

        if (startBlock != null)
        {
            route.Add(startBlock);
        }
        if (blocks != null && blocks.Length > 0)
        {
            route.AddRange(blocks);
        }
        if (turningBlock != null)
        {
            route.Add(turningBlock);
        }

        blocks = route.ToArray();

        stateMachine = new TargetStateMachine(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void OnTriggerEnter(Collider other)
    {

        TargetBlockInfo blockInfo = other.GetComponent<TargetBlockInfo>();
        if (blockInfo != null)
        {
            Debug.LogError($"Blocks{BlockNumber}, Type : {blockInfo.blockStateType}");

            switch (blockInfo.blockStateType)
            {
                case TargetBlockStateType.Idle:
                    {
                        TargetIdleState idleState = stateMachine.IdleState;
                        idleState.SetDuration(blockInfo.stateDuration);

                        stateMachine.ChangeState(idleState);
                    }
                    break;

                case TargetBlockStateType.Interaction:
                    {
                        TargetInteractionState interactionState = stateMachine.InteractionState;
                        interactionState.SetDuration(blockInfo.stateDuration);
                        stateMachine.ChangeState(interactionState);
                    }
                    break;

                default:
                    {
                        stateMachine.Target.BlockNumber++;
                        stateMachine.ChangeState(stateMachine.ChasingState);
                    }
                    break;
            }
        }

    }

}
