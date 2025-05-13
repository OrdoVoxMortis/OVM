using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcIdleState : NpcBaseState
{
    private float suspicionTimer = 0f;
    public NpcIdleState(NpcStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.npc.Agent.isStopped = false;

        Debug.Log("idle");
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (stateMachine.npc is Guard)
        {
            GuardWait();
        }
        else if (stateMachine.npc.behaviorType == BaseBehaviorType.Idle)
        {
            stateMachine.npc.Agent.isStopped = true;
            StopAnimation("Walk");
            StartAnimation("Talking");
        }
        else base.Update();

        if (!stateMachine.npc.IsAction)
        {
            if (stateMachine.npc.CurAlertTime > 0)
                stateMachine.npc.CurAlertTime -= Time.deltaTime;
            else DecreaseSuspicion();

            if (IsPlayerInSight())
            {
                stateMachine.ChangeState(stateMachine.AlertState);
            }
        }

    }




    private void DecreaseSuspicion()
    {
        suspicionTimer += Time.deltaTime;
        if (suspicionTimer >= 1f)
        {
            suspicionTimer = 0f;
            stateMachine.npc.CurSuspicion = Mathf.Max(0, stateMachine.npc.CurSuspicion -= stateMachine.npc.SuspicionParams.decreasePerSec);
        }
    }

}
