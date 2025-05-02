using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : NPC
{
    public void NotifyTarget(Target target, System.Action onComplete)
    {
        Debug.Log("nofity");
        StartCoroutine(WaitCo(target, onComplete));
        
    }

    private IEnumerator WaitCo(Target target, System.Action onComplete)
    {
        target.FriendPosition = transform.position;
        Debug.Log(target.IsNotified);
        yield return new WaitForSeconds(6.55f);
        target.IsNotified = true;
        stateMachine.ChangeState(stateMachine.IdleState);
        onComplete?.Invoke();
    }
}
