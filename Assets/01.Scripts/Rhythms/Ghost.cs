using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{

    public bool isPointNotes = false;
    private float[] judges = new float[3] { 0.2f, 0.3f, 0.4f }; //perfect, good, miss, 0.4이후론 fail

    public bool isOverGood;

    public void CheckGhost(float timing)
    {
        StageManager.Instance.StageResult.GhostCheck = true;
        timing = Mathf.Abs(timing);
        if (timing < judges[0])
        {
            Debug.Log("Perfect!");
            isOverGood = true;
        }
        else if (timing < judges[1])
        {
            Debug.Log("Good!");
            isOverGood = true;
            StageManager.Instance.StageResult.GhostCheck = false;
        }
        else if (timing < judges[2])
        {
            Debug.Log("Miss!");
            isOverGood = false;
            StageManager.Instance.StageResult.GhostCheck = false;
        }
        else
        {
            Debug.Log("Fail!");
            isOverGood = false;
            StageManager.Instance.StageResult.GhostCheck = false;
        }
    }
}
