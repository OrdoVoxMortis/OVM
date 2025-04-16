using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAlertState : NpcBaseState
{
    private float suspicionTimer = 0f;
    public NpcAlertState(NpcStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("alert");
        stateMachine.npc.CurAlertTime = 0f;
    }
    public override void Exit() { base.Exit(); }

    public override void Update()
    {
        stateMachine.npc.CurAlertTime += Time.deltaTime;
        if (IncreaseSuspicion()) stateMachine.ChangeState(stateMachine.ActionState);
        else
        {
            DecreaseSuspicion();
            if(stateMachine.npc.CurSuspicion == 0) stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    private bool IncreaseSuspicion() // true면 ActionState
    {
        suspicionTimer += Time.deltaTime;
        if (suspicionTimer >= 1f)
        {
            suspicionTimer = 0f;
            if (stateMachine.npc.CurSuspicion < stateMachine.npc.SuspicionParams.maxValue)
            {
                stateMachine.npc.CurSuspicion += stateMachine.npc.SuspicionParams.increasePerSec;
                return false;
            }
            return true;
        }
        return false;
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
