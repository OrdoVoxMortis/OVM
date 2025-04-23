using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{

    public bool isPointNotes = false;
    private float[] judges = new float[3] { 0.2f, 0.3f, 0.4f }; //perfect, good, miss, 0.4이후론 fail


    public void CheckGhost(float judgeTime)
    {

    }
}
