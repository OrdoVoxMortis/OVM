using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class DoorController : MonoBehaviour, IInteractable
{
    [Header("Door Setting")]

    [Tooltip("문이 열릴 각도")]
    [SerializeField] private float openAngle = 90f;
    
    [Tooltip("문이 열릴 시간")]
    [SerializeField] private float openDuration = 0.5f;

    [Tooltip("문이 자동으로 닫힐 시간")]
    [SerializeField] private float closeDelay = 1.0f;

    [Tooltip("닫는 데 걸릴 시간")]
    [SerializeField] private float closeDuration = 0.5f;

    [Header("Lockpick Setting")]

    [Tooltip("문 잠금 여부")]
    [SerializeField] private bool isLocked = false;
    [Tooltip("락픽 시간")]
    [SerializeField] private float lockPickDuration;
    [Tooltip("문 손잡이 위치")]
    [SerializeField] private Transform doorHandle;

    // 부모의 오브젝트
    [SerializeField] private GameObject obj;
    private int doorOpenCount = 0;          //문을 연 횟수


    private string lockedInteractText = "락픽 사용하기 [E]";


    //private NavMeshObstacle obstacle;
    private Collider doorCollider;

    private Quaternion closeRot;
    private Quaternion openRot;
    private Coroutine routine;

    private string interactText = "문 열기 [E]";

    private bool isClose;   // 문이 닫혔는지 확인

    private void Awake()
    {

        closeRot = this.transform.localRotation;
        openRot = closeRot * Quaternion.Euler(0f, openAngle, 0f);

        //if (obstacle == null)
        //    obstacle = GetComponent<NavMeshObstacle>();
        if (doorCollider == null)
            doorCollider = GetComponent<Collider>();

        //if (obstacle != null)
        //    obstacle.carving = true;
        if (doorCollider != null)
            doorCollider.isTrigger = false;

        //if (obstacle == null)
        //    Debug.LogWarning("Door 에 Nav Mesh Obstacle이 존재하지 않습니다.");
        if (doorCollider == null)
            Debug.LogWarning("Door 에 Collider가 존재하지 않습니다.");

        isClose = true;
    }

    public void OpenDoor()
    {
        if (!isClose) return;

        isClose = false;

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(OpenClose());
    }

    private IEnumerator OpenClose()
    {
        isClose = false;

        //if (obstacle != null)
        //    obstacle.carving = false;
        if (doorCollider != null)
            doorCollider.isTrigger = true;

        yield return RotateDoor(closeRot, openRot, openDuration);

        yield return new WaitForSeconds(closeDelay);

        if (doorCollider != null)
            doorCollider.isTrigger = false;

        yield return RotateDoor(openRot, closeRot, closeDuration);

        //if (obstacle != null)
        //    obstacle.carving = true;

        isClose = true;

    }

    public void Unlock()
    {
        isLocked = false;
    }


    private IEnumerator RotateDoor(Quaternion from, Quaternion to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            t = Mathf.SmoothStep(0f, 1f, t);
            this.transform.localRotation = Quaternion.Slerp(from, to, t);
            yield return null;
        }
        this.transform.localRotation = to;
    }

    public void OnInteract()
    {
        if (GameManager.Instance.Player.stateMachine.CurrentState() is PlayerAirState)
            return;

        if (GameManager.Instance.Player.stateMachine.CurrentState() is PlayerInteractionLockpick || GameManager.Instance.Player.isSimulMode)
            return;

        if (!isClose)
            return;

        doorOpenCount++;

        // 애널리틱스 상호작용(문)
        //var doorEvent = new CustomEvent("interact_door")
        //{
        //    ["object_id"] = obj.name,
        //    ["stage_id"] = StageManager.Instance.StageResult.id,
        //    ["door_open_count"] = doorOpenCount      
        //};
        //AnalyticsService.Instance.RecordEvent(doorEvent);


        if (isLocked)
        {
            float duration = lockPickDuration;
            PlayerStateMachine sm = GameManager.Instance.Player.stateMachine;
            PlayerInteractionLockpick lockState = new PlayerInteractionLockpick(sm, this, duration);
            sm.ChangeState(lockState);
        }
        else
        {
            OpenDoor();
        }

    }

    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    public string GetInteractComponent()
    {
        if (!isClose)
            return string.Empty;

        if (GameManager.Instance.Player.isSimulMode)
            return "";

        return isLocked ? lockedInteractText : interactText;
    }

    public void SetInteractComponenet(string newText)
    {
        interactText = newText;
    }

    public Transform GetDoorHandleTrans()
    {
        return doorHandle;
    }

}
