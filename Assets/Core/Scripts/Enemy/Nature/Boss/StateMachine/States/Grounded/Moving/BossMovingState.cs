using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BossMovingState : BossGroundedState
{
    public BossMovingState(BossStateMachine BossStateMachine) : base(BossStateMachine)
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

    private void Move()
    {
        //if (stateMachine.ReusableData.MovementInput == Vector2.zero || stateMachine.ReusableData.MovementSpeedModifier == 0f)
        //{
        //    return;
        //}

        //Vector3 movementDirection = GetMovementDirection();

        //float targetRotationYAngle = Rotate(movementDirection);

        //Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

        //float movementSpeed = GetMovementSpeed();

        ////stateMachine.Player.Rigidbody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        //stateMachine.Enemy.Agent.Move(Time.deltaTime * targetRotationDirection * movementSpeed);


        stateMachine.Enemy.Agent.SetDestination(Player.Instance.transform.position);
        stateMachine.Enemy.Agent.speed = groundedData.BaseSpeed * stateMachine.ReusableData.MovementSpeedModifier;
    }

}
