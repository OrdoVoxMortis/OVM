using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PostProcessingToggle : MonoBehaviour
{
    public bool isEnabled = false; // 기본적으로 활성화 상태
    public GameObject timeLine_UI;
    public GameObject playRhythm_UI;
    public GameObject playerPrefab;
    public Volume volume; // 적용할 볼륨값 가져오기
    private GameObject simulationPlayer;
    private ColorAdjustments colorAdjustments; // 볼륨의 color adjustment 값 가져오기
    private Coroutine saturationCoroutine;
    public float saturationValue = 0.5f;
    PlayerController input;
    private Vector3 savedPlayerPosition;
    public GameObject SimulationPlayer => simulationPlayer;

    void Start()
    {
        GameManager.Instance.SimulationMode = false;
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = isEnabled;
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            colorAdjustments.saturation.value = isEnabled ? saturationValue : 0f;
        }
        input = GameManager.Instance.Player.Input;
        input.playerActions.Simulate.started += OnSimulateInput;

    }

    public void TogglePostProcessing()
    {
        isEnabled = !isEnabled;
        GameManager.Instance.Player.isSimulMode = isEnabled;

        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = isEnabled;

        //if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        //{
        //    float from = colorAdjustments.saturation.value;
        //    float to = isEnabled ? -100f : 0f;

        //    // 기존 코루틴이 있으면 중지하고 새로 시작
        //    if (saturationCoroutine != null)
        //    {
        //        StopCoroutine(saturationCoroutine);
        //    }
        //    saturationCoroutine = StartCoroutine(LerpSaturation(from, to, 1f)); 
        //}
        if (isEnabled && timeLine_UI != null)
        {
            
            GameManager.Instance.SimulationMode = true;
            savedPlayerPosition = GameManager.Instance.Player.transform.position;
            Debug.Log("플레이어 위치 저장!" + savedPlayerPosition);
            //TODO 시뮬레이션 전용 Cancle 구독
            simulationPlayer = Instantiate(playerPrefab, savedPlayerPosition, Quaternion.identity);
            if (simulationPlayer != null)
            {
                Player_Ghost player_Ghost = simulationPlayer.gameObject.GetComponent<Player_Ghost>();
                player_Ghost.Initialize(GameManager.Instance.Player);
            }

            GameManager.Instance.Player.Input.UnsubscribeCancleUI();
            timeLine_UI.SetActive(true);
            playRhythm_UI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            GameManager.Instance.SimulationMode = false;
            if (simulationPlayer != null)
            {
                Destroy(simulationPlayer);
                Debug.Log("시뮬레이션용 플레이어 제거됨");
            }
            StartCoroutine(RestorePlayerPosition());
            Debug.Log("플레이어 원래 위치로 복귀" + savedPlayerPosition);
            //TODO 시뮬레이션 전용 Cancle 구독 해제
            GameManager.Instance.Player.Input.SubscribeCancleUI();
            timeLine_UI.SetActive(false);
            playRhythm_UI.SetActive(false);
            UIManager.Instance.UIDeactive();
        }
        GameManager.Instance.OnSimulationMode?.Invoke();

        foreach(var block in TimelineManager.Instance.GetActiveElements())
        {
            if (block is Block b) b.ToggleGhost();
            else if (block is Event e) e.ToggleOutline();
        }

        Debug.Log("시뮬레이션 모드: " + GameManager.Instance.SimulationMode);
    }
    public void EnablePostProcessing()
    {
        if (isEnabled && timeLine_UI != null)
        {
            if (simulationPlayer != null)
            {
                Destroy(simulationPlayer);
                Debug.Log("기존 시뮬레이션 플레이어 제거됨");
            }
            StartCoroutine(RestorePlayerPosition());
            GameManager.Instance.Player.Input.SubscribeCancleUI();
            timeLine_UI.SetActive(false);
            playRhythm_UI.SetActive(false);
            UIManager.Instance.UIDeactive();
        }
        isEnabled = true;
        GameManager.Instance.Player.isSimulMode = isEnabled;
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = isEnabled;

        if (timeLine_UI != null)
        {
            GameManager.Instance.SimulationMode = true;
            savedPlayerPosition = GameManager.Instance.Player.transform.position;
            Debug.Log("플레이어 위치 저장!" + savedPlayerPosition);
            simulationPlayer = Instantiate(playerPrefab, savedPlayerPosition, Quaternion.identity);
            if (simulationPlayer != null)
            {
                Player_Ghost player_Ghost = simulationPlayer.gameObject.GetComponent<Player_Ghost>();
                player_Ghost.Initialize(GameManager.Instance.Player);
            }
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
        if (context.phase == InputActionPhase.Started && GameManager.Instance.SelectedBGM != null && !GameManager.Instance.Player.isLockpick == true)
        {
            TogglePostProcessing();
        }
    }

    private IEnumerator RestorePlayerPosition()
    {
        yield return new WaitForEndOfFrame(); // 한 프레임 기다림
        GameManager.Instance.Player.transform.position = savedPlayerPosition;
        Debug.Log("[PostProcessingToggle] 딜레이 후 복구 위치: " + savedPlayerPosition);
    }

    //private IEnumerator LerpSaturation(float from, float to, float duration) // 포스트프로세싱 볼륨에 해당된 값을 부드럽게 처리하기 위한 코루틴
    //{
    //    float elapsed = 0f; // 경과한 시간 초기화
    //    while(elapsed < duration)
    //    {
    //        float t = elapsed / duration; // 경과 시간을 비율로 계산
    //        colorAdjustments.saturation.value = Mathf.Lerp(from, to, t); // 시작값과 끝나는 값 보간하여 점차 증가함
    //        elapsed += Time.deltaTime; // 경과시간 프레임마다 증가
    //        yield return null;
    //    }
    //    colorAdjustments.saturation.value = to;
    //}


}