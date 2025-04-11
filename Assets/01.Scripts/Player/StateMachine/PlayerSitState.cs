using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSitState : PlayerGroundState
{
    public PlayerSitState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.SitParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.SitParameterHash);
    }

    protected override void OnSitStarted(InputAction.CallbackContext context)
    {
        base.OnSitStarted(context);
        stateMachine.ChangeState(stateMachine.SitState);
    }

    public override void Update()
    {
        base.Update();
    }


}
