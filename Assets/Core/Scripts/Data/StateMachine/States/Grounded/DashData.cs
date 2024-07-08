using System;
using UnityEngine;

[Serializable]
public class DashData
{
    [field: SerializeField][field: Range(0f, 10f)] public float SpeedModifier { get; private set; } = 2.225f;
    [field: SerializeField] public RotationData RotationData { get; private set; }
    [field: SerializeField] public float TimeToSprint { get; private set; } = 0.2f;
}
