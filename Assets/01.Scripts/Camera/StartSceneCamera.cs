using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneCamera : MonoBehaviour
{
    [Header("Camera Setting")]
    public CinemachineVirtualCamera startCamera;
    public CinemachineFreeLook playerCamera;
    public float duration = 2f;

    private float startCamFov;
    private float playerCamFov;
    private PlayerController playerController;

    private void Awake()
    {
        startCamFov = startCamera.m_Lens.FieldOfView;
        playerCamFov = playerCamera.m_Lens.FieldOfView;
        startCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
    }

    private void Start()
    {
        playerController = GameManager.Instance.Player.Input;
    }

}
