using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private float verticalVelocity;

    public Vector3 Movement => Vector3.up * verticalVelocity;

    private bool jumpTriggered = false;

    private float jumpBufferTime = 0.1f;
    private float jumpTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded)
        {
            if (!jumpTriggered)
            {

                verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        if (jumpTriggered)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= jumpBufferTime)
            {
                jumpTriggered = false;
                jumpTimer = 0f;
            }
        }

    }

    public void Jump(float jumpForce)
    {
        verticalVelocity = jumpForce;
        jumpTriggered = true;
        jumpTimer = 0f;
    }
}
