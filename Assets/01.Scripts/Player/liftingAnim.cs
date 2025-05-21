using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liftingAnim : MonoBehaviour
{
   [SerializeField] private Animator animator;

    void Start()
    {
        animator.Play("Lifting");  // 예: "IntroPose"
    }
}
