
using UnityEngine;
using UnityEngine.AI;

public class NpcActionState : NpcBaseState
{
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
                stateMachine.npc.Agent.isStopped = false;
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
        StopAnimation("TurnLeft");
        StopAnimation("TurnRight");

        ActionType type = stateMachine.npc.TriggerAlertAction;

        switch (type)
        {
            case ActionType.Notify:
                NotifyTarget();

                break;
            default:
                break;
        }
    }

    private void NotifyTarget()
    {
        if(stateMachine.npc.target != null)
        {
            if (stateMachine.npc.isColliding) stateMachine.npc.Agent.isStopped = true;
            else stateMachine.npc.Agent.isStopped = false;
            stateMachine.npc.Agent.SetDestination(stateMachine.npc.target.transform.position);
            StartAnimation("Run");
            stateMachine.npc.isWalking = false;
            isMovingToTarget = true;
        }
    }

    private void MoveToTarget()
    {
        if (stateMachine.npc.target.IsNotified) return;
        var agent = stateMachine.npc.Agent;
        
        if (agent.remainingDistance <= agent.stoppingDistance) //도착시
        {
            StartAnimation("Notify");
            StopAnimation("Run");
            Vector3 lookDir = (stateMachine.npc.target.transform.position - stateMachine.npc.transform.position);
            lookDir.y = 0;
            if (lookDir.sqrMagnitude > 0.01f)
            {
                Quaternion lookRot = Quaternion.LookRotation(lookDir);
                Quaternion rotated = lookRot * Quaternion.Euler(0, -90f, 0);
                stateMachine.npc.transform.rotation = rotated;
            }
            agent.isStopped = true;
            isMovingToTarget = false;

            if (stateMachine.npc is Friend friend)
            {
                if (friend.IsNotifying) return;
                friend.NotifyTarget(stateMachine.npc.target, () =>
                {
                    StopAnimation("Notify");
                });

            }
        }
        else
        {
            agent.SetDestination(stateMachine.npc.target.transform.position);
        }
    }
    private void LookAtTarget()
    {
        StopAnimation("Walk");
        stateMachine.npc.Agent.isStopped = true;
        stateMachine.npc.Agent.velocity = Vector3.zero;
        Vector3 dirToTarget = (stateMachine.Target.transform.position - stateMachine.npc.transform.position).normalized;
        dirToTarget.y = 0f;
        if (dirToTarget.sqrMagnitude < 0.01f) return;

        stateMachine.npc.Agent.updateRotation = false;

        Quaternion lookRotation = Quaternion.LookRotation(dirToTarget.normalized);

        Vector3 forward = stateMachine.npc.transform.forward;

        float angle = Vector3.SignedAngle(forward, dirToTarget.normalized, Vector3.up);

        stateMachine.npc.transform.rotation = Quaternion.Slerp(stateMachine.npc.transform.rotation, lookRotation, Time.deltaTime * stateMachine.RotationDamping);
        
        if(Mathf.Abs(angle) > 10f)
        {
            if(angle > 0f)
            {
                StartAnimation("TurnRight");
                StopAnimation("TurnLeft");
            }
            else
            {
                StopAnimation("TurnRight");
                StartAnimation("TurnLeft");
            }
        }
        else 
        {
            StopAnimation("TurnRight");
            StopAnimation("TurnLeft");
        }

        stateMachine.npc.Agent.updateRotation = true;

    }

    private void ChasePlayer()
    {
        stateMachine.npc.isWalking = false;
        stateMachine.npc.Agent.isStopped = false;
        StartAnimation("Run");
        stateMachine.npc.Agent.SetDestination(stateMachine.Target.transform.position);
        if (stateMachine.npc.Agent.remainingDistance <= stateMachine.npc.Agent.stoppingDistance)
        {
            StopAnimation("Run");
            GameManager.Instance.GameOver();
            Debug.Log("가드");
            stateMachine.ChangeState(stateMachine.IdleState);
        }


    }
}
