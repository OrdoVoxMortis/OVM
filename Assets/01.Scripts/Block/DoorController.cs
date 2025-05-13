using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private string lockedInteractText = "락픽 사용하기 [E]";


    private NavMeshObstacle obstacle;
    private Collider doorCollider;

    private Quaternion closeRot;
    private Quaternion openRot;
    private Coroutine routine;

    private string interactText = "E키를 눌러 상호작용";

    private void Awake()
    {

        closeRot = this.transform.localRotation;
        openRot = closeRot * Quaternion.Euler(0f, openAngle, 0f);

        if (obstacle == null)
            obstacle = GetComponent<NavMeshObstacle>();
        if (doorCollider == null)
            doorCollider = GetComponent<Collider>();

        if (obstacle != null)
            obstacle.carving = true;
        if (doorCollider != null)
            doorCollider.isTrigger = false;

        if (obstacle == null)
            Debug.LogWarning("Door 에 Nav Mesh Obstacle이 존재하지 않습니다.");
        if (doorCollider == null)
            Debug.LogWarning("Door 에 Collider가 존재하지 않습니다.");

    }

    public void OpenDoor()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(OpenClose());
    }

    private IEnumerator OpenClose()
    {
        if (obstacle != null)
            obstacle.carving = false;
        if (doorCollider != null)
            doorCollider.isTrigger = true;

        yield return RotateDoor(closeRot, openRot, openDuration);

        yield return new WaitForSeconds(closeDelay);

        if (doorCollider != null)
            doorCollider.isTrigger = false;

        yield return RotateDoor(openRot, closeRot, closeDuration);

        if (obstacle != null)
            obstacle.carving = true;

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
        OpenDoor();
    }

    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    public string GetInteractComponent()
    {
        return interactText;
    }

    public void SetInteractComponenet(string newText)
    {
        interactText = newText;
    }
}
