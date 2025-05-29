using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class TargetBaseState : IState
{
    protected TargetStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    protected Target target;


    public TargetBaseState(TargetStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Target.Data.GroundData;
        target = stateMachine.Target;
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
            if (target.isAction)
                return;

            // 플레이어가 시야 안에 락픽 애니메이션이 진행 중 이라면 바로 도주
            if (GameManager.Instance.Player.isLockpick)
            {
                stateMachine.ChangeState(stateMachine.RunAwayState);
                return;
            }

            // 플레이어가 시야 범위 안에 들어왔다면 초당 경계수치 증가
            stateMachine.AlertValue += stateMachine.SuspicionParams.increasePerSec * Time.deltaTime;
            stateMachine.AlertValue = Mathf.Min(stateMachine.AlertValue, stateMachine.SuspicionParams.maxValue);     //경계수치의 최댓값은 100(고정)
            Debug.Log($"Target 경계수치 : {stateMachine.AlertValue}");

            // 경계수치가 최대(100)이 되면 GuardState로 전환
            if (stateMachine.AlertValue >= stateMachine.SuspicionParams.maxValue)
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
            stateMachine.AlertValue -= stateMachine.SuspicionParams.decreasePerSec * Time.deltaTime;
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
        // 이동 해야하는 위치
        Vector3 movementDirection = GetMovementDirection();
        // NavMeshAgent를 이용하여 목적지 방향 벡터를 설정해줍니다.
        stateMachine.Target.Agent.SetDestination(movementDirection);

        RotateVelocity();
    }
    protected void RotateVelocity()
    {
        // NavMeshAgent에서 현재 움직이고 있는 방향
        NavMeshAgent agent = stateMachine.Target.Agent;
        Vector3 vel = agent.velocity;

        // 속도가 0에 가깝다면 회전처리를 중단합니다.
        if (vel.sqrMagnitude < 0.01f) return;

        // Target이 움직이는 방향
        Quaternion targetRot = Quaternion.LookRotation(vel.normalized);

        // 현재 회전에서 Target이 움직이는 방향으로 부드럽게 회전
        Transform tTrans = stateMachine.Target.transform;
        tTrans.rotation = Quaternion.Slerp(tTrans.rotation, targetRot, stateMachine.RotationDamping * Time.deltaTime);

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


    // Target의 시야 범위를 구하기 
    protected bool IsPlayerInSight()
    {
        if (target.isAction)
            return false;

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

        // y값은 머리 위치
        Vector3 headPosition = stateMachine.Target.transform.position + new Vector3(0, 1.5f, 0);

        // Target의 머리위치에서 부터 플레이어 콜라이더의 가장 가까운 위치를 구합니다.
        Vector3 playerClosetPoint = stateMachine.Target.playerCollider.ClosestPoint(headPosition);
        Vector3 toPlayer = playerClosetPoint - headPosition;

        float sqrDistance = toPlayer.sqrMagnitude;
        float maxDistance = stateMachine.ViewDistance * stateMachine.ViewDistance; 

        if (sqrDistance > maxDistance)
        {
            Debug.Log("사거리에 들어오지 않음");
            return false;
        }

        float horizontaolFov = stateMachine.ViewAngle;
        float verticalFov = stateMachine.ViewAngle;

        // 수평 시야각 검사
        Vector3 forward = stateMachine.Target.transform.forward;
        Vector3 forwardXZ = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
        Vector3 toPlayerXZ = Vector3.ProjectOnPlane(toPlayer, Vector3.up).normalized;
        float horizontalAngle = Vector3.Angle(forwardXZ, toPlayerXZ);
        if (horizontalAngle > horizontaolFov * 0.5f)
        {
            Debug.Log("수평 시야각 벗어남 Target");
            return false;
        }

        // 수직 시야각 검사
        float verticalAngle = Vector3.Angle(toPlayer, toPlayerXZ);
        if (verticalAngle > verticalFov * 0.5f)
        {
            Debug.Log("수직 시야각 벗어남 Target");
            return false;
        }

        float dist = Mathf.Sqrt(sqrDistance);
        int layerInteraciontColl = LayerMask.NameToLayer("InteractionCollider");

        int excludeMask = (1 << layerInteraciontColl);
        int occlusionMask = ~excludeMask;
        if (Physics.Raycast(headPosition, toPlayer.normalized, out RaycastHit hit, dist, occlusionMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject != stateMachine.Target.player)
            {
                Debug.Log($"시야가 가려짐 by {hit.collider.name}");
                return false;
            }
        }


        return true;

    }






}
