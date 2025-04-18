using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : NPC
{
    public void NotifyTarget(Target target)
    {
        Debug.Log("nofity");
        StartCoroutine(WaitCo(target));
        
    }

    private IEnumerator WaitCo(Target target)
    {
        target.IsNotified = true;
        Debug.Log(target.IsNotified);
        yield return new WaitForSeconds(2f);
        stateMachine.ChangeState(stateMachine.IdleState);

    }
}
