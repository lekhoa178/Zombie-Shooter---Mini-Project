using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossEarlyAttackingState : BossAttackingState
{

    public BossEarlyAttackingState(BossStateMachine BossStateMachine) : base(BossStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Enemy.AnimationData.EarlyAttackParameterHash);

        stateMachine.Enemy.Agent.updateRotation = false;

        Vector3 dir = Player.Instance.transform.position - stateMachine.Enemy.transform.position;
        dir.y = 0;

        stateMachine.Enemy.transform.rotation = Quaternion.LookRotation(dir);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Enemy.AnimationData.EarlyAttackParameterHash);

        stateMachine.Enemy.Agent.updateRotation = true;

        base.Exit();
    }

    public override void OnAnimationExitEvent()
    {
        stateMachine.ChangeState(stateMachine.JumpAttackingState);
    }

    public override void OnTriggerEnter(Collider collider)
    {
        if (stateMachine.Enemy.LayerData.IsPlayerLayer(collider.gameObject.layer))
        {
            DisableHitBoxes();
        }
    }
}
