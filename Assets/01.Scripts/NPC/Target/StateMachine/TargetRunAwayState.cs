using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetRunAwayState : TargetBaseState
{
    private const float ArrivalThreshold = 1f;
    public TargetRunAwayState(TargetStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.Target.Agent.isStopped = false;
        stateMachine.Target.Agent.speed = 2.5f;
        StartAnimation(stateMachine.Target.AnimationData.RunParameterHash);

        if (stateMachine.Target.safeZone != null)
        {
            stateMachine.Target.Agent.SetDestination(stateMachine.Target.safeZone.transform.position);
        }

    }

    public override void Update()
    {
        NavMeshAgent agent = stateMachine.Target.Agent;
        var safePos = stateMachine.Target.safeZone.transform.position;
        agent.SetDestination(safePos);

        RotateVelocity();

        float dist = Vector3.Distance(stateMachine.Target.transform.position, safePos);
        if (dist <= ArrivalThreshold + agent.stoppingDistance)
        {
            // 도착 이후 실행 할 동작이 있으면 실행(현재는 Idle)
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Target.AnimationData.RunParameterHash);
    }

}
