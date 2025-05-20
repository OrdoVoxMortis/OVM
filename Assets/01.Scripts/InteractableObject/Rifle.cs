using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour, IInteractable
{
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    private bool isAiming = false;
    public string GetInteractComponent()
    {
        if (!isAiming) return "E키를 눌러 조준 모드 진입";
        else return string.Empty;
    }

    public void OnInteract()
    {
        if (aimCamera != null && !isAiming)
        {
            aimCamera.Priority = 20;
            isAiming = true;
        }
        else if (isAiming)
        {
            aimCamera.Priority = 0;
            isAiming = false;
        }
    
    }

    public void SetInteractComponenet(string newText)
    {
        throw new System.NotImplementedException();
    }
    public void Deactive()
    {
        throw new System.NotImplementedException();
    }
}
