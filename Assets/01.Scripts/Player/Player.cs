using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field:SerializeField] public PlayerSO Data { get; private set; }

    [field:Header("Animations")]
    [field:SerializeField]public PlayerAnimationData AnimationData {  get; private set; }

    public Animator Animator { get; private set; }
    public PlayerController Input {  get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }

    private PlayerStateMachine stateMachine;
    public CinemachineComposer composer;

    // 카메라 설정
    public Transform CameraLookPoint { get; private set; }
    public Vector3 CameraFollowOffset { get; private set; }

    public CinemachineFreeLook playerCamera;

    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerController>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();

        stateMachine = new PlayerStateMachine(this);
        
        playerCamera = FindObjectOfType<CinemachineFreeLook>();
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

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }
}
