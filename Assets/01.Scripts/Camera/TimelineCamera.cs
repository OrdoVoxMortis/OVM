using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class TimelineCamera : MonoBehaviour
{
    [System.Serializable]
    public class CameraEntry
    {
        [Tooltip("카메라 번호")]
        public int id;
        [Tooltip("등록할 카메라")]
        public CinemachineVirtualCamera virtualCamera;
        [Tooltip("카메라 지속시간")]
        public float duration = 4.0f;
    }

    [Header("VirtualCamera Setting")]
    public List<CameraEntry> cameraEntries = new List<CameraEntry>();

    private Dictionary<int, CameraEntry> cameras;
    private bool isPlaying = false;

    private void Awake()
    {
        cameras = new Dictionary<int, CameraEntry>();
        foreach (var cameraE in cameraEntries)
        {
            if (cameraE != null && cameraE.virtualCamera)
            {
                cameraE.virtualCamera.gameObject.SetActive(false);
                cameras[cameraE.id] = cameraE;
            }
        }
    }

   public IEnumerator PlayTimelineCam()
    {
        if (isPlaying) yield break;
        isPlaying = true;


        foreach (Block block in TimelineManager.Instance.PlacedBlocks)
        {
            if (cameras.TryGetValue(block.id, out var entry))
            {
                // 해당 카메라의 우선순위를 높혀서 활성화
                entry.virtualCamera.gameObject.SetActive(true);
                Debug.Log($"{entry.virtualCamera.gameObject.name} 카메라 실행");
                // FixedTime 만큼 대기
                yield return new WaitForSeconds(block.FixedTime);
                // 재생 후 다시 원래 우선순위로 돌려놓기
                entry.virtualCamera.gameObject.SetActive(false);


            }
            else
            {
                Debug.LogWarning($"{block.id} 와 연동된 카메라가 없습니다.");
            }
        }
        isPlaying = false;

    }

}
