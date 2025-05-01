using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TargetGuardState : TargetBaseState
{
    private enum GuardSubState { Rotating, LookAround}
    private GuardSubState subState;

    // 상태를 계산할 타이머와 지속시간
    private float phaseTimer;
    private float phaseDuration;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    // GuardeState의 전체 시간
    private float currentGuardTimer;
    private float accumulatedGuardTime;

    public TargetGuardState(TargetStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //NavMeshAgent의 이동을 멈춘다
        stateMachine.Target.Agent.isStopped = true;
        // 최소 경계 시간을 가져온다.
        currentGuardTimer = stateMachine.MinAlertTime;
        // 경계 지속 시간
        accumulatedGuardTime = 0f;
        StartAnimation(stateMachine.Target.AnimationData.IdleParameterHash);

        subState = GuardSubState.Rotating;
        phaseDuration = 0.6f;
        phaseTimer = phaseDuration;
        startRotation = stateMachine.Target.transform.rotation;
        targetRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Target.AnimationData.IdleParameterHash);

        stateMachine.Target.Agent.isStopped = false;

    }

    public override void Update()
    {
        float delta = Time.deltaTime;

        currentGuardTimer -= delta;
        accumulatedGuardTime += delta;

        if (IsPlayerInSight())
        {
            // 플레이어가 시야안에 들어왔다면 최소시간을 다시 갱신
            currentGuardTimer = stateMachine.MinAlertTime;
        }

        // 경계 지속 시간이 최대 경계 시간이 되었다면
        if (accumulatedGuardTime >= stateMachine.MaxAlertTime)
        {
            stateMachine.ChangeState(stateMachine.RunAwayState);
            return;
        }

        // GuardeState의 경계모드가 끝나면 복귀
        if (currentGuardTimer <= 0f)
        {
            TargetBaseState prev = stateMachine.PreviousState;
            if (prev != null)
            {
                stateMachine.ChangeState(prev);
                prev.ResumeState(stateMachine.PreviousStateRemainingTime);
            }
            else
            {
                // 이전 상태 정보가 없다면 Chasing으로
                stateMachine.ChangeState(stateMachine.ChasingState);
            }
            return;
        }

        phaseTimer -= delta;
        Transform tTrans = stateMachine.Target.transform;
        if (subState == GuardSubState.Rotating)
        {
            float tFactor = (phaseDuration - phaseTimer) / phaseDuration;
            tTrans.rotation = Quaternion.Slerp(startRotation, targetRotation, tFactor);

            if (phaseTimer <= 0f)
            {
                subState = GuardSubState.LookAround;

                phaseDuration = Random.Range(0.4f, 1f);
                phaseTimer = phaseDuration;
            }

        }
        else if (subState == GuardSubState.LookAround)
        {
            // 좌우로 살짝식만 회전
            float frequency = 1f;
            float amplitude = 10f;
            float lookAroundAngle = (phaseDuration - phaseTimer) * 2f * Mathf.PI * frequency;
            Quaternion offset = Quaternion.Euler(0f, amplitude * Mathf.Sin(lookAroundAngle), 0f);
            tTrans.rotation = targetRotation * offset;

            if (phaseTimer <= 0f)
            {
                subState = GuardSubState.Rotating;
                phaseDuration = 0.6f;
                phaseTimer = phaseDuration;

                startRotation = tTrans.rotation;
                targetRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            }
        }


    }


}
