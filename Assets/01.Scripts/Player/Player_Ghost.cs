using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class Player_Ghost : MonoBehaviour
{
    public Animator anim;



    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Initialize(Player player)
    {
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }

        // 위치와 회전상태 동기화
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;

        // 플레이어와 같은 아바타, 애니메이터 컨트롤러 복사
        anim.runtimeAnimatorController = player.Animator.runtimeAnimatorController;
        anim.avatar = player.Animator.avatar;

        // Bool 파라미터 전부 복사
        foreach (var param in player.Animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                anim.SetBool(param.nameHash, player.Animator.GetBool(param.nameHash));
            }
        }

        // 내부 상태 초기화 및 첫 프레임 업데이트
        anim.Rebind();
        anim.Update(0f);

        // 레이어(0)에서 현재 재생 중인 클립 정보 가져오기
        var clipInfos = player.Animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfos.Length == 0)
        {
            Debug.LogWarning("고스트 : 레이어 0에 클립이 없습니다.");
            return;
        }

        // 여러 클립 중 weight가 가장 높은 클립을 선택
        var best = clipInfos[0];
        for (int i = 1; i < clipInfos.Length; i++)
        {
            if (clipInfos[i].weight > best.weight)
            {
                best = clipInfos[i];
            }
        }

        var clip = best.clip;

        // 현재 재생 시간을 계산
        var state = player.Animator.GetCurrentAnimatorStateInfo(0);
        float normalized = state.normalizedTime % 1f;
        float timeSec = normalized * clip.length;

        // 해당 클립의 정확한 시간을 샘플링 적용
        // 수동으로 특정 시점(timeSec) 만큼 재생
        clip.SampleAnimation(anim.gameObject, timeSec);

        // 애니메이터 비활성화(멈춘 포즈 유지)
        anim.enabled = false;

    }




}
