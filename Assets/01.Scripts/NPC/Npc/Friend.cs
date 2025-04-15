using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : NPC
{
    public void NotifyTarget(Target target)
    {
        target.IsNotified = true;
    }
}
