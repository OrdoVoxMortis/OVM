using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    // 애니메이터 파라미터명을 지정
    [SerializeField] private string groundParameterName = "@Ground";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string interactionParameterName = "Interaction";
    [SerializeField] private string walkParameterName = "Walk";
    [SerializeField] private string runParameterName = "Run";
    [SerializeField] private string sitParameterName = "Sit";
    [SerializeField] private string skipSitDownParameterName = "SkipSitDown";
    [SerializeField] private string squatParameterName = "Squat";

    [SerializeField] private string lockpickParameterName = "LockPick";

    [SerializeField] private string airParameterName = "@Air";
    [SerializeField] private string jumpParameterName = "Jump";
    [SerializeField] private string fallParameterName = "Fall";



    // string 이름을 int 해시값을 이용해 저장할 변수
    public int GroundParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int InteractionParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int SitParameterHash {  get; private set; } 
    public int SkipSitDownParameterHash { get; private set; }
    public int SquatParameterHash { get; private set; }   

    public int LockpickParameterHash { get; private set; }

    public int AirParameterHash {  get; private set; }
    public int JumpParameterHash { get; private set; }
    public int FallParameterHash {  get; private set; }

    public void Initialize()
    {
        //  Animator.StringToHash(string) 을 호출하여 파라미터명을 해시로 변환합니다.
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        InteractionParameterHash = Animator.StringToHash(interactionParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        SitParameterHash = Animator.StringToHash(sitParameterName);
        SkipSitDownParameterHash = Animator.StringToHash(skipSitDownParameterName);
        SquatParameterHash = Animator.StringToHash(squatParameterName);

        LockpickParameterHash = Animator.StringToHash(lockpickParameterName);

        AirParameterHash = Animator.StringToHash(airParameterName);
        JumpParameterHash = Animator.StringToHash(jumpParameterName);
        FallParameterHash = Animator.StringToHash(fallParameterName);

    }

}
