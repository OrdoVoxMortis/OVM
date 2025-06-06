using GoogleSheet.Core.Type;
using System.Collections;
using Unity.VisualScripting;
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

public enum BaseBehaviorType
{
    Idle,
    Wander
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
    [field: SerializeField] public NpcAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public BoxCollider Area { get; set; }
    public NpcStateMachine stateMachine;
    public bool IsAction { get; set; }
    private bool isPause = false;
    private bool prevAgentStopped;
    private float prevAnimSpeed;

    public GameObject player;
    public Collider playerCollider;
    public bool isColliding = false; // 충돌
    public bool isWalking = true;
    public LayerMask layer;
    private Coroutine stopCoroutine;

    public Target target;

    [Header("대기 시간")]
    public float moveDelay = 2f;
    [Header("기본 행동")]
    public BaseBehaviorType behaviorType;

    protected virtual void Start()
    {
        Init();
        stateMachine = new NpcStateMachine(this);
        stateMachine.ChangeState(stateMachine.IdleState);

        target = FindObjectOfType<Target>();
        player = GameManager.Instance.Player.gameObject;
        playerCollider = player.GetComponent<Collider>();
        prevAnimSpeed = Agent.speed;
        GameManager.Instance.OnStart += Destroy;
    }
    private void Init()
    {
        LoadData();
        Animator = GetComponentInChildren<Animator>();
        AnimationData.Initialize();
        Agent = GetComponent<NavMeshAgent>();
        FindArea();

        CurSuspicion = 0;
        isWalking = true;
    }
    private void Update()
    {
        if (GameManager.Instance.SimulationMode)
        {
            if (!isPause)
            {
                prevAgentStopped = Agent.isStopped;
                prevAnimSpeed = Animator.speed;

                Agent.velocity = Vector3.zero;
                Animator.speed = 0f;
                Agent.isStopped = true;
                isPause = true;
            }
            return;
        }

        if (isPause)
        {
            Agent.isStopped = prevAgentStopped;
            Animator.speed = prevAnimSpeed;
            isPause = false;
        }

        stateMachine?.Update();
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

        SuspicionParams = new Suspicion();
        SuspicionParams.grade = DataManager.Instance.suspicionDict[grade].grade;
        SuspicionParams.increasePerSec = DataManager.Instance.suspicionDict[grade].increasePerSec;
        SuspicionParams.decreasePerSec = DataManager.Instance.suspicionDict[grade].decreasePerSec;
    }

    private void OnDrawGizmosSelected()
    {
        if (Agent == null)
            return;

        Gizmos.color = Color.yellow;

        Vector3 position = transform.position;
        Vector3 forward = transform.forward;

        // ViewAngle 양쪽 방향 계산
        Vector3 leftBoundary = Quaternion.Euler(0, -ViewAngle / 2f, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, ViewAngle / 2f, 0) * forward;

        // 중심선
        Gizmos.DrawLine(position, position + forward * ViewDistance);
        // 왼쪽 시야선
        Gizmos.DrawLine(position, position + leftBoundary * ViewDistance);
        // 오른쪽 시야선
        Gizmos.DrawLine(position, position + rightBoundary * ViewDistance);

        // 시야 거리 원
        Gizmos.color = new Color(1, 1, 0, 0.2f);
        Gizmos.DrawWireSphere(position, ViewDistance);
    }

    private void FindArea()
    {
        if (behaviorType == BaseBehaviorType.Idle) return;
        Area[] allAreas = GameObject.FindObjectsOfType<Area>();
        float minDist = float.MaxValue;
        Area selectedArea = null;

        foreach(var area in allAreas)
        {
            if (area.AreaBounds.bounds.Contains(transform.position)) 
            {
                selectedArea = area;
                break;
            }
            float dist = Vector3.Distance(transform.position, area.transform.position);
            if(dist < minDist)
            {
                minDist = dist;
                selectedArea = area;
            }
        }

        if (selectedArea != null)
        {
            Area = selectedArea.AreaBounds;
            
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.OnStart -= Destroy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("충돌");
            Animator.SetBool(AnimationData.WalkParameterHash, false);
            Animator.SetBool(AnimationData.RunParameterHash, false);
            Animator.SetBool(AnimationData.TriggerParameterHash, true);
            if (stopCoroutine != null) StopCoroutine(stopCoroutine);
            stopCoroutine = StartCoroutine(StopDelay(0f, isWalking));

        }
    }

    private IEnumerator StopDelay(float delay, bool walk)
    {
        float speed = Agent.speed;
        float elapsed = 0f;
        float duration = 2f;

        //Stop Walking - 애니메이션 2초 후 멈춤
        if (walk)
        {
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                Agent.speed = Mathf.Lerp(speed, 0f, elapsed / duration);
                yield return null;
            }
        }
        else
        {
            Agent.speed = 0f; // 즉시 정지
        }


        isColliding = true;

        yield return new WaitForSeconds(walk ? delay : 1f);

        Agent.speed = prevAnimSpeed;
        isColliding = false;
        Agent.isStopped = false;
        Animator.SetBool(AnimationData.TriggerParameterHash, false);

        if (walk) Animator.SetBool(AnimationData.WalkParameterHash, true);
        else Animator.SetBool(AnimationData.RunParameterHash, true);
    }
}

