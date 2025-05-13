using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour, IInteractable
{
    [Header("Door Setting")]
    [SerializeField] private Transform door;

    [Tooltip("문이 열릴 각도")]
    [SerializeField] private float openAngle = 90f;
    
    [Tooltip("문이 열릴 시간")]
    [SerializeField] private float openDuration = 0.5f;

    [Tooltip("문이 자동으로 닫힐 시간")]
    [SerializeField] private float closeDelay = 1.0f;

    [Tooltip("닫는 데 걸릴 시간")]
    [SerializeField] private float closeDuration = 0.5f;

    private Quaternion closeRot;
    private Quaternion openRot;
    private Coroutine routine;

    private string interactText = "E키를 눌러 상호작용";

    private void Awake()
    {
        if (door == null)
        {
            door = transform.Find("Door");
            if (door == null)
                Debug.LogError($"{name}의 하위에 Door 오브젝트를 찾을 수 없습니다."); 
        }

        closeRot = door.localRotation;
        openRot = closeRot * Quaternion.Euler(0f, openAngle, 0f);

    }

    public void OpenDoor()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(OpenClose());
    }

    private IEnumerator OpenClose()
    {
        yield return RotateDoor(closeRot, openRot, openDuration);

        yield return new WaitForSeconds(closeDelay);

        yield return RotateDoor(openRot, closeRot, closeDuration);

    }


    private IEnumerator RotateDoor(Quaternion from, Quaternion to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            t = Mathf.SmoothStep(0f, 1f, t);
            door.localRotation = Quaternion.Slerp(from, to, t);
            yield return null;
        }
        door.localRotation = to;
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
