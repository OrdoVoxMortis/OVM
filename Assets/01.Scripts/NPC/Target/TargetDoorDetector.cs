using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDoorDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("door open");
        DoorController door = other.GetComponent<DoorController>();
        if (door == null) return;
        door.OpenDoor();
    }
}
