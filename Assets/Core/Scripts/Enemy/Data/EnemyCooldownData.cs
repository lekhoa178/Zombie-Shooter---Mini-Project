using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class EnemyCooldownData : ICooldownData
{
    [field: SerializeField] public float StrikeCooldown { get; private set; } = 1f;
    public float StrikeStart { get; set; }

    [field: SerializeField] public float TimeToAttack { get; private set; } = 2f;
    public float TimeToAttackStart { get; set; }

    [field: SerializeField] public float TimeToBreak { get; private set; } = 2f;
    public float TimeToBreakStart { get; set; }
}
