using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private float verticalVelocity;

    public Vector3 Movement => Vector3.up * verticalVelocity;

    private bool jumpTriggered = false;

    [SerializeField]private float jumpBufferTime = 0.1f;
    private float jumpTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (controller.isGrounded) //지면을 감지
        {
            if (!jumpTriggered)
            {
                // 착지 직후 중력 보정
                verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
        }
        else
        {
            // 공중에 떠 있다면 중력 가속
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        if (jumpTriggered)
        {
            // 점프 타이머 진행
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
