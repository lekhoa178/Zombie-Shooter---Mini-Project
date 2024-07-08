using System;
using UnityEngine;

[Serializable]
public class AirborneData
{
    [field: SerializeField] public RotationData BaseRotationData { get; private set; }
    [field: SerializeField] public JumpData JumpData { get; private set; }
    [field: SerializeField] public FallData FallData { get; private set; }
}
