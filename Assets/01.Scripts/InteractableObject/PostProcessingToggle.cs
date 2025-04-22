using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PostProcessingToggle : MonoBehaviour
{
    public bool isEnabled = false; // 기본적으로 활성화 상태
    public GameObject timeLine_UI;
    public GameObject playRhythm_UI;
    PlayerController input;

    void Start()
    {
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = isEnabled;
        input = GameManager.Instance.Player.Input;
        input.playerActions.Simulate.started += OnSimulateInput;
    }

    public void TogglePostProcessing()
    {
        isEnabled = !isEnabled;
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = isEnabled;
        if(isEnabled && timeLine_UI != null)
        {
            GameManager.Instance.SimulationMode = true;

            //TODO 시뮬레이션 전용 Cancle 구독
            input.playerActions.CancleUI.performed -= input.OnCancleUI;

            timeLine_UI.SetActive(true);
            playRhythm_UI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            GameManager.Instance.SimulationMode = false;
            
            //TODO 시뮬레이션 전용 Cancle 구독 해제
            input.playerActions.CancleUI.performed += input.OnCancleUI;

            timeLine_UI.SetActive(false);
            playRhythm_UI.SetActive(false);
            UIManager.Instance.UIDeactive();
        }
    }
    private void OnSimulateInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && GameManager.Instance.SelectedBGM != null)
        {
            TogglePostProcessing();
        }
    }



}