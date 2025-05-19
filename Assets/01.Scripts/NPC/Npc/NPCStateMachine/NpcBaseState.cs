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

    //Talking
    protected float talkCoolDown = 0f;
    protected float talkInterval = 0.5f;
    protected bool isTalking = false;
    protected bool talkIndexInitialized = false;
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
                StopAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
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
                StartAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
            }
            else StopAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
        }
        else
        {
            StopAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
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
            if (crossY > 0f) StartAnimation(stateMachine.npc.AnimationData.TurnLeftParameterHash);
            else StartAnimation(stateMachine.npc.AnimationData.TurnRightParameterHash);
        }
        StopAnimation(stateMachine.npc.AnimationData.TurnRightParameterHash);
        StopAnimation(stateMachine.npc.AnimationData.TurnLeftParameterHash);
        stateMachine.npc.Agent.SetDestination(nextPosition);
    }
    protected void StartAnimation(int hash)
    {
        stateMachine.npc.Animator.SetBool(hash, true);
    }

    protected void StopAnimation(int hash)
    {
        stateMachine.npc.Animator.SetBool(hash, false);
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
        //if (GameManager.Instance.Player.unlocking)
        //{
        //    stateMachine.ChangeState(stateMachine.ActionState);
        //}
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
                    StartAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
                    StopAnimation(stateMachine.npc.AnimationData.LookAroundParameterHash);
                }
                else
                {
                    StopAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
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
                            StartAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
                            StopAnimation(stateMachine.npc.AnimationData.LookAroundParameterHash);
                        }
                    }
                }
            }
            else // 대기중
            {
                if (isMoving)
                {
                    RotateVelocity();
                    StartAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
                    StopAnimation(stateMachine.npc.AnimationData.LookAroundParameterHash);
                }
                else
                {
                    StopAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
                    StartAnimation(stateMachine.npc.AnimationData.LookAroundParameterHash);
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

    public void TalkingIdle()
    {
        stateMachine.npc.Agent.isStopped = true;
        StopAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
        StartAnimation(stateMachine.npc.AnimationData.TalkingParameterHash);

        if (!talkIndexInitialized)
        {
            int initIndex = Random.Range(1, 5);
            stateMachine.npc.Animator.SetInteger("TalkIndex", initIndex);
            talkIndexInitialized = true;
        }
        talkCoolDown += Time.deltaTime;

        if(talkCoolDown >= talkInterval) 
        {
            talkCoolDown = 0f;
            AnimatorStateInfo stateInfo = stateMachine.npc.Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsTag("Talk") && stateInfo.normalizedTime >= 1f)
            {
                StopAnimation(stateMachine.npc.AnimationData.TalkingParameterHash);
                int randomIndex = Random.Range(1, 5);
                stateMachine.npc.Animator.SetInteger("TalkIndex", randomIndex);
            }
        }
    }

    public void GuardIdle()
    {
        if (stateMachine.npc is Guard guard)
        {
            var agent = guard.Agent;

            if (agent.pathPending) return;


            if (agent.remainingDistance > agent.stoppingDistance)
            {
                Debug.Log("이동 중");
                RotateVelocity();
                if (Vector3.Distance(agent.destination, guard.startPosition.position) > 0.1f)
                {
                    agent.SetDestination(guard.startPosition.position);
                }
                StopAnimation(guard.AnimationData.RunParameterHash);
                StartAnimation(guard.AnimationData.WalkParameterHash);
                agent.isStopped = false;
            }
            else 
            {
                StopAnimation(guard.AnimationData.RunParameterHash);
                StopAnimation(guard.AnimationData.WalkParameterHash);
                agent.isStopped = true;
            }
        }

    }
}
