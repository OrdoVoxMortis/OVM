using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NpcAnimationData
{
    // 애니메이터 파라미터명을 지정
    [SerializeField] private string groundParameterName = "@Ground";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string interactionParameterName = "Interaction";
    [SerializeField] private string walkParameterName = "Walk";
    [SerializeField] private string runParameterName = "Run";
    [SerializeField] private string triggerParameterName = "Trigger";
    [SerializeField] private string lookAroundParameterName = "LookAround";
    [SerializeField] private string notifyParameterName = "Notify";
    [SerializeField] private string turnLeftParameterName = "TurnLeft";
    [SerializeField] private string turnRightParameterName = "TurnRight";
    [SerializeField] private string talkingParameterName = "Talking";



    // string 이름을 int 해시값을 이용해 저장할 변수
    public int GroundParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int InteractionParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int TriggerParameterHash {  get; private set; }
    public int LookAroundParameterHash { get; private set; }
    public int NotifyParameterHash { get; private set; }
    public int TurnLeftParameterHash { get; private set; }
    public int TurnRightParameterHash { get; private set; }
    public int TalkingParameterHash { get; private set; }

    public void Initialize()
    {
        //  Animator.StringToHash(string) 을 호출하여 파라미터명을 해시로 변환합니다.
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        InteractionParameterHash = Animator.StringToHash(interactionParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        TriggerParameterHash = Animator.StringToHash(triggerParameterName);
        LookAroundParameterHash = Animator.StringToHash(lookAroundParameterName);
        NotifyParameterHash = Animator.StringToHash(notifyParameterName);
        TurnLeftParameterHash = Animator.StringToHash(turnLeftParameterName);
        TurnRightParameterHash = Animator.StringToHash(turnRightParameterName);
        TalkingParameterHash = Animator.StringToHash(talkingParameterName); 
    }
}
