using System;
using UnityEngine;

[Serializable]
public class PlayerLayerData : ILayerData
{
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    [field: SerializeField] public LayerMask RaycastLayer { get; private set; }
    [field: SerializeField] public LayerMask EnemyDamageLayer { get; private set; }

    public bool ContainsLayer(LayerMask layerMask, int layer)
    {
        return (1 << layer & layerMask) != 0;
    }

    public bool IsGroundLayer(int layer)
    {
        return ContainsLayer(GroundLayer, layer);
    }

    public bool IsEnemyDamageLayer(int layer)
    {
        return ContainsLayer(EnemyDamageLayer, layer);
    }
}