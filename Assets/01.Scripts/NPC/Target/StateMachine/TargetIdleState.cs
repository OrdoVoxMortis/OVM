using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIdleState : TargetBaseState
{
    private float idleDuration = 5f;        //기본 대기 시간
    private float timer;

    public TargetIdleState(TargetStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Target.Agent.isStopped = true;
        base.Enter();
        StartAnimation(stateMachine.Target.AnimationData.GroundParameterHash);
        StartAnimation(stateMachine.Target.AnimationData.IdleParameterHash);
        timer = idleDuration;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Target.AnimationData.GroundParameterHash);
        StopAnimation(stateMachine.Target.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        UpdateAlertValue();
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            stateMachine.Target.BlockNumber++;
            stateMachine.Target.Agent.isStopped = false;
            stateMachine.ChangeState(stateMachine.ChasingState);
        }
    }

    public void SetDuration(float duration)
    {
        idleDuration = duration;
    }

    public override float GetRemainingActionTime()
    {
        return timer;
    }

    public override void ResumeState(float remainingTime)
    {
        timer = remainingTime;
    }



}
