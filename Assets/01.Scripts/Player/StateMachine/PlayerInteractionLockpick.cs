using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionLockpick : PlayerGroundState
{
    private readonly int triggerHash;
    private readonly float duration;
    private float timer;
    private readonly IInteractable interactable;

    private CinemachineBrain brain;

    public PlayerInteractionLockpick(PlayerStateMachine stateMachine, IInteractable interactable, float duration) : base(stateMachine)
    {
        triggerHash = stateMachine.Player.AnimationData.LockpickParameterHash;
        this.duration = duration;
        this.interactable = interactable;
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    public override void Enter()
    {
        // 카메라 잠금
        if (brain != null)
            brain.enabled = false;

        // 플레이어가 문 방향을 바라보도록 회전
        if (interactable is DoorController door)
        {
            Transform doorTransform = door.GetDoorHandleTrans();
            Vector3 dir = doorTransform.position - stateMachine.Player.transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
            {
                stateMachine.Player.transform.rotation = Quaternion.LookRotation(dir);
            }
        }

        // 애니메이션 속도 계산
        Animator ani = stateMachine.Player.Animator;
        float cliLen = 1f;
        foreach (var clip in ani.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "Interact Lockpick")
            {
                cliLen = clip.length;
                break;
            }
        }
        ani.speed = cliLen / duration;


        GameManager.Instance.Player.isLockpick = true;

        // 트리거 발동
        ani.SetTrigger(triggerHash);

        timer = 0f;
    }

    public override void HandleInput()
    {
    }

    public override void PhysicsUpdate()
    {
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration)
        {
            if (interactable is DoorController door)
            {
                door.Unlock();
                door.OpenDoor();
                
            }

            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void Exit()
    {
        Animator animator = stateMachine.Player.Animator;
        animator.speed = 1f;

        if (brain != null)
            brain.enabled = true;

        stateMachine.IsRunKeyHeld = false;

        GameManager.Instance.Player.isLockpick = false;

        base.Exit();
    }

}
