using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ObjectCapsuleColliderUtility : CapsuleColliderUtility
{
    [field: SerializeField] public TriggerColliderData TriggerColliderData { get; private set; }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        TriggerColliderData.Initialize();
    }
}