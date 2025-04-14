using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curIntercactGameObject;
    private Camera camera;
    private CinemachineFreeLook playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    private void Awake()
    {
        playerCamera = FindObjectOfType<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red);

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curIntercactGameObject)
                {
                    curIntercactGameObject = hit.collider.gameObject;
                    //TODO 텍스트를 출력시켜 줘야함
                }
            }
            else
            {
                curIntercactGameObject = null;
                //TODO 텍스트 출력을 없애 줘야함
            }
        }
        
    }
}
