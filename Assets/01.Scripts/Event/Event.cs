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

    protected virtual void LoadData()
    {
        //var data = DataManager.Instance.eventDict[id];
        //name = data.name;
        //description = data.description;
    }
    public string GetInteractComponent()
    {
        return "E키를 눌러 타임라인에 추가";
    }

    public void OnInteract()
    {
       TimelineManager.Instance.AddEventSlot(this);
    }

    public void Deactive()
    {
        throw new System.NotImplementedException();
    }
}

   
