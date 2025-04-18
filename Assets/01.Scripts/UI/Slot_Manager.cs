using System.Collections.Generic;
using UnityEngine;

public class Slot_Manager : MonoBehaviour
{
    public static Slot_Manager Instance { get; private set; }

    private List<UI_Slot> slotList = new List<UI_Slot>();
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
        
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        slotList.Clear();
        slots = GetComponentsInChildren<UI_Slot>();
    }

    public void RefreshSlots()
    {
        InitializeSlots();
    }

    public void AddSlot(UI_Slot newslot)
    {
        if (!slotList.Contains(newslot))
        {
            slotList.Add(newslot);
        }
    }
    public UI_Slot[] GetSlots()
    {
        RefreshSlots(); // 항상 최신 상태 반환
        return slots;
    }
}


