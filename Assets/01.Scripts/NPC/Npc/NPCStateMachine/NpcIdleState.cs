using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcIdleState : NpcBaseState
{
    private float suspicionTimer = 0f;
    public NpcIdleState(NpcStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("idle");
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        base.Update();
        if (stateMachine.npc.CurAlertTime > 0)
            stateMachine.npc.CurAlertTime -= Time.deltaTime;
        else DecreaseSuspicion();

        if (IsPlayerInSight())
        {
           stateMachine.ChangeState(stateMachine.AlertState);
        }
    }

    private bool IsPlayerInSight() //true -> 경계
    {
        Transform player = GameManager.Instance.Player.transform;
        Vector3 directionPlayer = (player.position - stateMachine.npc.transform.position).normalized;
        float angle = Vector3.Angle(stateMachine.npc.transform.forward, directionPlayer);

        if (angle > stateMachine.npc.ViewAngle / 2f || stateMachine.npc.Agent.remainingDistance > stateMachine.npc.ViewDistance) return false;

        return true;
    }
    private void DecreaseSuspicion()
    {
        suspicionTimer += Time.deltaTime;
        if (suspicionTimer >= 1f)
        {
            suspicionTimer = 0f;
            stateMachine.npc.CurSuspicion = Mathf.Max(0, stateMachine.npc.CurSuspicion -= stateMachine.npc.SuspicionParams.decreasePerSec);
        }
    }


}
