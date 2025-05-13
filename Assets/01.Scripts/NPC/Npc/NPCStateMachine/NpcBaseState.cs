using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class NpcBaseState : IState
{
    protected NpcStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    protected bool isAlert = true;
    protected bool isAction = false;
    protected float moveTimer = 0f;

    //Guard
    protected float waitTimer = 0f;
    protected bool isWaiting = false;
    protected float cooldownTimer = 0f;
    public NpcBaseState(NpcStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.npc.Data.GroundData;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public void HandleInput()
    {

    }

    public virtual  void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
        if (GameManager.Instance.SelectedBGM != null)
        {

            if (stateMachine.npc.isColliding)
            {
                StopAnimation("Walk");
                stateMachine.npc.Agent.isStopped = true;
            }
            else
            {
                stateMachine.npc.Agent.isStopped = false;
                moveTimer += Time.deltaTime;
                if (moveTimer >= stateMachine.npc.moveDelay)
                {
                    Move();
                    moveTimer = 0f;
                }
            }
            var agent = stateMachine.npc.Agent;
            bool isMoving = !agent.pathPending && agent.remainingDistance > agent.stoppingDistance;
            if (isMoving)
            {
                RotateVelocity();
                StartAnimation("Walk");
            }
            else StopAnimation("Walk");
        }
        else
        {
            StopAnimation("Walk");
            stateMachine.npc.Agent.isStopped = true;
        }
    }

    public void Move()
    {
        Transform npc = stateMachine.npc.transform;
        Vector3 nextPosition = GetRandomPointInArea(stateMachine.npc.Area);

        Vector3 forward = npc.forward;
        Vector3 nextDir = (nextPosition - npc.position).normalized;
        float crossY = Vector3.Cross(forward, nextDir).y;

        if(Mathf.Abs(crossY) > 0.01f)
        {
            if (crossY > 0f) StartAnimation("TurnLeft");
            else StartAnimation("TurnRight");
        }
        StopAnimation("TurnRight");
        StopAnimation("TurnLeft");
        stateMachine.npc.Agent.SetDestination(nextPosition);
    }
    protected void StartAnimation(string anim)
    {
        stateMachine.npc.Animator.SetBool(anim, true);
    }

    protected void StopAnimation(string anim)
    {
        stateMachine.npc.Animator.SetBool(anim, false);
    }

    public Vector3 GetRandomPointInArea(BoxCollider collider)
    {
        Vector3 center = collider.bounds.center;
        Vector3 size = collider.bounds.size;

        float randomX = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float randomZ = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        return new Vector3(randomX, center.y, randomZ);
    }
    public bool IsPlayerInSight() //true -> 경계
    {
        Transform player = GameManager.Instance.Player.transform;
        Vector3 directionPlayer = (player.position - stateMachine.npc.transform.position).normalized;
        float angle = Vector3.Angle(stateMachine.npc.transform.forward, directionPlayer);

        float distance = Vector3.Distance(stateMachine.npc.transform.position, player.position);
        if (angle > stateMachine.npc.ViewAngle / 2f || distance > stateMachine.npc.ViewDistance)
        {
            return false;
        }

        //벽
        Vector3 headPosition = stateMachine.npc.transform.position + new Vector3(0, 1.5f, 0);

        Vector3 playerClosetPoint = stateMachine.npc.playerCollider.ClosestPoint(headPosition); 

        float sqrDistance = (playerClosetPoint - headPosition).sqrMagnitude;

        if (Physics.Raycast(headPosition, directionPlayer, out RaycastHit hit, stateMachine.npc.ViewDistance, stateMachine.npc.layer))
        {
            if (hit.collider.gameObject != stateMachine.npc.player.transform.gameObject)
            {
                return false;
            }
        }
        return true;
    }

    protected void RotateVelocity()
    {
        NavMeshAgent agent = stateMachine.npc.Agent;
        Vector3 vel = agent.velocity;

        if (vel.sqrMagnitude < 0.01f) return;

        Quaternion rot = Quaternion.LookRotation(vel.normalized);
        Transform trans = stateMachine.npc.transform;
        trans.rotation = Quaternion.Slerp(trans.rotation, rot, stateMachine.RotationDamping * Time.deltaTime);

    }

    public void GuardWait()
    {
        var agent = stateMachine.npc.Agent;
        bool isMoving = !agent.pathPending && agent.remainingDistance > agent.stoppingDistance;
        if (GameManager.Instance.SelectedBGM != null)
        {
            agent.updateRotation = false;
            if (!isWaiting)
            {
                waitTimer += Time.deltaTime;

                if (isMoving)
                {
                    RotateVelocity();
                    StartAnimation("Walk");
                    StopAnimation("LookAround");
                }
                else
                {
                    StopAnimation("Walk");
                    cooldownTimer += Time.deltaTime; 
                }

                if (cooldownTimer >= 2f) 
                {
                    if (waitTimer >= 3f) 
                    {
                        if (stateMachine.npc is Guard guard)
                        {
                            agent.SetDestination(guard.GetWaitPosition().transform.position);
                            isWaiting = true;
                            waitTimer = 0f;
                            cooldownTimer = 0f;
                            StartAnimation("Walk");
                            StopAnimation("LookAround");
                        }
                    }
                }
            }
            else // 대기중
            {
                if (isMoving)
                {
                    RotateVelocity();
                    StartAnimation("Walk");
                    StopAnimation("LookAround");
                }
                else
                {
                    StopAnimation("Walk");
                    StartAnimation("LookAround");
                    cooldownTimer += Time.deltaTime;

                    if (cooldownTimer >= 5f)
                    {
                        isWaiting = false;
                        cooldownTimer = 0f;
                        waitTimer = 0f;
                        moveTimer = 0f;
                        agent.SetDestination(GetRandomPointInArea(stateMachine.npc.Area));
                    }
                }
            }
            agent.updateRotation = true;
        }
    }
}
