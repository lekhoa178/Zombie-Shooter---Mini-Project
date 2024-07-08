using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WalkData
{
    [field: SerializeField][field: Range(0f, 1f)] public float speedModifier { get; private set; } = 0.225f;
    //[field: SerializeField] public List<PlayerCameraRecenteringData> BackwardsCameraRecenteringData { get; private set; }
}
