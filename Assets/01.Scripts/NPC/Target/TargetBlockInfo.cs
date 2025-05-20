using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetBlockStateType
{
    None,       // 상태 전환 없이 다음 블럭위치로 이동
    Idle,       // 도착 후 Idle 상태 실행
    Interaction, // 도착 후 상호작용 모션 실행
    Drinking // 두리번 거리는 모션
}

public class TargetBlockInfo : MonoBehaviour
{
    [Header("블럭 도착 시 행동할 타입")]
    public TargetBlockStateType blockStateType = TargetBlockStateType.None;

    [Header("행동 지속 시간(초)")]
    public float stateDuration = 0f;

    [Header("해당 블럭까지의 이동속도")]
    public float moveSpeed = 0f;

    [Header("Gizmos 구 설정")]
    [SerializeField] private float radius = 1f;
    [SerializeField] private Color color = Color.red;

    public bool isSimationBlockCheck = false;
    public GameObject isSimulationObject;


    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void Start()
    {
    
    }

    public void Update()
    {
        if (isSimationBlockCheck && GameManager.Instance.SimulationMode)
        {
            isSimulationObject.SetActive(true);
        }
    }


}
