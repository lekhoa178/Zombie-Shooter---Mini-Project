using System;
using UnityEngine;

[Serializable]
public class GlobalLayerData : ILayerData
{
    [field: SerializeField] public LayerMask PlayerLayer { get; private set; }
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    [field: SerializeField] public LayerMask EnemyLayer { get; private set; }
    [field: SerializeField] public LayerMask PreventerLayer { get; private set; }


    public bool ContainsLayer(LayerMask layerMask, int layer)
    {
        return (1 << layer & layerMask) != 0;
    }

    public bool IsPlayerLayer(int layer)
    {
        return ContainsLayer(PlayerLayer, layer);
    }

    public bool IsEnemyLayer(int layer)
    {
        return ContainsLayer(EnemyLayer, layer);
    }

    public bool IsPreventerLayer(int layer)
    {
        return ContainsLayer(PreventerLayer, layer);
    }

    public bool IsGroundLayer(int layer)
    {
        return ContainsLayer(GroundLayer, layer);
    }
}