using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetStateMachine : StateMachine
{
    public Target Target { get; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;


    public GameObject[] Blocks { get; private set; }
    public GameObject SafeZoneBlock { get; private set; }
    public TargetIdleState IdleState { get; }
    public TargetInteractionState InteractionState { get; }
    public TargetChasingState ChasingState { get; }
    public TargetGuardState GuardState { get; }
    public TargetRunAwayState RunAwayState { get; }
    public TargetNotifiedState NotifiedState { get; }

    // GuardState 진입 전 이전 상태와 해당 상태의 남은 시간을 저장
    public TargetBaseState PreviousState { get; set; }
    public float PreviousStateRemainingTime { get; set; }

    public float ViewAngle { get; private set; } // 시야 각도
    public float ViewDistance { get; private set; } // 시야 거리
    public float MinAlertTime { get; private set; } // 최소 경계 시간 
    public float MaxAlertTime { get; private set; } // 최대 경계 시간
    public NpcType Type { get; private set; } // NPC 유형

    public Suspicion SuspicionParams { get; set; } // 의심 수치
    public float CurAlertTime { get; set; } // 현재 경계 시간


    public float AlertValue = 0f;           // 현재 경계 수치
    public float AccumulateGuardTime = 0f;  // 누적된 경계 시간
    public float RemainingTravelTime = 0f;  // 다음 지점으로 도착해야 하는 시간


    public TargetStateMachine(Target target)
    {
        this.Target = target;

        this.Blocks = target.route;
        this.SafeZoneBlock = target.safeZone;

        IdleState = new TargetIdleState(this);
        ChasingState = new TargetChasingState(this);
        InteractionState = new TargetInteractionState(this);
        GuardState = new TargetGuardState(this);
        RunAwayState = new TargetRunAwayState(this);
        NotifiedState = new TargetNotifiedState(this);

        MovementSpeed = Target.Data.GroundData.BaseSpeed;
        RotationDamping = Target.Data.GroundData.BaseRotationDamping;

        LoadData();
    }

    void LoadData()
    {
        var data = DataManager.Instance.npcDict["NPC_T001"];
        Type = data.type;

        var type = DataManager.Instance.npcTypeDict[Type.ToString()];
        ViewAngle = type.viewAngle;
        ViewDistance = type.viewDistance;
        MinAlertTime = type.minAlertTime;
        MaxAlertTime = type.maxAlertTime;
        var grade = type.suspicionParams;

        SuspicionParams = new Suspicion();
        SuspicionParams.grade = DataManager.Instance.suspicionDict[grade].grade;
        SuspicionParams.increasePerSec = DataManager.Instance.suspicionDict[grade].increasePerSec;
        SuspicionParams.decreasePerSec = DataManager.Instance.suspicionDict[grade].decreasePerSec;
    }

    public void SaveCurrentState(TargetBaseState currentState, float remainingTime)
    {
        PreviousState = currentState;
        PreviousStateRemainingTime = remainingTime;
    }


}
