using System;
using UnityEngine;

[Serializable]
public class EnemyLayerData : ILayerData
{
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    [field: SerializeField] public LayerMask PlayerLayer { get; private set; }
    [field: SerializeField] public LayerMask PlayerDamageLayer { get; private set; }

    public bool ContainsLayer(LayerMask layerMask, int layer)
    {
        return (1 << layer & layerMask) != 0;
    }

    public bool IsGroundLayer(int layer)
    {
        return ContainsLayer(GroundLayer, layer);
    }

    public bool IsPlayerLayer(int layer)
    {
        return ContainsLayer(PlayerLayer, layer);
    }

    public bool IsPlayerDamageLayer(int layer)
    {
        return ContainsLayer(PlayerDamageLayer, layer);
    }
}