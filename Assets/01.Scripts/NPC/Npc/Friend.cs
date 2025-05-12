using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : NPC
{
    public bool IsNotifying { get; private set; } = false;
    public void NotifyTarget(Target target, System.Action onComplete)
    {
        if (IsNotifying) return;
        IsNotifying = true;
        Debug.Log("nofity");
        StartCoroutine(WaitCo(target, onComplete));
        
    }

    private IEnumerator WaitCo(Target target, System.Action onComplete)
    {
        target.FriendPosition = transform.position;
        Debug.Log(target.IsNotified);
        target.IsNotified = true;

        yield return new WaitForSeconds(10.5f);

        IsNotifying = false;
        stateMachine.ChangeState(stateMachine.IdleState);
        onComplete?.Invoke();
    }
}
