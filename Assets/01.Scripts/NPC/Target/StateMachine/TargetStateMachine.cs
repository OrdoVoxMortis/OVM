using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetStateMachine : StateMachine
{
    public Target Target { get; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;


    public GameObject[] Blocks { get; private set; }
    public TargetIdleState IdleState { get; }
    public TargetChasingState ChasingState { get; }
    public TargetInteractionState InteractionState { get; }
    public TargetGuardState GuardState { get; }

    public TargetStateMachine(Target target)
    {
        this.Target = target;
        
        //TODO 타겟의 이동 블럭들을 전부 가져온다

        IdleState = new TargetIdleState(this);
        ChasingState = new TargetChasingState(this);
        InteractionState = new TargetInteractionState(this);
        GuardState = new TargetGuardState(this);

        MovementSpeed = Target.Data.GroundData.BaseSpeed;
        RotationDamping = Target.Data.GroundData.BaseRotationDamping;
    }
}
