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


    public float AlertValue = 0f;           // 경계수치
    public float AccumulateGuardTime = 0f;  // 누적된 경계 시간
    public float RemainingTravelTime = 0f;  // 다음 지점으로 도착해야 하는 시간


    public GameObject[] Blocks { get; private set; }
    public TargetIdleState IdleState { get; }
    public TargetChasingState ChasingState { get; }
    public TargetInteractionState InteractionState { get; }
    public TargetGuardState GuardState { get; }
    public TargetRunAwayState RunAwayState { get; }

    public TargetStateMachine(Target target)
    {
        this.Target = target;
        
        //TODO 타겟의 이동 블럭들을 전부 가져온다

        IdleState = new TargetIdleState(this);
        ChasingState = new TargetChasingState(this);
        InteractionState = new TargetInteractionState(this);
        GuardState = new TargetGuardState(this);
        RunAwayState = new TargetRunAwayState(this);

        MovementSpeed = Target.Data.GroundData.BaseSpeed;
        RotationDamping = Target.Data.GroundData.BaseRotationDamping;
    }


}
