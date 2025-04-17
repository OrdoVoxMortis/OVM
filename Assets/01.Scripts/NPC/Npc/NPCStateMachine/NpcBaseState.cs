using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //groundData = stateMachine.npc.Data.GroundData;
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
            //stateMachine.npc.Animator.SetBool("Walk", true);
            stateMachine.npc.Agent.SetDestination(GetRandomPointInArea(stateMachine.npc.Area));
            moveTimer = 0f;
            //stateMachine.npc.Animator.SetBool("Walk", false);
        }
    }
    protected void StartAnimation(int animatorHash)
    {
        stateMachine.npc.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        stateMachine.npc.Animator.SetBool(animatorHash, false);
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
