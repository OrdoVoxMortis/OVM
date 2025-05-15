using UnityEngine;

public class MouseRotator : MonoBehaviour
{
    [Header("회전 속도")]
    public float rotationSpeed = 5f;

    [Header("마우스 버튼 (0=좌클릭, 1=우클릭, 2=휠클릭)")]
    public int mouseButton = 0;

    private bool isDragging = false;
    private Vector3 lastMousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(mouseButton))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(mouseButton))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotX = -delta.y * rotationSpeed * Time.deltaTime;
            float rotY = delta.x * rotationSpeed * Time.deltaTime;

            transform.Rotate(Camera.main.transform.up, rotY, Space.World);     // 수평 회전
            transform.Rotate(Camera.main.transform.right, rotX, Space.World);  // 수직 회전

            lastMousePosition = Input.mousePosition;
        }
    }
}
