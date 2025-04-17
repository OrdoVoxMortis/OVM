using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInteractionState : TargetBaseState
{
    private float interactionDuration = 5f;        //기본 대기 시간
    private float timer;

    public TargetInteractionState(TargetStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Target.Agent.isStopped = true;

        timer = interactionDuration;
        //TODO : Interaction 애니메이션 넣기
    }

    public override void Exit()
    {
        base.Exit();

        //TODO : Interaction 애니메이션 중지
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
        interactionDuration = duration;
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
