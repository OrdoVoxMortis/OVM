using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    Transform target;
    Renderer _renderer;

    bool isChecked = false;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (isChecked)
            return;

        if (target == null)
            return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            CheckJudge();
        }

        Color color = _renderer.material.color;
        color.a = Mathf.Clamp(1 - (transform.position - target.position).magnitude / 3f, 0.5f, 1f);
        _renderer.material.color = color;
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
        isChecked = true;
        //Destroy(gameObject);
    }
}
