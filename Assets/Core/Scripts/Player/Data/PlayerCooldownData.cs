using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class PlayerCooldownData : ICooldownData
{
    [field: SerializeField] public float NorAttackCooldown { get; private set; } = 1f;

    public float ShootStart { get; set; }
}
