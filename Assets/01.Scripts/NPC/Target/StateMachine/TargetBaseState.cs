using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

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
        UpdateAlertValue();
    }

    protected virtual void UpdateAlertValue()
    {
        if (IsPlayerInSight())
        {
            // 플레이어가 시야 범위 안에 들어왔다면 초당 경계수치 증가
            stateMachine.AlertValue += 50f * Time.deltaTime;
            stateMachine.AlertValue = Mathf.Min(stateMachine.AlertValue, 100f);     //경계수치의 최댓값은 100(고정)

            // 경계수치가 최대(100)이 되면 GuardState로 전환
            if (stateMachine.AlertValue >= 100f)
            {
                // 현재 상태와 남은 시간을 저장
                stateMachine.SaveCurrentState(this, GetRemainingActionTime());

                stateMachine.ChangeState(stateMachine.GuardState);
                return;
            }
        }
        else
        {
            //  플레이어가 시야에 보이지 않을 경우 경계수치를 감소
            //  0이면 작동 안하게, 경계모드일때도 작동 안하게 해야함
            //TODO Target 경계수치 내려가는 값 필요
            stateMachine.AlertValue -= 20f * Time.deltaTime;
            stateMachine.AlertValue = Mathf.Max(stateMachine.AlertValue, 0f);
        }
    }

    //남은 시간을 저장, 이어받기를 위한 부분

    public virtual float GetRemainingActionTime()
    {
        return 0f;
    }

    public virtual void ResumeState(float remainingTime)
    {

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

        stateMachine.Target.Agent.SetDestination(movementDirection);

        RotateVelocity();
        //Rotate(movementDirection - stateMachine.Target.transform.position);

    }

    private Vector3 GetMovementDirection()
    {
        if (stateMachine.Blocks == null || stateMachine.Blocks.Length == 0)
        {
            Debug.LogWarning("blocks가 등록되어있지 않습니다.");
            return stateMachine.Target.transform.position;
        }

        int blockNumber = stateMachine.Target.BlockNumber;


        return stateMachine.Blocks[blockNumber].transform.position;
    }

    protected void RotateVelocity()
    {
        NavMeshAgent agent = stateMachine.Target.Agent;
        Vector3 vel = agent.velocity;

        if (vel.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(vel.normalized);
        Transform tTrans = stateMachine.Target.transform;
        tTrans.rotation = Quaternion.Slerp(tTrans.rotation, targetRot, stateMachine.RotationDamping * Time.deltaTime);

    }


    //private void Rotate(Vector3 direction)
    //{
    //    if (direction != Vector3.zero)
    //    {
    //        Transform playerTransform = stateMachine.Target.transform;
    //        Quaternion targetRotation = Quaternion.LookRotation(direction);
    //        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);

    //    }
    //}

    // Target의 시야 범위를 구하기 
    protected bool IsPlayerInSight()
    {
        if (stateMachine.Target.player == null)
        {
            Debug.LogError("Player가 등록되있지 않습니다.");
            return false;
        }

        if (stateMachine.Target.playerCollider == null)
        {
            Debug.LogError("Player Collider가 등록되있지 않습니다.");
            return false;
        }

        Vector3 headPosition = stateMachine.Target.transform.position + new Vector3(0, 1.5f, 0); // y값은 머리 위치

        Vector3 playerClosetPoint = stateMachine.Target.playerCollider.ClosestPoint(headPosition); // Target의 머리위치에서 부터 플레이어 콜라이더의 가장 가까운 위치를 구합니다.

        float sqrDistance = (playerClosetPoint - headPosition).sqrMagnitude;
        const float maxDistance = 8f * 8f;      //TODO : 두개의 값 다 Target의 시야 길이를 넣어야 합니다.

        if (sqrDistance > maxDistance)
            return false;

        Vector3 directionToPlayer = (playerClosetPoint - headPosition).normalized;
        float angle = Vector3.Angle(stateMachine.Target.transform.forward, directionToPlayer);
        if (angle > 60f)
            return false;

        //Raycast를 통해서 머리 위치에서 closetPoint 까지의 장애물을 확인합니다
        float distance = Mathf.Sqrt(sqrDistance);
        if (Physics.Raycast(headPosition, directionToPlayer, out RaycastHit hit, distance))
        {
        // 시야 범위내에 물건이 있다면 확인 불가능
            if (hit.collider.gameObject != stateMachine.Target.player)
                return false;
        }

        return true;

    }






}
