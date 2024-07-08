using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class FallData
{
    [field: SerializeField][field: Range(1f, 100f)] public float FallSpeedLimit { get; private set; } = 15f;
    [field: SerializeField][field: Range(0f, 1f)] public float AccelerationPercent { get; private set; } = 0.2f;
    [field: SerializeField][field: Range(1f, 100f)] public float MinimumDistanceToBeConsideredHardFall { get; private set; } = 3f;

}
