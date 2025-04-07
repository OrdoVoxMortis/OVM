using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    public Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public float minXLook;
    public float maxXLook;
    private float charCurXRot;
    private float charCurYRot;
    private float camCurXRot;
    public float lookSensitive;
    private Vector2 mouseDelta;
    public bool canLook = true;

    public Action inventory;
    public Action interation;

    private Rigidbody _rigidbody;
    private Camera _camera;





    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = Camera.main;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move();

    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CharacterLook();
        }
    }

    void Move()
    {
        Vector3 dirction = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dirction *= moveSpeed;

        // 점프를 했을 때만 움직여야 하므로 값을 유지시키기
        dirction.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dirction;
    }

    void CharacterLook()
    {
        charCurXRot += mouseDelta.y * lookSensitive;
        charCurXRot = Mathf.Clamp(charCurXRot, minXLook, maxXLook);

        charCurYRot += mouseDelta.x * lookSensitive;

        transform.localEulerAngles = new Vector3(0, charCurYRot, 0);

        _camera.transform.localEulerAngles = new Vector3(-charCurXRot, 0, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnInterction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            interation?.Invoke();
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    bool isGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)

        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;

    }

    public void AddMoveSpeed(float value)
    {
        moveSpeed += value;
    }

    public void AddJumpPower(float value)
    {
        jumpPower += value;
    }


    private void OnDrawGizmos()
    {
        Vector3 origin1 = transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f);
        Vector3 origin2 = transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f);
        Vector3 origin3 = transform.position + (transform.right * 0.2f) + (transform.up * 0.01f);
        Vector3 origin4 = transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin1, origin1 + Vector3.down * 0.2f);
        Gizmos.DrawLine(origin2, origin2 + Vector3.down * 0.2f);
        Gizmos.DrawLine(origin3, origin3 + Vector3.down * 0.2f);
        Gizmos.DrawLine(origin4, origin4 + Vector3.down * 0.2f);

        Gizmos.color = Color.gray;

    }

}
