using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Custom/Weapon/Gun")]
public class GunSO : ScriptableObject
{
    [field: SerializeField] public GameObject WeaponPrefab { get; private set; }
    [field: SerializeField] public GunType Type { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public PoolingEntity BulletPrefab { get; private set; }
    [field: SerializeField] public PoolingEntity ExplodePrefab { get; private set; }
    [field: SerializeField] public float BulletSpeed { get; private set; } = 3;
    [field: SerializeField] public int Damage { get; private set; } = 1;
    [field: SerializeField] public float ReloadTime { get; private set; } = 0.2f;
    [field: SerializeField] public float DstToDestroy { get; private set; } = 10f;
    [field: SerializeField] public float Radius { get; private set; } = 1f;
    [field: SerializeField] public bool Sustain { get; private set; } = false;


    public string GetMetaString()
    {
        return "~" + name +
            "_" + BulletPrefab.ID + // 1
            "_" + BulletSpeed +     // 2
            "_" + (ExplodePrefab != null ? ExplodePrefab.ID : -1) +          // 3
            "_" + ReloadTime +      // 4
            "_" + DstToDestroy +    // 5
            "_" + Damage +          // 6
            "_" + (int)Type +        // 7
            "_" + Radius +        // 8
            "_" + (Sustain? '1' : '0');        // 8

    }

}

public enum GunType
{
    Straight, Parabolic
}