using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : NPC
{
    public GameObject[] waitPositions;
    public Transform startPosition;

    protected override void Start()
    {
        base.Start();
        startPosition = transform;
    }

    public GameObject GetWaitPosition()
    {
        if(waitPositions.Length > 0)
        {
            return waitPositions[Random.Range(0, waitPositions.Length)];
        }
        return null;
    }

}
