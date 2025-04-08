using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    Transform target;

    private void Update()
    {
        if (target == null)
            return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            CheckJudge();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target = other.GetComponent<Transform>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
    }

    private void CheckJudge()
    {
        if(target == null)
            return;

        float distance = (transform.position - target.position).magnitude;
        if(distance < 0.2f)
        {
            Debug.Log("성공!");
        } else
        {
            Debug.Log("실패!");
        }

        Destroy(gameObject);
    }
}
