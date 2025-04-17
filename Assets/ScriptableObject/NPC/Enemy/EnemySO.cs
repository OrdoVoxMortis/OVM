using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Characters/Enemy")]

public class EnemySO : ScriptableObject
{
    [field: SerializeField] public float PlayerChasingRange { get; private set; }
    [field: SerializeField][field: Range(0.1f, 2f)] public float SeizeRange { get; private set; }

    [field:SerializeField] public PlayerGroundData GroundData { get; private set; }

}
