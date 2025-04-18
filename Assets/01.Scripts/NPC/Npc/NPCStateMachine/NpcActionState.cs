
using UnityEngine;
using UnityEngine.AI;

public class NpcActionState : NpcBaseState
{
    private bool notifyToTarget = false;

    private float lookTime;
    private float lookTimer;
    private Quaternion targetRotation;
    private bool isTriggered = false;
    private bool isPlayerInSight = false;
    private float lostSightTimer = 0f;

    private bool isMovingToTarget = false;
    public NpcActionState(NpcStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("action");
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (isMovingToTarget)
        {
            MoveToTarget();
            return;
        }
        if (IsPlayerInSight()) // 시야 내
        {
            if (!isPlayerInSight)
            {
                isPlayerInSight = true;
                lostSightTimer = 0f;
                stateMachine.npc.CurAlertTime = 0f; // 경계 시간 초기화
            }

            stateMachine.npc.CurAlertTime += Time.deltaTime; // 경계 시간 카운트
            Debug.Log("지속형 시작");
            ContiActionByType(); // 지속형 행동

            if (stateMachine.npc.CurAlertTime >= stateMachine.npc.MaxAlertTime && !isTriggered)
            {
                TriggerActionByType(); // 최대 경계 시간 초과 시 발동형 행동
            }
        }
        else // 시야 밖
        {
            if (isPlayerInSight)
            {
                isPlayerInSight = false;
                lostSightTimer = 0f;
            }

            lostSightTimer += Time.deltaTime;
            // 최소 경계 시간 동안 지속형 행동
            if (lostSightTimer < stateMachine.npc.MinAlertTime) ContiActionByType();
            else
            {
                isAlert = false;
                Debug.Log("지속형 끝");
                stateMachine.ChangeState(stateMachine.AlertState); // 최소 경계 시간 지나면 중단
            }
        }
    }

    private void ContiActionByType() // 지속형
    {
        if (isTriggered) return;
        ActionType type = stateMachine.npc.ContiAlertAction;

        switch (type)
        {
            case ActionType.Chase:
                ChasePlayer();
                break;
            case ActionType.Watch:
                LookAtTarget();
                break;
            default:
                break;
        }
    }

    private void TriggerActionByType() // 발동형
    {
        Debug.Log("발동형 시작");
        isTriggered = true;
        stateMachine.npc.IsAction = true;
        ActionType type = stateMachine.npc.TriggerAlertAction;

        switch (type)
        {
            case ActionType.Notify:
                NotifyTarget();

                break;
            default:
                break;
        }
        //stateMachine.ChangeState(stateMachine.AlertState);
    }

    private void NotifyTarget()
    {
        var target = GameObject.FindObjectOfType<Target>();
        if(target != null)
        {
            stateMachine.npc.Agent.isStopped = false;
            stateMachine.npc.Agent.SetDestination(target.transform.position);
            StartAnimation("Walk");
            isMovingToTarget = true;
        }
    }

    private void MoveToTarget()
    {
        var target = GameObject.FindObjectOfType<Target>();
        var agent = stateMachine.npc.Agent;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            StopAnimation("Walk");
            agent.isStopped = true;
            isMovingToTarget = false;

            if (stateMachine.npc is Friend friend)
            {
                friend.NotifyTarget(target);

            }
        }
    }
    private void LookAtTarget()
    {
        StopAnimation("Walk");
        Vector3 dirToTarget = (stateMachine.Target.transform.position - stateMachine.npc.transform.position).normalized;
        if (dirToTarget.sqrMagnitude < 0.01f) return;

        stateMachine.npc.Agent.updateRotation = false;

        Quaternion lookRotation = Quaternion.LookRotation(dirToTarget.normalized);
        stateMachine.npc.transform.rotation = Quaternion.Slerp(stateMachine.npc.transform.rotation, lookRotation, Time.deltaTime * stateMachine.RotationDamping);
        stateMachine.npc.Agent.updateRotation = true;
    }

    private void ChasePlayer()
    {
        StartAnimation("Run");
        stateMachine.npc.Agent.SetDestination(stateMachine.Target.transform.position);
        if(stateMachine.npc.Agent.remainingDistance <= stateMachine.npc.Agent.stoppingDistance)
        {
            StopAnimation("Run");
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
