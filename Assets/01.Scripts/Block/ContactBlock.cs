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
        var raw = DataManager.Instance.blockDict[id].conditionalSequence;

        if (raw.Count % 3 != 0)
        {
            Debug.LogWarning($"[ConditionalSequence] 3개씩 나누어 떨어지지 않음: {raw.Count}개");
            return;
        }

        for (int i = 0; i < raw.Count; i += 3)
        {
            string triggerIdStr = raw[i].Replace("\"", "").Trim();
            string successName = raw[i + 1].Replace("\"", "").Trim();
            string failName = raw[i + 2].Replace("\"", "").Trim();

            if (!int.TryParse(triggerIdStr, out int triggerId))
            {
                Debug.LogWarning($"[ConditionalSequence] triggerBlockId 변환 실패: {triggerIdStr}");
                continue;
            }

            var successClip = ResourceManager.Instance.LoadAnimationClip(successName);
            var failClip = ResourceManager.Instance.LoadAnimationClip(failName);

            conditionalSequences.Add(new ConditionalSequence
            {
                triggerBlockId = triggerId,
                successClip = successClip,
                failClip = failClip
            });
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
