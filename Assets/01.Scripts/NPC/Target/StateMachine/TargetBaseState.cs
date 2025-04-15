using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBaseState : IState
{
    protected TargetStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public TargetBaseState(TargetStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Target.Data.GroundData;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void HandleInput()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
        Move();
    }

    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Target.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Target.Animator.SetBool(animatorHash, false);
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Rotate(movementDirection);
        Move(movementDirection);

    }

    private Vector3 GetMovementDirection()
    {
        int blockNumber = stateMachine.Target.BlockNumber;
        Vector3 dir = (stateMachine.Blocks[blockNumber].transform.position - stateMachine.Target.transform.position);

        return dir;
    }

    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Target.Controller.Move(((direction * movementSpeed)) * Time.deltaTime);
    }

    private float GetMovementSpeed()
    {
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Transform playerTransform = stateMachine.Target.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);

        }
    }

    // Target의 시야 범위를 구하기 

    // Target의 시야 범위 내에 플레이어가 들어왔다면 경계모드 및 경계시간 증가 (sqrMagnitude)





}
