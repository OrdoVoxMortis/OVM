using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    void Update()
    {
        transform.position = transform.position + Vector3.right * Time.deltaTime;
    }
}
