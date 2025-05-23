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
        GameManager.Instance.OnStart += DeactiveLine;
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
                for (int j = 0; j < path.corners.Length - 1; j++)
                {
                    var preciseSegment = GetPreciseNavMeshLine(path.corners[j], path.corners[j + 1], 0.2f); // NavMesh 위 경로 추출
                    fullPathPoints.AddRange(preciseSegment);
                }
            }
            else Debug.LogWarning($"[PATH FAIL] from {start} to {end}");
        }

        if (elements.Count > 0) fullPathPoints.Add(elements[^1].transform.position);

        lineRenderer.positionCount = fullPathPoints.Count;
        lineRenderer.SetPositions(fullPathPoints.ToArray());

    }

    List<Vector3> GetPreciseNavMeshLine(Vector3 from, Vector3 to, float step = 0.2f)
    {
        List<Vector3> pathPoints = new();

        float dist = Vector3.Distance(from, to);
        int steps = Mathf.CeilToInt(dist / step);

        for (int i = 0; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector3 rawPoint = Vector3.Lerp(from, to, t);

            // NavMesh 위 위치 찾기
            if (NavMesh.SamplePosition(rawPoint, out var hit, 1.0f, NavMesh.AllAreas))
            {
                pathPoints.Add(hit.position); // 정확히 NavMesh 위
            }
        }

        return pathPoints;
    }

    private void ToggleLine()
    {
        gameObject.SetActive(GameManager.Instance.SimulationMode);

    }

    private void DeactiveLine()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        TimelineManager.Instance.OnBlockUpdate -= DrawLines;
        GameManager.Instance.OnSimulationMode -= ToggleLine;
        GameManager.Instance.OnStart -= DeactiveLine;
    }
}
