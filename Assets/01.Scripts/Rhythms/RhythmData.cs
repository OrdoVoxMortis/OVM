using UnityEngine;

[CreateAssetMenu(fileName = "RhythmData", menuName = "Rhythm/RhythmData")]
public class RhythmData : ScriptableObject
{
    public string musicName;
    public string musicPath;
    public float bpm;
    public AnimationCurve musicCurve;


}
