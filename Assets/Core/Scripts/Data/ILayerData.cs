using System;
using UnityEngine;

public interface ILayerData
{

    public bool ContainsLayer(LayerMask layerMask, int layer);

}