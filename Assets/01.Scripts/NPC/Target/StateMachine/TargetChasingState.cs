using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetChasingState : TargetBaseState
{
    public TargetChasingState(TargetStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        base.Enter();
        StartAnimation(stateMachine.Target.AnimationData.GroundParameterHash);
        StartAnimation(stateMachine.Target.AnimationData.WalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Target.AnimationData.GroundParameterHash);
        StopAnimation(stateMachine.Target.AnimationData.WalkParameterHash);
    }

    public override void Update()
    {
        base.Update();

        // 만약 시야 범위 안에 플레이어가 들어왔다면 경계수치를 증가시킨다.
        // 경계수치가 100 이라면 경계모드로 변경+(경계모드 안에 플레이어가 시야에 보였다면 시간 갱신) (경계모드일 때 다음 블럭으로 이동하는 시간을 멈추기)
        // 경계 모드일 때 플레이어가 다시 시야에 보였다면 시간 갱신

        // 경계모드 시간이 지났다면 다시 행동 계시
    }



}
