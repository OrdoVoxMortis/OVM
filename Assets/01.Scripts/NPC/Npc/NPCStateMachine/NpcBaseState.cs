using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class NpcBaseState : IState
{
    protected NpcStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    protected bool isAlert = true;
    public float moveDelay = 2f;
    private float moveTimer = 0f;
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

        if (angle > stateMachine.npc.ViewAngle / 2f || stateMachine.npc.Agent.remainingDistance > stateMachine.npc.ViewDistance) return false;

        return true;
    }
}
