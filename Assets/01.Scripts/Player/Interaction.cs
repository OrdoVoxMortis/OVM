using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    private GameObject curInteractGameObject;
    private IInteractable curInteractable;
    [SerializeField] private TextMeshProUGUI interactText;

    private Camera camera;
    private CinemachineFreeLook playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        PlayerController input = GameManager.Instance.Player.Input;
        input.playerActions.Interection.started += OnInteractInput;
        SceneManager.sceneLoaded += OnInteract;
    }

    private void Awake()
    {
        playerCamera = FindObjectOfType<CinemachineFreeLook>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!UIManager.Instance.isUIActive && Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red);

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    //TODO 텍스트를 출력시켜 줘야함
                    SetText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                //TODO 텍스트 출력을 없애 줘야함
                interactText.gameObject.SetActive(false);
            }
        }
        
    }
    private void SetText()
    {
        interactText.gameObject.SetActive(true);
        interactText.text = curInteractable.GetInteractComponent();
    }
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && curInteractable != null)
        {
            interactText.gameObject.SetActive(false);
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
        }
    }

    private void OnInteract(Scene scene, LoadSceneMode mode)
    {
        PlayerController input = GameManager.Instance.Player.Input;
        input.playerActions.Interection.started += OnInteractInput;
    }
}
