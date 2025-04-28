using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Interaction : MonoBehaviour
{
    public LayerMask layerMask;

    [Header("UI")]
    public GameObject settingMenu;
    [SerializeField] private TextMeshProUGUI interactText;

    private IInteractable curInteractable;
    private readonly List<IInteractable> curInterdatas = new List<IInteractable>();

    // Start is called before the first frame update
    void Start()
    {
        if (interactText == null)
        {

            interactText = GameObject.Find("Canvas/InteractText").gameObject.GetComponent<TextMeshProUGUI>();
        }

        PlayerController input = GameManager.Instance.Player.Input;
        input.playerActions.Interection.started -= OnInteractInput;
        input.playerActions.Interection.started += OnInteractInput;
        input.playerActions.Setting.started += OnSettingInput;
        //input.playerActions.Cancel.started += OnCancelInput;
        SceneManager.sceneLoaded += OnInteract;

        interactText.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (curInterdatas.Count <= 0)
        {
            if (curInteractable != null)
            {
                curInteractable = null;
                interactText.gameObject.SetActive(false);
            }
            return;
        }

        // 플레이어와 가장 가까운 IInteratable 찾기
        float shortestDistance = float.MaxValue;
        IInteractable nearInteracte = null;
        Vector3 playerPosision = GameManager.Instance.Player.transform.position;

        foreach (var candidate in curInterdatas)
        {
            var candidateTransform = (candidate as MonoBehaviour)?.transform;
            if (candidateTransform == null)
            {
                continue;
            }

            float distance = Vector3.Distance(playerPosision, candidateTransform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearInteracte = candidate;
            }
        }

        // UI 갱신(가장 가까운 대상이 바뀌었을 때)
        if (nearInteracte != curInteractable)
        {
            curInteractable = nearInteracte;
            if (curInteractable != null)
            {
                SetText();
                interactText.gameObject.SetActive(true);
            }
            else
            {
                interactText.gameObject.SetActive(false);
            }
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask.value & (1 << other.gameObject.layer)) == 0) return;

        var interactable = other.GetComponent<IInteractable>();

        if (interactable != null && !curInterdatas.Contains(interactable))
        {
            curInterdatas.Add(interactable);

            //curInteractGameObject = other.gameObject;
            //curInteractable = interactable;
            //SetText();
            //Debug.Log("MusicDoor가 찍힘");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            curInterdatas.Remove(interactable);
            if (interactable == curInteractable)
            {
                curInteractable = null;
                interactText.gameObject.SetActive(false);
            }
        }

        //if (curInteractable != null && other.gameObject == ((MonoBehaviour)curInteractable).gameObject)
        //{
        //    curInteractGameObject = null;
        //    curInteractable = null;
        //    //TODO 텍스트 출력을 없애 줘야함
        //    interactText.gameObject.SetActive(false);
        //}
      
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
            curInteractable.OnInteract();
            interactText.gameObject.SetActive(false);
            curInteractable = null;
        }
    }

    public void OnSettingInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            // UI가 비활성화 상태라면 → 세팅창 열기
            if (!UIManager.Instance.isUIActive)
            {
                UIManager.Instance.ShowUI<UI_Volume>("UI_Volume");
                UIManager.Instance.UIActive(); // 커서 unlock, playerCamera 끄기 등
            }
            // UI가 이미 켜져 있으면 → 세팅창 닫기
            else
            {
                UIManager.Instance.HideUI<UI_Volume>();
                UIManager.Instance.UIDeactive(); // 커서 lock, camera 다시 켜기
            }
        }
    }

    public void OnCancelInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            interactText.gameObject.SetActive(false);
            curInteractable = null;
        }
    }

    private void OnInteract(Scene scene, LoadSceneMode mode)
    {
        PlayerController input = GameManager.Instance.Player.Input;
        input.playerActions.Interection.started -= OnInteractInput;
        input.playerActions.Interection.started += OnInteractInput;

        curInterdatas.Clear();

        curInteractable = null;
    }

}
