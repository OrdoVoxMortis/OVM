using UnityEngine;

public class Slot_Manager : MonoBehaviour
{
    public static Slot_Manager Instance { get; private set; }

    public UI_Slot[] slots;

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            // 필요하면 DontDestroyOnLoad(gameObject); // 씬 이동해도 안 없어지게
        }
        else
        {
            Destroy(gameObject); // 두 번째 인스턴스는 파괴
        }

        slots = GetComponentsInChildren<UI_Slot>();
    }
}


