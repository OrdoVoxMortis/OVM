using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStateMachine : StateMachine
{
    public NPC npc { get; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; } = 3f;
    public float MovementSpeedModifier { get; set; } = 1f;

    public GameObject Target { get; private set; }
    public NpcIdleState IdleState { get; }
    public NpcAlertState AlertState { get; }
    public NpcActionState ActionState { get; }

    public NpcStateMachine(NPC npc)
    {
        this.npc = npc;

        Target = GameManager.Instance.Player.gameObject;
        IdleState = new NpcIdleState(this);
        AlertState = new NpcAlertState(this);
        ActionState = new NpcActionState(this);
    }
}
