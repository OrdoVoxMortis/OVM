using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Event : MonoBehaviour, IInteractable
{
    //QTEManager
    //public int id;
    //public string name;
    //public string description;
    private PostProcessingToggle postProcessingToggle; // 추후 수정
    public bool IsActive { get; set; } // 타임라인 내 활성화
    private void Awake()
    {
        postProcessingToggle = FindObjectOfType<PostProcessingToggle>(); // 추후수정
    }

    protected virtual void LoadData()
    {
        //var data = DataManager.Instance.eventDict[id];
        //name = data.name;
        //description = data.description;
    }
    public string GetInteractComponent()
    {
        if (!IsActive) return "E키를 눌러 활성화";
        else return "X키를 눌러 비활성화";
    }

    public void OnInteract()
    {
        if (!IsActive)
        {
            FindObjectOfType<PostProcessingToggle>().EnablePostProcessing();
            TimelineManager.Instance.AddEventSlot(this);
        }
        else
        {
            IsActive = false;
            Deactive();
            Debug.Log("이벤트 데이터 삭제!");
        }
       
    }

    public void Deactive()
    {
        gameObject.SetActive(false);
    }
}

   
