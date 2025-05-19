using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        stateMachine.npc.Agent.isStopped = false;

    }
    public override void Exit() 
    { 
        base.Exit();
        StopAnimation(stateMachine.npc.AnimationData.TalkingParameterHash);
    }

    public override void Update()
    {
        
        if (stateMachine.npc is Guard)
        {
            if (stateMachine.npc.behaviorType != BaseBehaviorType.Idle)
            {
                GuardWait();
            }
            else
            {
                GuardIdle();
            }
        }
        else if (stateMachine.npc.behaviorType == BaseBehaviorType.Idle)
        {
            TalkingIdle();
        }
        else base.Update();
        if (IsPlayerInSight())
        {
            IncreaseSuspicion();
            if (stateMachine.npc.CurSuspicion == stateMachine.npc.SuspicionParams.maxValue)
                stateMachine.ChangeState(stateMachine.ActionState);
        }
        else if (!isAlert)
        {
            DecreaseSuspicion();
            if (stateMachine.npc.CurSuspicion == 0) stateMachine.ChangeState(stateMachine.IdleState);
        }

    }

    private void IncreaseSuspicion() // true(max)면 ActionState
    {
        suspicionTimer += Time.deltaTime;
        if (suspicionTimer >= 1f)
        {
            suspicionTimer = 0f;
            Debug.Log(stateMachine.npc.CurSuspicion);
            stateMachine.npc.CurSuspicion = Mathf.Min(stateMachine.npc.SuspicionParams.maxValue, stateMachine.npc.CurSuspicion + stateMachine.npc.SuspicionParams.increasePerSec);
        }

    }

    private void DecreaseSuspicion()
    {
        suspicionTimer += Time.deltaTime;
        if (suspicionTimer >= 1f)
        {
            suspicionTimer = 0f;
            stateMachine.npc.CurSuspicion = Mathf.Max(0, stateMachine.npc.CurSuspicion - stateMachine.npc.SuspicionParams.decreasePerSec);
        }
    }
}
