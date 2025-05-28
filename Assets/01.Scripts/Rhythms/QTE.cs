
using UnityEngine;
using UnityEngine.UI;

public abstract class QTE: MonoBehaviour
{
    [HideInInspector]
    public int qteIndex;
    [HideInInspector]
    public QTEManager manager;

    public float outerLineSize;
    public RectTransform outerLine;

    public Image innerImage;

    public Gradient gradient;

    public GameObject particle;

    [HideInInspector]
    public bool isChecked = false;
    [HideInInspector]
    public bool isPointNotes = false;

    public float[] judges = new float[3] { 0.2f, 0.3f, 0.4f }; //perfect, good, miss, 0.4이후론 fail

    public abstract void CheckJudge();
}
