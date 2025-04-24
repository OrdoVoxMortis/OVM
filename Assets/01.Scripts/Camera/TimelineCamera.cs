using Cinemachine;
using System.Collections;
using System.Collections.Generic;
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
        public float duration = 3.0f;
    }

    [Header("VirtualCamera Setting")]
    public List<CameraEntry> cameraEntries = new List<CameraEntry>();

    private bool hasPlayed = false;
    private Dictionary<int, CameraEntry> cameras;

    private void Awake()
    {
        cameras = new Dictionary<int, CameraEntry>();
        foreach (var e in cameraEntries)
        {
            if (e != null && e.virtualCamera)
            {
                cameras[e.id] = e;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasPlayed)
        {
            
        }
    }

    //private IEnumerator PlaySequnece()
    //{
    //    foreach (var e in cameras.Values)
    //    {
    //        if (e.virtualCamera != null)
    //        {
    //            e.virtualCamera.Priority = 0;
    //        }
    //    }

    //    //TODO: 타임라인에 등록된 정보로 카메라 연동

        

    //}

}
