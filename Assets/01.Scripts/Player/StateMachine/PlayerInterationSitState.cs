using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInterationSitState : PlayerGroundState
{
    private InputAction interactAction;

    private readonly Transform seatPoint;
    private readonly float sitdownDuration;
    private readonly float standupDuration;

    private readonly float standupAniDuration = 2.14f;
    private readonly float sitdownAniDuration = 2.14f;

    private readonly int sitHash;

    private float sitTimer;
    private bool sitFinished = false;
    public bool isStandUp = false;


    public PlayerInterationSitState(PlayerStateMachine stateMachine, Transform seatPoint, 
        float sitDownDuration, float standupDuration)
        : base(stateMachine)
    {
        this.seatPoint = seatPoint;
        this.sitdownDuration = sitDownDuration;
        this.standupDuration = standupDuration;
        sitHash = stateMachine.Player.AnimationData.SitParameterHash;

        interactAction = stateMachine.Player.Input.playerActions.Interection;
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.Player.Input.UnsubscribeAllInputs(stateMachine.Player.Interaction);

        sitTimer = 0f;
        sitFinished = false;

        // 플레이어 위치, 회전 맞추기
        Transform playerTrans = stateMachine.Player.transform;
        playerTrans.position = seatPoint.position;
        playerTrans.rotation = seatPoint.rotation;

        Animator ani = stateMachine.Player.Animator;
        ani.speed = sitdownAniDuration / sitdownDuration;

        GameManager.Instance.Player.isSit = true;
        // 앉기 애니메이션 시작
        Debug.Log($"[SitState] sitHash = {sitHash} = true");
        StartAnimation(sitHash);

    }

    public override void HandleInput()
    {

        if (!isStandUp && sitFinished && interactAction.triggered)
        {
            isStandUp = true;

            Animator ani = stateMachine.Player.Animator;
            ani.speed = standupAniDuration / standupDuration;

            StopAnimation(sitHash);
        }
    }

    public override void Update()
    {
        if (!sitFinished)
        {
            sitTimer += Time.deltaTime;
            if (sitTimer >= sitdownDuration)
            {
                // 앉는모션 끝
                sitFinished = true;
                sitTimer = 0f;
            }
        }
        else  if (isStandUp)
        {
            sitTimer += Time.deltaTime;
            if (sitTimer >= standupDuration)
                stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void Exit()
    {
        Animator ani = stateMachine.Player.Animator;
        ani.speed = 1f;
        GameManager.Instance.Player.isSit = false;

        stateMachine.Player.Input.SubscribeAllInputs();

        base.Exit();
    }




}
