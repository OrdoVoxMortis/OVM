using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController :MonoBehaviour
{
    public PlayerInputs playerInputs { get; private set; }
    public PlayerInputs.PlayerActions playerActions { get; private set; }


    // 카메라 설정
    public Transform CameraLookPoint { get; private set; }
    public CinemachineFreeLook playerCamera;
    public Vector3 CameraFollowOffset { get; private set; }



    // Start is called before the first frame update
    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;

        
        // 카메라 찾기
        playerCamera = transform.Find("CameraLookPoint/FollowPlayerCamera").GetComponent<CinemachineFreeLook>();

        //카메라 참조 및 초기화
        if (playerCamera == null)
        {
            Debug.LogError("씬에서 CinemachineFreeLook 카메라를 찾을 수 없습니다.");
        }
        else
        {
            CinemachineVirtualCamera middleRog = playerCamera.GetRig(1);
            if (middleRog != null)
            {
                // 중간리그에서 CinemachineComposer 추출 후 초기 오프셋(m_TrackedObjectOffset) 저장
                CinemachineComposer composer = middleRog.GetCinemachineComponent<CinemachineComposer>();
                if (composer != null)
                {
                    CameraFollowOffset = composer.m_TrackedObjectOffset;
                }
                else
                {
                    Debug.LogError("중간리그에서 CinemachineComposer 컴포넌트를 찾을 수 없습니다.");
                }
            }
            else
            {
                Debug.LogError("playerCamera 에서 중간리그를 찾을 수 없습니다.");
            }
        }


        CameraLookPoint = this.transform.Find("CameraLookPoint");
        if (CameraLookPoint == null)
        {
            Debug.LogError("Player 자식에 CameraLookPoint 을 찾을수가 없습니다.");
        }
    }

    private void OnEnable()
    {
        
        playerActions.CancelUI.started += OnCancelUI;
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        
        playerActions.CancelUI.started -= OnCancelUI;
        playerInputs.Disable();
    }

    public void OnCancelUI(InputAction.CallbackContext context)
    {
        Debug.Log("Cancle 클릭");
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("현재UI 숨기기!");
            UIManager.Instance.CurrentUIHide();
        }
    }

    public void SubscribeCancleUI()
    {
        playerActions.CancelUI.started += OnCancelUI;
    }

    public void UnsubscribeCancleUI()
    {
        playerActions.CancelUI.started -= OnCancelUI;
    }

    //플레이어의 액션 전부 해제
    public void PlayerActionUnsubscribe()
    {
        Player player = GameManager.Instance.Player;
        var currentState = player.stateMachine.CurrentState();

        playerActions.Interection.started -= GameManager.Instance.Player.Interaction.OnInteractInput;

        if (currentState is PlayerBaseState baseState)
        {
            baseState.RemoveInputActionCallbacks();
        }

        UnsubscribeCancleUI();


        playerInputs.Disable();

        Debug.Log("플레이어 상호작용 인풋 액션 키 비활성화!");
        
    }


}
