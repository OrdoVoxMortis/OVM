using GoogleSheet.Core.Type;
using UnityEngine;
using UnityEngine.AI;

[UGS(typeof(NpcType))]
public enum NpcType
{
    Target,
    Guard,
    Friend,
    Normal
}
[UGS(typeof(ActionType))]
public enum ActionType
{
    None,
    LookAround, // 두리번
    RunAway, // 도망
    Chase, // 추격
    Notify, // 타겟 알림
    Watch // 시선
}

public class Suspicion
{
    public string grade;
    public int increasePerSec; // 의심 수치 상승량
    public int decreasePerSec; // 의심 수치 하락량
    public int maxValue = 100; // 최대 의심 수치 
}
public class NPC : MonoBehaviour
{
    //data
    [SerializeField] private string id; 
    public string Name {  get; private set; } // NPC 이름
    public NpcType Type { get; private set; } // NPC 유형
    public float ViewAngle {  get; private set; } // 시야 각도
    public float ViewDistance {  get; private set; } // 시야 거리
    public float MinAlertTime {  get; private set; } // 최소 경계 시간 
    public float MaxAlertTime {  get; private set; } // 최대 경계 시간
    public ActionType ContiAlertAction {  get; private set; } // 지속형 행동 패턴
    public ActionType TriggerAlertAction {  get; private set; } // 발동형 행동 패턴
    public Suspicion SuspicionParams { get; private set; } // 의심 수치
    public int CurSuspicion { get; set; } // 현재 의심 수치
    public float CurAlertTime {  get; set; } // 현재 경계 시간
    public NavMeshAgent Agent { get; set; }

    [field: SerializeField] public NPCSO Data { get; private set; }
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public BoxCollider Area { get; set; }

    private void Init()
    {
        LoadData();
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        CurSuspicion = 0;
    }

    void LoadData()
    {
        var data = DataManager.Instance.npcDict[id];
        Name = data.name;
        Type = data.type;

        var type = DataManager.Instance.npcTypeDict[Type.ToString()];
        ViewAngle = type.viewAngle;
        ViewDistance = type.viewDistance;
        MinAlertTime = type.minAlertTime;
        MaxAlertTime = type.maxAlertTime;
        ContiAlertAction = type.contiAlertAction;
        TriggerAlertAction = type.triggerAlertAction;
        var grade = type.suspicionParams;
        SuspicionParams.grade = DataManager.Instance.suspicionDict[grade].grade;
        SuspicionParams.increasePerSec = DataManager.Instance.suspicionDict[grade].increasePerSec;
        SuspicionParams.decreasePerSec = DataManager.Instance.suspicionDict[grade].decreasePerSec;
    }


}

