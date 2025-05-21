using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rifle : MonoBehaviour, IInteractable
{
    [Header("카메라")]
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minFov = 1f;
    [SerializeField] private float maxFov = 20f;

    [Header("Ray")]
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private LayerMask interactionLayer;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private Image crosshair;

    private Camera mainCam;
    private bool isAiming = false;
    private IInteractable curInteract;
    private PlayerInputs inputActions;

    private void OnEnable()
    {
        if(inputActions == null) inputActions = new PlayerInputs();
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        mainCam = Camera.main;
        crosshair.gameObject.SetActive(false);
        interactText.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (UIManager.Instance.isUIActive)
        {
            aimCamera.enabled = false;
        }
        else aimCamera.enabled = true;
        if (isAiming)
        {
            ZoomFOV();

            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f);
            Ray ray = mainCam.ScreenPointToRay(screenCenter);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactionLayer))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.white);
                Debug.Log($"[Ray hit] {hit.collider.name}");

                var interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    curInteract = interactable;
                    interactText.text = curInteract.GetInteractComponent();
                    interactText.gameObject.SetActive(true);
                }
                else
                {
                    interactText.text = string.Empty;
                    interactText.gameObject.SetActive(false);
                }
            }
            else
            {
                curInteract = null;
                interactText.text = string.Empty;
                interactText.gameObject.SetActive(false);
                Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.blue);
            }
        }
    }
    public string GetInteractComponent()
    {
        if (!isAiming) return "E키를 눌러 조준 모드 진입";
        else if (isAiming && !interactText.gameObject.activeSelf) return "E키를 눌러 조준 모드 해제";
        else return string.Empty;
    }

    public void OnInteract()
    {
        if (!GameManager.Instance.SimulationMode)
        {
            if (aimCamera != null && !isAiming)
            {
                crosshair.gameObject.SetActive(true);
                aimCamera.Priority = 20;
                isAiming = true;
            }
            else if (isAiming)
            {
                crosshair.gameObject.SetActive(false);
                interactText.gameObject.SetActive(false);
                aimCamera.Priority = 0;
                isAiming = false;
                if (curInteract != null)
                    curInteract.OnInteract();
            }
        }
    }
    
    private void ZoomFOV()
    {
        float scrollInput = inputActions.Player.Zoom.ReadValue<float>();

        if(Mathf.Abs(scrollInput) > 0.01f)
        {
            float fov = aimCamera.m_Lens.FieldOfView;
            fov -= scrollInput * zoomSpeed;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            aimCamera.m_Lens.FieldOfView = fov;
        }
    }

    public void SetInteractComponenet(string newText)
    {
        throw new System.NotImplementedException();
    }
    public void Deactive()
    {
        throw new System.NotImplementedException();
    }
}
