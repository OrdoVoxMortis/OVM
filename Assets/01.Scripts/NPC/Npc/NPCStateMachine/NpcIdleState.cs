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
        base.Update();
        if (!isAction)
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
