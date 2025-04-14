using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSitState : PlayerGroundState
{
    Vector3 sitCenter = new Vector3(0, 0.77f, 0);
    Vector3 originCenter;

    public PlayerSitState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        originCenter = stateMachine.Player.Controller.center;
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.SitSpeedModifier;
        //AddInputActionCallbacks();
        //StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
        base.Enter();
        stateMachine.Player.Controller.height = 0.85f;
        stateMachine.Player.Controller.center = sitCenter;
        StartAnimation(stateMachine.Player.AnimationData.SitParameterHash);
    }

    public override void Exit()
    {
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
        stateMachine.Player.Controller.height = 1.7f;
        stateMachine.Player.Controller.center = originCenter;
        StopAnimation(stateMachine.Player.AnimationData.SitParameterHash);
        base.Exit();
    }

    protected override void OnSitStarted(InputAction.CallbackContext context)
    {
        //base.OnSitStarted(context);
        //stateMachine.ChangeState(stateMachine.SitState);

        stateMachine.ChangeState(stateMachine.IdleState);
    }

}
