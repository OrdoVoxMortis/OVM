using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public BoxCollider AreaBounds { get; private set; }

    private void Awake()
    {
        AreaBounds = GetComponent<BoxCollider>();
    }
}
