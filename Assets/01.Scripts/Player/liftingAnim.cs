using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liftingAnim : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Lifting");  // 예: "IntroPose"
    }
}
