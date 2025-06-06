using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float SquatSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get;  set; } = 1f;
    public float MaxRotationSpeed { get; private set; } = 720f;

    public float JumpForce { get; set;}

    public Transform MainCamTransform { get; set; }

    //상태들 (States)
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerSquatState SquatState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }

    public PlayerInterationSitState InteractionSitState { get; private set; }
    public PlayerInteractionLockpick InteractionLoackpick { get; private set; }

    public bool IsRunKeyHeld { get; set; }


    public PlayerStateMachine(Player player)
    {
        this.Player = player;

        MainCamTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        SquatState = new PlayerSquatState(this);

        //JumpState = new PlayerJumpState(this);
        FallState = new PlayerFallState(this);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
        SquatSpeed = Player.Data.GroundData.BaseSitSpeed;
        RotationDamping = Player.Data.GroundData.BaseRotationDamping;
    }

    public PlayerStateMachine(Player_Ghost player)
    {
        MainCamTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        SquatState = new PlayerSquatState(this);

        //JumpState = new PlayerJumpState(this);
        FallState = new PlayerFallState(this);
    }

}
