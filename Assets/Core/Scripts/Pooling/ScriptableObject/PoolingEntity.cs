using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Custom/Pooling/Entity")]
public class PoolingEntity : ScriptableObject
{
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }

}