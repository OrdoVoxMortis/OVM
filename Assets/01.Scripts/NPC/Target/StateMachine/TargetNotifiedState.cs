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

        Vector3 lookDir = (stateMachine.Target.FriendPosition - stateMachine.Target.transform.position);
        lookDir.y = 0;
        if (lookDir.sqrMagnitude > 0.01f)
        {
            stateMachine.Target.transform.rotation = Quaternion.LookRotation(lookDir);
        }

        StartAnimation(stateMachine.Target.AnimationData.IdleParameterHash);
        timer = 10.5f;
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
