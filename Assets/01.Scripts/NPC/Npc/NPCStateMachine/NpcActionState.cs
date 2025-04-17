
using System.Collections;
using System.IO;
using UnityEngine;

public class NpcActionState : NpcBaseState
{
    private bool notifyToTarget = false;

    private float lookTime;
    private float lookTimer;
    private Quaternion targetRotation;
    public NpcActionState(NpcStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        ActionByType();
    }

    private void ActionByType()
    {
        ActionType type = stateMachine.npc.AlertAction;

        switch (type)
        {
            case ActionType.Notify:
                //타겟에게 알림
                NotifyTarget();
                break;
            case ActionType.Chase:
                stateMachine.npc.Agent.SetDestination(stateMachine.Target.transform.position);
                break;
            case ActionType.Watch:
                LookAtTarget();
                break;
            case ActionType.RunAway:
                //안전구역으로 이동
                if (stateMachine.npc is Target target)
                {
                    //도망
                    if (target.IsNotified)
                    {
                        RunAway();
                    }
                    else //두리번
                    {
                        LookAround();
                    }
                }
                break;
        }
    }

    private void NotifyTarget()
    {
        var target = GameObject.FindObjectOfType<Target>();
        stateMachine.npc.Agent.SetDestination(target.transform.position);
        if (stateMachine.npc.Agent.remainingDistance == 0 && stateMachine.npc is Friend friend)
        {
            friend.NotifyTarget(target);
        }
    }
    private void LookAtTarget()
    {
        stateMachine.npc.Agent.updateRotation = false;
        Vector3 dirToTarget = (stateMachine.Target.transform.position - stateMachine.npc.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(dirToTarget);
        stateMachine.npc.transform.rotation = Quaternion.Slerp(stateMachine.npc.transform.rotation, lookRotation, Time.deltaTime * stateMachine.RotationDamping);
        stateMachine.npc.Agent.updateRotation = true;
    }

    private void RunAway()
    {
        //TODO. 안전 구역 받아오기 -> SetDestination -> 구역 내 있으면 ChangeState
    }


    private void LookAround() // 두리번
    {
        lookTimer += Time.deltaTime;

        stateMachine.npc.transform.rotation = Quaternion.Slerp(
            stateMachine.npc.transform.rotation,
            targetRotation,
            Time.deltaTime * 2f
        );

        if (lookTimer >= lookTime)
        {
            lookTimer = 0f;
            lookTime = Random.Range(1f, 2f);
            PickNewDirection();
        }
    }

    private void PickNewDirection()
    {
        float randomY = Random.Range(0f, 360f);
        Vector3 newDir = new Vector3(0f, randomY, 0f);
        targetRotation = Quaternion.Euler(newDir);
    }

}
