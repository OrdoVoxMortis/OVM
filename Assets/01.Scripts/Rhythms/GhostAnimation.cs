using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimation : MonoBehaviour
{
    Animator anim;
    bool isMoving = false;

    [HideInInspector]
    public Vector3 moving;

    public float animationSpeed;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = transform.position + moving * Time.deltaTime;
        }
    }

    public void PlayAnimation()
    {
        anim.speed = animationSpeed;
        isMoving = true;
        if (anim != null)
        {
            anim.enabled = true;
            anim.Play("Ghost");
        }
    }

    public void StopAnimation()
    {
        isMoving = false;
        if (anim != null)
            anim.enabled = false;
    }
}
