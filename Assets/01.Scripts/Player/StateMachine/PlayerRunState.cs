using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        base.Enter();
        
        // 현재 애니메이터 상태 진행도를 가져와 Run 애니메이션에 적용
        Animator anim = stateMachine.Player.Animator;
        int runHash = stateMachine.Player.AnimationData.RunParameterHash;
        StartAnimation(runHash);

        // 현재 레이어의 상태 정보
        AnimatorStateInfo curState = anim.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = (curState.normalizedTime) % 1f;

        // Run 상태로 즉시 전환하면서 같은 진행 비율에서 재생을 시작합니다
        anim.Play(runHash, 0, normalizedTime);

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (!stateMachine.IsRunKeyHeld)
        {
            if (stateMachine.MovementInput != Vector2.zero)
                stateMachine.ChangeState(stateMachine.WalkState);
            else
                stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

}
