using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetNotifiedState : TargetBaseState
{
    private float timer;
    public TargetNotifiedState(TargetStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Target.Agent.isStopped = true;
        StartAnimation(stateMachine.Target.AnimationData.IdleParameterHash);
        timer = 2f;
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            StopAnimation(stateMachine.Target.AnimationData.IdleParameterHash);
            stateMachine.ChangeState(stateMachine.RunAwayState);
        }
    }

    public override void Exit()
    {
        base .Exit();
        stateMachine.Target.Agent.isStopped = false;
    }

}
