using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "Characters/NPC")]

public class NPCSO : ScriptableObject
{
    [field: SerializeField] public float DegreeOfDoubt { get; private set; }

    [field: SerializeField] public float GuardTime { get; private set; }

    [field: SerializeField] public PlayerGroundData GroundData { get; private set; }
}
