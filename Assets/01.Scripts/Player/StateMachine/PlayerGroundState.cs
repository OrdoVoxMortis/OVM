using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void PhysicsUpdate()        // 플레이어가 공중이라면 
    {
        base.PhysicsUpdate();

        if (!stateMachine.Player.Controller.isGrounded && stateMachine.Player.Controller.velocity.y < Physics.gravity.y * Time.fixedDeltaTime)
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }

    }

    protected override void OnSquatStarted(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.isUIActive) return;
        stateMachine.ChangeState(stateMachine.SquatState);
        base.OnSquatStarted(context);
    }

    protected override void OnMovementStarted(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.isUIActive) return;
        stateMachine.MovementInput = context.ReadValue<Vector2>();
        if (stateMachine.IsRunKeyHeld)
            stateMachine.ChangeState(stateMachine.RunState);
        else
            stateMachine.ChangeState(stateMachine.WalkState);

        base.OnMovementStarted(context);
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.isUIActive) return;
        if (stateMachine.MovementInput == Vector2.zero) return;
        stateMachine.ChangeState(stateMachine.IdleState);

        base.OnMovementCanceled(context);
    }

    //protected override void OnJumpStarted(InputAction.CallbackContext context)
    //{
    //    if (UIManager.Instance.isUIActive) return;
    //    if (stateMachine.Player.Controller.isGrounded)
    //    {

    //        base.OnJumpStarted(context);
    //        stateMachine.ChangeState(stateMachine.JumpState);
    //    }
    //}

}
