using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Custom/Character/Enemy")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public GroundedData GroundedData { get; private set; }

    [field: Header("Properties")]
    [field: SerializeField] public EnemyType EnemyType { get; private set; } = EnemyType.Melee;
    [field: SerializeField] public List<GunSO> Weapons { get; private set; }

    [field: Header("Stats")]
    [field: SerializeField] public float Health { get; private set; } = 5;

    [field: Header("Movements")]
    [field: SerializeField] public float SqrDstToChase { get; private set; } = 40f;
    [field: SerializeField] public float SqrDstToAttack { get; private set; } = 15f;
    [field: SerializeField] public float Acceleration { get; private set; } = 20f;
    [field: SerializeField] public float IdleMovementSpeedModifer { get; private set; } = 0.5f;

}

public enum EnemyType
{
    Melee, Range, Boss
}