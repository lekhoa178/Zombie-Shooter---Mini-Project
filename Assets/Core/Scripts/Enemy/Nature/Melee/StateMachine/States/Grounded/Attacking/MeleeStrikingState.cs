using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeStrikingState : MeleeAttackingState
{

    public MeleeStrikingState(MeleeStateMachine MeleeStateMachine) : base(MeleeStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.MovementSpeedModifier = 1f;

        StartAnimation(stateMachine.Enemy.AnimationData.StrikeParameterHash);

        stateMachine.Enemy.Agent.updateRotation = false;

        Vector3 dir = Player.Instance.transform.position - stateMachine.Enemy.transform.position;
        dir.y = 0;

        stateMachine.Enemy.transform.rotation = Quaternion.LookRotation(dir);

        EnableHitBoxes();
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Enemy.AnimationData.StrikeParameterHash);

        stateMachine.Enemy.Agent.updateRotation = true;

        base.Exit();

        DisableHitBoxes();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();

        Move();
    }

    public override void Update()
    {
        base.Update();

        if (GetSqrDstToPlayer() > stateMachine.Enemy.Data.SqrDstToAttack)
        {
            stateMachine.ChangeState(stateMachine.RunningState);
        }
    }

    public override void OnAnimationExitEvent()
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
    }

    public override void OnTriggerEnter(Collider collider)
    {
        if (stateMachine.Enemy.LayerData.IsPlayerLayer(collider.gameObject.layer))
        {
            DisableHitBoxes();
        }
    }
}
