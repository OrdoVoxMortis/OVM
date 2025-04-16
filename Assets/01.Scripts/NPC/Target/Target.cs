using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : NPC
{
    [field: SerializeField] public NPCSO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    private TargetStateMachine stateMachine;

    public bool IsNotified { get; set; } = false;


    public int BlockNumber { get; set; } = 1;

    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponent<Animator>();
        Controller = GetComponent<CharacterController>();

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

}
