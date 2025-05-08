using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimelineElement : MonoBehaviour, IInteractable
{
    public int id;
    public string Name { get; protected set; }
    public bool IsActive { get; set; }
    public abstract void Deactive();


    public abstract string GetInteractComponent();


    public abstract void OnInteract();


    public abstract void SetInteractComponenet(string newText);

}
