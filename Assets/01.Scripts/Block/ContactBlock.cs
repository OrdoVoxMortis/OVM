using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ConditionalSequence
{
    public int triggerBlockId;
    public AnimationClip successClip;
    public AnimationClip failClip;
}

public class ContactBlock : Block
{
    public bool IsTarget { get; private set; } // 타겟
    public bool IsDeath {  get; set; }  
    public List<ConditionalSequence> conditionalSequences = new();
    public int matchedTriggerId = -1;
    
    public override void LoadData()
    {
        base.LoadData();
        IsTarget = DataManager.Instance.blockDict[id].isTarget;
        ConvertData();
    }

    public void ConvertData()
    {
        var data = DataManager.Instance.blockDict[id].conditionalSequence;
        foreach (var d in data)
        {
            string[] splits = d.Split(',');
            ConditionalSequence sequence = new ConditionalSequence()
            {
                triggerBlockId = int.Parse(splits[0]),
                successClip = ResourceManager.Instance.LoadAnimationClip(splits[1]),
                failClip = ResourceManager.Instance.LoadAnimationClip(splits[2])
            };
            conditionalSequences.Add(sequence);
        }

    }
}
