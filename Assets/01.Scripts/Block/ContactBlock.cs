using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    protected override void Awake()
    {
        base.Awake();

    }
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
            string[] splits = d.Split(',').Select(s=> s.Trim()).ToArray();
            if (!int.TryParse(splits[0], out int triggerId))
            {
                Debug.LogWarning($"triggerBlockId 변환 실패: {splits[0]}");
                continue;
            }
            ConditionalSequence sequence = new ConditionalSequence()
            {
                triggerBlockId = triggerId,
                successClip = ResourceManager.Instance.LoadAnimationClip(splits[1]),
                failClip = ResourceManager.Instance.LoadAnimationClip(splits[2])
            };
            conditionalSequences.Add(sequence);
        }

    }

    public override void SetGhost()
    {
        if (ghostManager == null) return;

        AnimationClip selectedClip = null;

        if(conditionalSequences != null)
        {
            var match = conditionalSequences.FirstOrDefault(seq => seq.triggerBlockId == matchedTriggerId);
            
            if(match != null)
            {
                selectedClip = IsSuccess ? match.successClip : match.failClip;
            }
        }

        if(selectedClip == null)
        {
            selectedClip = IsSuccess ? SuccessSequence : FailSequence;
        }

        ghostManager.ghostClip = selectedClip;

        var animatorController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        foreach (var clip in animatorController.animationClips)
        {
            animatorController[clip.name] = ghostManager.ghostClip;
        }
        animator.runtimeAnimatorController = animatorController;
        ghostManager.SetBeatList(ghostManager.beats, ghostManager.pointNoteList, ghostManager.bpm);
    }
}
