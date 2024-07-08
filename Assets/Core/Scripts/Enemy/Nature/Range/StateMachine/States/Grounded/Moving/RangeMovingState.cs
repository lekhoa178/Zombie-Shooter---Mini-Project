using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RangeMovingState : RangeGroundedState
{
    private bool movingFollowAnimation;

    public RangeMovingState(RangeStateMachine RangeStateMachine) : base(RangeStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Enemy.AnimationData.MovingParameterHash);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Enemy.AnimationData.MovingParameterHash);

        base.Exit();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();

        Move();
    }

    public override void Update()
    {
        base.Update();

        if (GetSqrDstToPlayer() < stateMachine.Enemy.Data.SqrDstToAttack)
        {
            if (Time.time > stateMachine.Enemy.CooldownData.TimeToBreakStart + stateMachine.Enemy.CooldownData.TimeToBreak)
                stateMachine.ChangeState(stateMachine.StrikingState);
            else
                stateMachine.ChangeState(stateMachine.IdlingState);
        }
    }

    private void Move()
    {
        stateMachine.Enemy.Agent.SetDestination(Player.Instance.transform.position);
        stateMachine.Enemy.Agent.speed = groundedData.BaseSpeed * stateMachine.ReusableData.MovementSpeedModifier;
    }


    public override void OnAnimationEnterEvent()
    {
        movingFollowAnimation = true;
    }

    public override void OnAnimationExitEvent()
    {
        movingFollowAnimation = false;
    }
}
