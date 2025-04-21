using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(QTEManager))]
public class QTEEditor : Editor
{
    float bpm;
    List<float> beats;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        QTEManager manager = (QTEManager)target;
        //List<int> beats = new List<int>() {1, 1, 2, 2 ,1, 2, 2, 2, 1, 2, 2};
        beats = manager.beats;
        bpm = manager.bpm;

        if (GUILayout.Button("QTE Test"))
        {
            manager.SetBeatList(beats, bpm);
        }
    }
}

[CustomEditor(typeof(GhostManager))]
public class GhostEditor : Editor
{
    float bpm;
    List<float> beats;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GhostManager manager = (GhostManager)target;
        //List<int> beats = new List<int>() {1, 1, 2, 2 ,1, 2, 2, 2, 1, 2, 2};
        beats = manager.beats;
        bpm = manager.bpm;

        if (GUILayout.Button("MakeGhost"))
        {
            manager.SetBeatList(beats, bpm);
        }

        if (GUILayout.Button("PlayGhost"))
        {
            manager.PlayGhost();
        }
    }
}

[CustomEditor(typeof(RhythmManager))]
public class RhythmEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RhythmManager manager = (RhythmManager)target;

        if (GUILayout.Button("Start Music"))
        {
            manager.StartMusic();
        }

        if (GUILayout.Button("Start Beep"))
        {
            manager.StartBeep();
        }

        if (GUILayout.Button("Start QTE"))
        {
            manager.RhythmAction();
        }
    }
}

