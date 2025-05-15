
using UnityEngine;
using UnityEngine.UI;

public abstract class QTE: MonoBehaviour
{
    public QTEManager manager;

    public float outerLineSize;
    public RectTransform outerLine;

    public Image innerImage;

    public Gradient gradient;

    public GameObject particle;

    public bool isChecked = false;
    public bool isPointNotes = false;

    public bool isOverGood;

    public float[] judges = new float[3] { 0.2f, 0.3f, 0.4f }; //perfect, good, miss, 0.4이후론 fail

    public abstract void CheckJudge();
}
