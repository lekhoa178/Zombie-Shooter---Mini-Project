using System;
using UnityEngine;

[Serializable]
public class RotationData
{
    [field: SerializeField] public Vector3 TargetRotationReachTime { get; private set; }
}