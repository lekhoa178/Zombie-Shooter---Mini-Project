using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossJumpAttackingState : BossAttackingState
{

    private int jumpCount = 0;
    private GameObject ca;

    public BossJumpAttackingState(BossStateMachine BossStateMachine) : base(BossStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Enemy.AnimationData.JumpAttackParameterHash);

        ca = AnticipationHandler.Instance.CircularAnticipation(Player.Instance.transform.position,
            stateMachine.Enemy.ColliderUtility.DefaultColliderData.Radius * stateMachine.Enemy.transform.localScale.x);
        stateMachine.Enemy.Agent.speed = 1000f;
        stateMachine.Enemy.Agent.SetDestination(Player.Instance.transform.position);

        EnableHitBoxes();
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Enemy.AnimationData.JumpAttackParameterHash);

        stateMachine.Enemy.Agent.speed = 0f;

        base.Exit();

        DisableHitBoxes();

        AnticipationHandler.Instance.ReturnCircularAnticipation(ca);
    }

    public override void OnAnimationExitEvent()
    {

        if (GetSqrDstToPlayer() < stateMachine.Enemy.Data.SqrDstToAttack && jumpCount < 2)
        {
            jumpCount++;
            stateMachine.ChangeState(stateMachine.EarlyAttackingState);
        }
        else
        {
            jumpCount = 0;
            stateMachine.ChangeState(stateMachine.IdlingState);
        }

        CameraShaker.Instance.Shake();
    }

    public override void OnTriggerEnter(Collider collider)
    {
        if (stateMachine.Enemy.LayerData.IsPlayerLayer(collider.gameObject.layer))
        {
            DisableHitBoxes();
        }
    }
}
