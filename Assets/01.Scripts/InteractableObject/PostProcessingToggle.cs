using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PostProcessingToggle : MonoBehaviour
{
    public bool isEnabled = false; // 기본적으로 활성화 상태
    public GameObject timeLine_UI;
    public GameObject playRhythm_UI;
    PlayerController input;
    private Vector3 savedPlayerPosition;

    void Start()
    {
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = isEnabled;
        input = GameManager.Instance.Player.Input;
        input.playerActions.Simulate.started += OnSimulateInput;
    }

    public void TogglePostProcessing()
    {
        isEnabled = !isEnabled;
        GameManager.Instance.OnSimulationMode?.Invoke();
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = isEnabled;
        if(isEnabled && timeLine_UI != null)
        {
            GameManager.Instance.SimulationMode = true;
            savedPlayerPosition = GameManager.Instance.Player.transform.position;
            Debug.Log("플레이어 위치 저장!" + savedPlayerPosition);
            //TODO 시뮬레이션 전용 Cancle 구독
            GameManager.Instance.Player.Input.UnsubscribeCancleUI();
            timeLine_UI.SetActive(true);
            playRhythm_UI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            GameManager.Instance.SimulationMode = false;
            StartCoroutine(RestorePlayerPosition());
            Debug.Log("플레이어 원래 위치로 복귀" + savedPlayerPosition);
            //TODO 시뮬레이션 전용 Cancle 구독 해제
            GameManager.Instance.Player.Input.SubscribeCancleUI();
            timeLine_UI.SetActive(false);
            playRhythm_UI.SetActive(false);
            UIManager.Instance.UIDeactive();
        }
    }
    public void EnablePostProcessing()
    {
        isEnabled = true;
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = isEnabled;

        if (timeLine_UI != null)
        {
            GameManager.Instance.SimulationMode = true;
            savedPlayerPosition = GameManager.Instance.Player.transform.position;
            Debug.Log("플레이어 위치 저장!" + savedPlayerPosition);
            GameManager.Instance.Player.Input.UnsubscribeCancleUI();
            timeLine_UI.SetActive(true);
            playRhythm_UI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        // 플레이어 위치값 초기화
        }
    }
    private void OnSimulateInput(InputAction.CallbackContext context)
    {
        //TODO : 테스트를 위해 GameManager.Instance.SelectedBGM != null 를 주석처리함
        if (context.phase == InputActionPhase.Started /*&& GameManager.Instance.SelectedBGM != null*/)
        {
            if(!IsChasing())
                TogglePostProcessing();
        }
    }

    private IEnumerator RestorePlayerPosition()
    {
        yield return null; // 한 프레임 기다림
        GameManager.Instance.Player.transform.position = savedPlayerPosition;
        Debug.Log("[PostProcessingToggle] 딜레이 후 복구 위치: " + savedPlayerPosition);
    }

    private bool IsChasing()
    {
        Guard guard = FindObjectOfType<Guard>();
        if (guard.isChasing) return true;
        return false;
    }


}