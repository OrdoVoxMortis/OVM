using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PostProcessingToggle : MonoBehaviour
{
    public bool isEnabled = false; // 기본적으로 활성화 상태
    public GameObject timeLine_UI;

    void Start()
    {
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = isEnabled;
        PlayerController input = GameManager.Instance.Player.Input;
        input.playerActions.Simulate.started += OnSimulateInput;
    }

    public void TogglePostProcessing()
    {
        isEnabled = !isEnabled;
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = isEnabled;
        if(isEnabled && timeLine_UI != null)
        {
            timeLine_UI.SetActive(true);
            UIManager.Instance.UIActive();
        }
        else
        {
            timeLine_UI.SetActive(false);
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