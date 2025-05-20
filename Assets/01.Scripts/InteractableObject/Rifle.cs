using Cinemachine;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class Rifle : MonoBehaviour, IInteractable
{
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private Image crosshair;

    private Camera mainCam;
    private bool isAiming = false;
    private IInteractable curInteract;
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
            if(curInteract != null) 
                curInteract.OnInteract();
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
