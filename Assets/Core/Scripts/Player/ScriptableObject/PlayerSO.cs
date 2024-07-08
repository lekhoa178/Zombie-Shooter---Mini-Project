using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Custom/Character/Player")]
public class PlayerSO : ScriptableObject
{
    [field: SerializeField] public GroundedData GroundedData { get; private set; }

    [field: Header("Weapon")]
    [field: SerializeField] public List<GunSO> Weapons { get; private set; }

    [field: Header("Stats")]
    [field: SerializeField] public float Health { get; private set; } = 2.5f;

    [field: Header("Immortality")]
    [field: SerializeField] public float TimeToImmortalAfterDamaged { get; private set; } = 2.5f;
    [field: SerializeField] public AnimationCurve EmissionCurve { get; private set; }
}