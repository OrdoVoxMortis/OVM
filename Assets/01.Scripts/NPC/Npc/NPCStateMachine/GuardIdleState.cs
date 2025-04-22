using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardIdleState : NpcBaseState
{
    private float waitTimer = 0f;
    private float cooldownTimer = 0f;
    private bool isWaiting = false;
    public GuardIdleState(NpcStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.npc.Agent.isStopped = false;
        waitTimer = 0f;
        waitTimer = 0f;
        isWaiting = false;
    }

    public override void Update()
    {
        if (GameManager.Instance.SimulationMode)
        {
            stateMachine.npc.Agent.isStopped = true;
            return;
        }

        var agent = stateMachine.npc.Agent;

        if (!isWaiting)
        {
            waitTimer += Time.deltaTime;

            moveTimer += Time.deltaTime;
            if(moveTimer >= moveDelay)
            {
                agent.SetDestination(GetRandomPointInArea(stateMachine.npc.Area));
                moveTimer = 0f;
            }

            bool isMoving = !agent.pathPending && agent.remainingDistance > agent.stoppingDistance;
            if (isMoving) StartAnimation("Walk");
            else StopAnimation("Walk");

            if(waitTimer >= 3f)
            {
                if (stateMachine.npc is Guard guard)
                {
                    agent.SetDestination(guard.GetWaitPosition().transform.position);
                    isWaiting = true;
                    waitTimer = 0f;
                }
            }
        }
        else // 대기중
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                StopAnimation("Walk");
                cooldownTimer += Time.deltaTime;

                if(cooldownTimer >= 5f)
                {
                    isWaiting = false;
                    waitTimer = 0f;
                    agent.SetDestination(GetRandomPointInArea(stateMachine.npc.Area));
                }
            }
        }
    }

}
