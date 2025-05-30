using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerInterationSitState : PlayerBaseState
{
    private InputAction interactAction;
    private readonly InteractionChair chair;

    private readonly Transform seatPoint;
    private readonly float sitdownDuration;
    private readonly float standupDuration;

    private readonly float standupAniDuration = 2.14f;
    private readonly float sitdownAniDuration = 2.14f;

    private readonly bool isSkipSitDown;

    private readonly int sitHash;
    private readonly int skipHash;

    private float sitTimer;
    private bool isSitFinished = false;
    public bool isStandUp = false;

    private bool hasSubStart = false;


    public PlayerInterationSitState(PlayerStateMachine stateMachine, InteractionChair chair,
        Transform seatPoint, float sitDownDuration, float standupDuration, bool isSkipSitDown)
        : base(stateMachine)
    {
        this.seatPoint = seatPoint;
        this.chair = chair;
        this.sitdownDuration = sitDownDuration;
        this.standupDuration = standupDuration;
        sitHash = stateMachine.Player.AnimationData.SitParameterHash;
        skipHash = stateMachine.Player.AnimationData.SkipSitDownParameterHash;

        interactAction = stateMachine.Player.Input.playerActions.Interection;
        this.isSkipSitDown = isSkipSitDown;
    }

    public override void Enter()
    {
        base.Enter();

        //stateMachine.Player.Input.UnsubscribeAllInputs(stateMachine.Player.Interaction);
        //stateMachine.Player.Input.playerActions.Interection.Disable();

        sitTimer = 0f;
        isSitFinished = false;
        isStandUp = false;


        // 플레이어 위치, 회전 맞추기
        Transform playerTrans = stateMachine.Player.transform;
        playerTrans.position = seatPoint.position;
        playerTrans.rotation = seatPoint.rotation;

        Animator ani = stateMachine.Player.Animator;

        if (SceneManager.GetActiveScene().name == "Lobby_Scene" && !GameManager.Instance.gameStarted)
        {
            ani.speed = 1f;
            StartAnimation(skipHash);
            SubscribeToStart();
            GameManager.Instance.Player.isSit = true;
            StartAnimation(sitHash);
            return;
        }

        //if (SceneManager.GetActiveScene().name == "Lobby_Scene" && GameManager.Instance.gameStarted)
        //{
        //    GameManager.Instance.Player.isSit = false;

        //    PlayerController input = stateMachine.Player.Input;
        //    input.playerActions.Enable();
        //    input.SubscribeAllInputs();

        //    var camInput = input.playerCamera.GetComponent<CinemachineInputProvider>();
        //    if (camInput != null)
        //        camInput.enabled = true;

        //    stateMachine.ChangeState(stateMachine.IdleState);
        //    return;
        //}


        {
            ani.speed = sitdownAniDuration / sitdownDuration;
            isSitFinished = false;
            GameManager.Instance.Player.isSit = true;
            Debug.Log($"[SitState] sitHash = {sitHash} = true");
            StartAnimation(sitHash);

        }


    }

    private void SubscribeToStart()
    {
        if (hasSubStart) return;
        hasSubStart = true;

        sitTimer = 0f;
        isSitFinished = true;
        isStandUp = false;

        UI_Start.OnStartButtonPressed += HandleStartPressed;
    }

    private void HandleStartPressed()
    {
        if (!isSitFinished) return;

        UI_Start.OnStartButtonPressed -= HandleStartPressed;

        sitTimer = 0f;
        isStandUp = true;

        Animator ani = stateMachine.Player.Animator;
        ani.speed = standupAniDuration / standupDuration;

        interactAction.Enable();

        StopAnimation(sitHash);
        StopAnimation(skipHash);
    }

    public override void HandleInput()
    {

        if (!isStandUp && isSitFinished && interactAction.triggered)
        {
            interactAction.Disable();
            isStandUp = true;

            Animator ani = stateMachine.Player.Animator;
            ani.speed = standupAniDuration / standupDuration;

            StopAnimation(sitHash);
        }
    }

    public override void Update()
    {
        if (!isSitFinished && !isSkipSitDown)
        {
            sitTimer += Time.deltaTime;
            if (sitTimer >= sitdownDuration)
            {
                // 앉는모션 끝
                isSitFinished = true;
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

        if (hasSubStart)
            UI_Start.OnStartButtonPressed -= HandleStartPressed;

        chair.EnableTrigger();

        interactAction.Enable();
        stateMachine.Player.Input.SubscribeAllInputs();

        base.Exit();
    }




}
