using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionChair : MonoBehaviour, IInteractable
{
    [Header("Sit Setting")]
    [Tooltip("앉을 위치")]
    [SerializeField] private Transform seatPoint;

    [Header("Animation duration")]
    [Tooltip("앉는 애니메이션 시간")]
    [SerializeField] private float sitDownDuration = 1f;
    [Tooltip("일어서는 애니메이션 시간")]
    [SerializeField] private float standUpDuration = 1f;

    private string interactionText_SitDown = "앉기 [E]";
    private string interactionText_StandUp = "일어서기 [E]";
    private bool isSit = false;
    private Collider chairCollider;

    public Transform SeatPoint => seatPoint;
    public float SitDownDuration => sitDownDuration;
    public float StandUpDuration => standUpDuration;


    private void Awake()
    {
        chairCollider = GetComponent<Collider>();
    }

    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    public string GetInteractComponent()
    {
        isSit = GameManager.Instance.Player.isSit;
        return isSit ? interactionText_StandUp : interactionText_SitDown;
    }

    public void OnInteract()
    {
        PlayerStateMachine playerState = GameManager.Instance.Player.stateMachine;
        if (playerState.CurrentState() is PlayerInterationSitState sit && !sit.isStandUp)
            return;

        playerState.ChangeState(new PlayerInterationSitState(playerState, this, seatPoint, sitDownDuration, standUpDuration, false));
    }

    public void SetInteractComponenet(string newText)
    {
        throw new System.NotImplementedException();
    }

    public void EnableTrigger() => chairCollider.isTrigger = true;

    public void DisableTrigger() => chairCollider.isTrigger = false;

}
