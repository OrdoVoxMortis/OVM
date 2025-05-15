using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterationSitState : PlayerGroundState
{
    private readonly Transform seatPoint;
    private readonly float sitdownDuration;
    private readonly float standupDuration;

    private readonly float standupAniDuration = 2.14f;
    private readonly float sitdownAniDuration = 2.14f;

    private readonly int sitHash;

    private float timer = 0f;
    public bool isStandUp = false;


    public PlayerInterationSitState(PlayerStateMachine stateMachine, Transform seatPoint, 
        float sitDownDuration, float standupDuration)
        : base(stateMachine)
    {
        this.seatPoint = seatPoint;
        this.sitdownDuration = sitDownDuration;
        this.standupDuration = standupDuration;
        sitHash = stateMachine.Player.AnimationData.SitParameterHash;
    }

    public override void Enter()
    {
        base.Enter();

        //stateMachine.Player.Input.UnsubscribeAllInputs(GameManager.Instance.Player.Interaction);
        //stateMachine.Player.Controller.enabled = false;

        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);

        // 플레이어 위치, 회전 맞추기
        Transform playerTrans = stateMachine.Player.transform;
        playerTrans.position = seatPoint.position;
        playerTrans.rotation = seatPoint.rotation;

        Animator ani = stateMachine.Player.Animator;
        ani.speed = sitdownAniDuration / sitdownDuration;

        // 앉기 애니메이션 시작
        Debug.Log($"[SitState] sitHash = {sitHash} = true");
        StartAnimation(sitHash);

    }

    public override void HandleInput()
    {
        if (!isStandUp && stateMachine.Player.Input.playerActions.Interection.triggered)
        {
            isStandUp = true;

            Animator ani = stateMachine.Player.Animator;
            ani.speed = standupAniDuration / standupDuration;

            StopAnimation(sitHash);
        }
    }

    public override void Update()
    {
        if (isStandUp)
        {
            timer += Time.deltaTime;
            if (timer >= standupDuration)
                stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void Exit()
    {
        Animator ani = stateMachine.Player.Animator;
        ani.speed = 1f;

        //stateMachine.Player.Controller.enabled = true;
        //stateMachine.Player.Input.SubscribeAllInputs();

        base.Exit();
    }




}
