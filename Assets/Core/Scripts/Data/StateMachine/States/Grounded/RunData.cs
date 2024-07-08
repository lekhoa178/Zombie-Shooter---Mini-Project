using System;
using UnityEngine;

[Serializable]
public class RunData
{
    [field: SerializeField][field: Range(0f, 1f)] public float speedModifier { get; private set; } = 0.225f;
}
