using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIdleState : TargetBaseState
{
    public TargetIdleState(TargetStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        base.Enter();
        StartAnimation(stateMachine.Target.AnimationData.GroundParameterHash);
        StartAnimation(stateMachine.Target.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Target.AnimationData.GroundParameterHash);
        StopAnimation(stateMachine.Target.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.MovementInput != Vector2.zero) // 시야 범위 안에 플레이어가 들어왔다면
        {
            stateMachine.ChangeState(stateMachine.GuardState);
            return;
        }
    }

}
