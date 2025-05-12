using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineCamera : MonoBehaviour
{
    [System.Serializable]
    public class CameraEntry
    {
        [Tooltip("카메라 번호")]
        public int id;
        [Tooltip("Timeline 미사용 시 활성화 할 카메라")]
        public CinemachineVirtualCamera virtualCamera;

        [Header("Timeline 컷신 설정")]
        public bool useTimeline = false;

        [Tooltip("성공 컷신용")]
        public PlayableDirector successTimeline;
        [Tooltip("실패 컷신용")]
        public PlayableDirector failureTimeline;

    }

    [Header("VirtualCamera Setting")]
    public List<CameraEntry> cameraEntries = new List<CameraEntry>();

    private Dictionary<int, CameraEntry> cameras;

    private void Awake()
    {
        cameras = new Dictionary<int, CameraEntry>();
        foreach (var entry in cameraEntries)
        {
            cameras[entry.id] = entry;

                // virtualCamera 비활성화
            if (entry != null && entry.virtualCamera)
                entry.virtualCamera.gameObject.SetActive(false);

            //Timeline Director GameObject 비활성화
            if (entry.successTimeline != null)
                entry.successTimeline.gameObject.SetActive(false);
            if (entry.failureTimeline != null)
                entry.failureTimeline.gameObject.SetActive(false);

        }
    }

    private void Start()
    {
        RhythmManager.Instance.RegisterTimelineCamera(this);
    }

    // 해당 id의 카메라를 활성화
    public void EnableCamera(int id, IRhythmActions action)
    {
        if (!cameras.TryGetValue(id, out var entry))
        {
            Debug.LogWarning($"[{name}] 카메라 {entry.id} 가 존재하지 않음");
            return;
        }

        if (entry.useTimeline)
        {
            List<Block> blocks = TimelineManager.Instance.ReturnBlocks();
            bool isSuccess = blocks[id].IsSuccess;// 성공 여부
            PlayTimeline(entry, isSuccess, action);
        }
        else
        {
            if (entry.virtualCamera != null)
            {
                entry.virtualCamera.gameObject.SetActive(true);
                Debug.Log($"Camera 활성화 id = {id}");

            }
        }

    }

    // 해당 id의 카메라를 비활성화
    public void DisableCamera(int id, IRhythmActions action)
    {
        if (!cameras.TryGetValue(id, out var entry))
        {
            Debug.LogWarning($"[{name}] 카메라 {entry.id} 가 존재하지 않음");
            return;
        }

        if (entry.useTimeline)
        {
            if (entry.successTimeline != null)
            {
                entry.successTimeline.stopped -= OnTimelineStopped;
                entry.successTimeline.Stop();
                entry.successTimeline.gameObject.SetActive(false);
            }
            if (entry.failureTimeline != null)
            {
                entry.failureTimeline.stopped -= OnTimelineStopped;
                entry.failureTimeline.Stop();
                entry.failureTimeline.gameObject.SetActive(false);
            }

        }
        else
        {
            if (entry.virtualCamera != null)
                entry.virtualCamera.gameObject.SetActive(false);
            Debug.Log($"Camera 비 활성화 id = {id}");
        }
    }

    private void PlayTimeline(CameraEntry entry, bool success, IRhythmActions action)
    {
        if (entry.virtualCamera != null)
            entry.virtualCamera.gameObject.SetActive(false);

        if (entry.successTimeline != null)
            entry.successTimeline.gameObject.SetActive(false);
        if (entry.failureTimeline != null)
            entry.failureTimeline.gameObject.SetActive(false);

        PlayableDirector director = success ? entry.successTimeline : entry.failureTimeline;
        if (director == null)
        {
            Debug.LogWarning($"[{name}] ID {entry.id} 의 [{(success ? "성공" : "실패")}] 타임라인이 없습니다.");
            return;
        }
        // 원본 타임라인 길이
        double originDur = director.playableAsset.duration;
        // 유지해야하는 시간
        double holdTime = GetTimelineHoldTime(action);
        
        if (holdTime <= 0)
        {
            Debug.LogWarning($"[{name}] 유효하지 않은 유지시간 입니다.{holdTime}");
            holdTime = originDur;
        }

        // 재생 시작
        director.gameObject.SetActive(true);
        director.time = 0;
        director.Play();

        // 재생 속도 계산 및 적용
        float speed = (float)(originDur / holdTime);
        var rootPlayable = director.playableGraph.GetRootPlayable(0);
        rootPlayable.SetSpeed(speed);

        //종료 이벤트 등록
        director.stopped += OnTimelineStopped;

    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        //이벤트 해제
        director.stopped -= OnTimelineStopped;
        // 속도 리셋
        var rootPlable = director.playableGraph.GetRootPlayable(0);
        rootPlable.SetSpeed(1f);
        // 오브젝트 비활성화
        director.gameObject.SetActive(false);
        

    }

    public double GetTimelineHoldTime(IRhythmActions action)
    {
        if (action is GhostManager ghost)
        {
            var times = ghost.checkTimes;
            if (times != null && times.Count > 0)
            {
                return times[times.Count - 1];
            }
            else
            {
                Debug.LogWarning($"GhostManager [{action}]chechTime이 비어있습니다.");
                return 0f;
            }
        }
        else
        {
            Debug.LogWarning($"IRhythmActions 이 GhostManager타입이 아닙니다.");
            return 0f;
        }
    }

}
