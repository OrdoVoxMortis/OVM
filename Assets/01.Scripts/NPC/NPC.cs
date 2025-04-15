using GoogleSheet.Core.Type;
using UnityEngine;

[UGS(typeof(NpcType))]
public enum NpcType
{
    Target,
    Guard,
    Friend,
    Normal
}

public class Suspicion
{
    public string grade;
    public float increasePerSec;
    public float decreasePerSec;
    public int maxValue = 100;
}
public class NPC : MonoBehaviour
{
    [SerializeField] private string id; 
    public string Name {  get; private set; } // NPC 이름
    public NpcType Type { get; private set; } // NPC 유형
    public float ViewAngle {  get; private set; } // 시야 각도
    public float ViewDistance {  get; private set; } // 시야 거리
    public Suspicion SuspicionParams { get; private set; } // 의심 수치
    private int curSuspicion; // 현재 의심 수치

    void LoadData()
    {
        var data = DataManager.Instance.npcDict[id];
        Name = data.name;
        Type = data.type;
        ViewAngle = data.viewAngle;
        ViewDistance = data.viewDistance;

        var grade = data.suspicionParams;
        SuspicionParams.grade = DataManager.Instance.suspicionDict[grade].grade;
        SuspicionParams.increasePerSec = DataManager.Instance.suspicionDict[grade].increasePerSec;
        SuspicionParams.decreasePerSec = DataManager.Instance.suspicionDict[grade].decreasePerSec;
    }
}

