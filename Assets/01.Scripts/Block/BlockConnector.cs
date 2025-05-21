using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BlockConnector : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public List<TimelineElement> elements = new();

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        TimelineManager.Instance.SetBlockConnector(this);
    }
    private void LateUpdate()
    {
        lineRenderer.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
    private void Start()
    {
        TimelineManager.Instance.OnBlockUpdate += DrawLines;
        GameManager.Instance.OnSimulationMode += ToggleLine;
        gameObject.SetActive(false);
    }
    public void DrawLines()
    {
        if(!gameObject.activeSelf) gameObject.SetActive(true);
        lineRenderer.positionCount = 0;
        elements = TimelineManager.Instance.PlacedBlocks;
        List<Vector3> fullPathPoints = new();
        if (elements.Count <= 1) return;
        for (int i = 0; i < elements.Count -1; i++)
        {
            Vector3 start = elements[i].transform.position;
            Vector3 end = elements[i + 1].transform.position;

            NavMeshPath path = new();

            if (NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path))
            {
                fullPathPoints.AddRange(path.corners);
            }
            else Debug.LogWarning($"[PATH FAIL] from {start} to {end}");
        }

        if (elements.Count > 0) fullPathPoints.Add(elements[^1].transform.position);

        lineRenderer.positionCount = fullPathPoints.Count;
        lineRenderer.SetPositions(fullPathPoints.ToArray());

    }

    private void ToggleLine()
    {
        gameObject.SetActive(!GameManager.Instance.SimulationMode);

    }

}
