using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRunAwayState : TargetBaseState
{
    public TargetRunAwayState(TargetStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.MovementInput *= 1.5f;
        StartAnimation(stateMachine.Target.AnimationData.RunParameterHash);

        if (stateMachine.Target.BlockNumber > 1)
        {
            stateMachine.Target.BlockNumber--;
        }

        if (stateMachine.Target.safeZone != null)
        {
            stateMachine.Target.Agent.SetDestination(stateMachine.Target.safeZone.transform.position);
        }

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Target.AnimationData.RunParameterHash);
    }

}
