using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBaseState : IState
{
    protected TargetStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    protected GameObject player;

    public TargetBaseState(TargetStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Target.Data.GroundData;
        player = GameManager.Instance.Player.gameObject;
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

    protected virtual void UpdateAlertValue()
    {
        if (IsPlayerInSight())
        {
            // 플레이어가 시야 범위 안에 들어왔다면 초당 경계수치 증가
            stateMachine.AlertValue += 50f * Time.deltaTime;
            stateMachine.AlertValue = Mathf.Min(stateMachine.AlertValue, 100f);     //경계수치의 최댓값은 100(고정)

            // 경계수치가 최대(100)이 되면 GuardState로 전환
            //if (stateMachine.AlertValue >= 100f && stateMachine.)
        }
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
    protected bool IsPlayerInSight()
    {
        if (player == null)
        {
            Debug.LogError("Player가 등록되있지 않습니다.");
            return false;
        }

        Collider playerCollider = player.GetComponent<Collider>();
        if (playerCollider == null)
        {
            Debug.LogError("Player Collider가 등록되있지 않습니다.");
            return false;
        }

        Vector3 headPosition = stateMachine.Target.transform.position + new Vector3(0, 1.5f, 0); // 머리 위치

        Vector3 playerClosetPoint = playerCollider.ClosestPoint(headPosition); // Target의 머리위치에서 부터 플레이어 콜라이더의 가장 가까운 위치를 구합니다.

        float sqrDistance = (playerClosetPoint - headPosition).sqrMagnitude;
        const float maxDistance = 8f * 8f;      //TODO : 두개의 값 다 Target의 시야 길이를 넣어야 합니다.

        if (sqrDistance > maxDistance)
            return false;

        Vector3 directionToPlayer = (playerClosetPoint - headPosition).normalized;
        float angle = Vector3.Angle(stateMachine.Target.transform.forward, directionToPlayer);
        if (angle > 60f)
            return false;

        //Raycast를 통해서 머리 위치에서 closetPoint 까지의 장애물을 체크합니다
        float distance = Mathf.Sqrt(sqrDistance);
        if (Physics.Raycast(headPosition, directionToPlayer, out RaycastHit hit, distance))
        {
        // 시야 범위내에 물건이 있다면 확인 불가능
            if (hit.collider.gameObject != player)
                return false;
        }

        return true;

    }






}
