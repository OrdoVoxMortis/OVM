using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSquatState : PlayerGroundState
{
    Vector3 sitCenter = new Vector3(0, 0.77f, 0);
    Vector3 originCenter;
    float originHeight;

    public PlayerSquatState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        originCenter = stateMachine.Player.Controller.center;
        originHeight = stateMachine.Player.Controller.height;
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Player.isSquat = true;
        stateMachine.MovementSpeedModifier = groundData.SitSpeedModifier;
        stateMachine.Player.Controller.height = 0.85f;
        stateMachine.Player.Controller.center = sitCenter;

        StartAnimation(stateMachine.Player.AnimationData.SquatParameterHash);
    }

    public override void Exit()
    {
        stateMachine.Player.isSquat = false;
        stateMachine.Player.Controller.height = originHeight;
        stateMachine.Player.Controller.center = originCenter;

        StopAnimation(stateMachine.Player.AnimationData.SquatParameterHash);
        base.Exit();
    }

    protected override void OnRunStarted(InputAction.CallbackContext context)
    {
        if (stateMachine.Player.isSquat) 
            stateMachine.ChangeState(stateMachine.RunState);
    }


    protected override void OnSquatStarted(InputAction.CallbackContext context)
    {
        if (stateMachine.Player.isSquat) 
            stateMachine.ChangeState(stateMachine.IdleState);

    }

    //protected override void OnJumpStarted(InputAction.CallbackContext context)
    //{
    //    if (stateMachine.Player.Controller.isGrounded)
    //        stateMachine.ChangeState(stateMachine.JumpState);
    //}

    protected override void OnMovementStarted(InputAction.CallbackContext context)
    {
        
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        
    }

}
