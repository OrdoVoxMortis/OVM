using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class NpcBaseState : IState
{
    protected NpcStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    protected bool isAlert = true;
    protected bool isAction = false;
    public float moveDelay = 2f;
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
        if (!GameManager.Instance.SimulationMode && GameManager.Instance.SelectedBGM != null)
        {
            stateMachine.npc.Agent.isStopped = false;
            moveTimer += Time.deltaTime;
            if (moveTimer >= moveDelay)
            {
                stateMachine.npc.Agent.SetDestination(GetRandomPointInArea(stateMachine.npc.Area));
                moveTimer = 0f;
            }

            var agent = stateMachine.npc.Agent;
            bool isMoving = !agent.pathPending && agent.remainingDistance > agent.stoppingDistance;
            if (isMoving) StartAnimation("Walk");
            else StopAnimation("Walk");
        }
        else
        {
            StopAnimation("Walk");
            stateMachine.npc.Agent.isStopped = true;
        }
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
        if (angle > stateMachine.npc.ViewAngle / 2f || distance > stateMachine.npc.ViewDistance) return false;

        return true;
    }

    public void GuardWait()
    {
        var agent = stateMachine.npc.Agent;

        if (!isWaiting)
        {
            waitTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;

            if (moveTimer >= moveDelay)
            {
                agent.SetDestination(GetRandomPointInArea(stateMachine.npc.Area));
                moveTimer = 0f;
            }

            bool isMoving = !agent.pathPending && agent.remainingDistance > agent.stoppingDistance;
            if (isMoving) StartAnimation("Walk");
            else StopAnimation("Walk");

            if (waitTimer >= 3f)
            {
                if (stateMachine.npc is Guard guard)
                {
                    agent.SetDestination(guard.GetWaitPosition().transform.position);
                    isWaiting = true;
                    waitTimer = 0f;
                }
            }
        }
        else // 대기중
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                StopAnimation("Walk");
                cooldownTimer += Time.deltaTime;

                if (cooldownTimer >= 5f)
                {
                    isWaiting = false;
                    cooldownTimer = 0f;
                    waitTimer = 0f;
                    agent.SetDestination(GetRandomPointInArea(stateMachine.npc.Area));
                }
            }
        }
    }
}
