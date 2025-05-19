
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class Interaction : MonoBehaviour
{
    // 상호 작용 가능한 오브젝트 레이어 필터
    public LayerMask layerMask;

    [Header("UI")]
    public GameObject settingMenu;
    public TextMeshProUGUI interactText;

    private IInteractable curInteractable;
    private readonly List<IInteractable> curInterdatas = new List<IInteractable>();

    private Player player;
    PlayerController input;

    // Start is called before the first frame update
    void Start()
    {
        if (interactText == null)
        {

            interactText = GameObject.Find("Canvas/InteractText").gameObject.GetComponent<TextMeshProUGUI>();
        }

        if (player == null)
            player = GameManager.Instance.Player;
        if (input == null)
        {
            input = player.Input;
        }

        input.playerActions.Interection.started -= OnInteractInput;
        input.playerActions.Interection.started += OnInteractInput;
        input.playerActions.Setting.started -= OnSettingInput;
        input.playerActions.Setting.started += OnSettingInput;

        interactText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        player = GameManager.Instance.Player;
        if (player == null) return;

        input = GameManager.Instance.Player.Input;
        if (input == null) return;


        input.playerActions.Interection.started -= OnInteractInput;
        input.playerActions.Interection.started += OnInteractInput;
        input.playerActions.Setting.started -= OnSettingInput;
        input.playerActions.Setting.started += OnSettingInput;

        input.SubscribeAllInputs();

        SceneManager.sceneLoaded += OnLoadInteract;
    }

    private void OnDisable()
    {
        if (input != null)
        {
            if (input != null)
            {
                input.UnsubscribeAllInputs(this);

            }

            input.playerActions.Interection.started -= OnInteractInput;
            input.playerActions.Setting.started -= OnSettingInput;
        }

        SceneManager.sceneLoaded -= OnLoadInteract;
    }


    // Update is called once per frame
    void Update()
    {
        // 리스트가 비어 있으면 UI를 숨기고 return 합니다.
        if (curInterdatas.Count <= 0)
        {

            curInteractable = null;
            interactText.gameObject.SetActive(false);
            return;
        }

        // 플레이어와 가장 가까운 IInteratable 찾기
        float shortestDistance = float.MaxValue;
        IInteractable nearInteracte = null;
        Vector3 playerPosision = player.transform.position;

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


            curInteractable = nearInteracte;
        if (curInteractable != null)
        {
            string txt = curInteractable.GetInteractComponent();

            if (!string.IsNullOrEmpty(txt))
            {
                interactText.text = curInteractable.GetInteractComponent();
                interactText.gameObject.SetActive(true);
            }
            else
            {
                interactText.gameObject.SetActive(false);
            }
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }



    }

    // 레이어 체크 후 IInteractable 이라면 리스트에 추가
    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask.value & (1 << other.gameObject.layer)) == 0) return;

        var interactable = other.GetComponent<IInteractable>();

        if (interactable != null && !curInterdatas.Contains(interactable))
        {
            curInterdatas.Add(interactable);
        }

    }

    // 리스트에서 제거, 현재 대상이 사라지면 UI를 숨깁니다.
    private void OnTriggerExit(Collider other)
    {
        var chair = other.GetComponent<InteractionChair>();
        if (chair != null)
            chair.DisableTrigger();

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
    }

    public void OnInteractInput(InputAction.CallbackContext context)    // 버튼을 눌렀을 때 대상의 OnInteract() 호출 후 UI 처리
    {
        if (GameManager.Instance.Player.stateMachine.CurrentState() is PlayerInteractionLockpick)
            return;

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

    private void OnLoadInteract(Scene scene, LoadSceneMode mode)        // 씬을 다시 불러오거나 리로딩 할 때 재등록
    {
        input.UnsubscribeAllInputs(this);
        player = GameManager.Instance.Player;
        input = player.Input;
        input.SubscribeAllInputs();

        curInterdatas.Clear();

        curInteractable = null;
    }
    

}
