using System;
using UnityEngine;

[Serializable]
public class GroundedData
{
    [field: SerializeField][field: Range(0f, 200f)] public float BaseSpeed { get; private set; } = 50f;
    [field: SerializeField][field: Range(0f, 5f)] public float GroundToFallRayDistance { get; private set; } = 2f;
    //[field: SerializeField] public List<PlayerCameraRecenteringData> SidewaysCameraRecenteringData { get; private set; }
    //[field: SerializeField] public List<PlayerCameraRecenteringData> BackwardsCameraRecenteringData { get; private set; }
    [field: SerializeField] public AnimationCurve SlopeSpeedAngle { get; private set; }
    [field: SerializeField] public RotationData BaseRotationData { get; private set; }
    [field: SerializeField] public IdleData IdleData { get; private set; }
    [field: SerializeField] public RunData RunData { get; private set; }
    [field: SerializeField] public WalkData WalkData { get; private set; }
    [field: SerializeField] public SprintData SprintData { get; private set; }
    [field: SerializeField] public DashData DashData { get; private set; }

}
