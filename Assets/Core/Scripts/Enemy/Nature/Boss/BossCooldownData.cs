using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class BossCooldownData : ICooldownData
{
    [field: SerializeField] public float TimeToJumpAttack { get; private set; } = 10f;

    public float JumpAttackStart { get; set; }

    [field: SerializeField] public float TimeToJumpConsecutive { get; private set; } = 2f;

    public float JumpConsecutiveStart { get; set; }
}
