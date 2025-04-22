using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        //playerActions.Accept.performed += OnAcceptQuest;
        playerActions.CancleUI.started += OnCancleUI;

        playerCamera = transform.Find("CameraLookPoint/FollowPlayerCamera").GetComponent<CinemachineFreeLook>();


        if (playerCamera == null)
        {
            Debug.LogError("씬에서 CinemachineFreeLook 카메라를 찾을 수 없습니다.");
        }
        else
        {
            CinemachineVirtualCamera middleRog = playerCamera.GetRig(1);
            if (middleRog != null)
            {
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
        //playerActions.Accept.performed += OnAcceptQuest;
        playerActions.CancleUI.performed += OnCancleUI;
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        //playerActions.Accept.performed -= OnAcceptQuest;
        playerActions.CancleUI.performed -= OnCancleUI;
        playerInputs.Disable();
    }

    private void OnAcceptQuest(InputAction.CallbackContext context)
    {
        
    }

    public void OnCancleUI(InputAction.CallbackContext context)
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
        playerActions.CancleUI.started += OnCancleUI;
    }

    public void UnsubscribeCancleUI()
    {
        playerActions.CancleUI.started -= OnCancleUI;
    }
}
