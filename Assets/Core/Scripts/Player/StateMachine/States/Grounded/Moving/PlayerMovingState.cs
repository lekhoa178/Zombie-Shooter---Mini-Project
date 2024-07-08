using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerMovingState : PlayerGroundedState
{
    public PlayerMovingState(PlayerStateMachine PlayerStateMachine) : base(PlayerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.MovingParameterHash);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.MovingParameterHash);

        base.Exit();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();

        Move();
    }


    private void Move()
    {
        if (stateMachine.ReusableData.MovementInput == Vector2.zero || stateMachine.ReusableData.MovementSpeedModifier == 0f)
        {
            return;
        }

        Vector3 movementDirection = GetMovementDirection();

        float targetRotationYAngle = Rotate(movementDirection);

        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

        float movementSpeed = GetMovementSpeed();

        //stateMachine.Player.Rigidbody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        stateMachine.Player.Agent.Move(Time.deltaTime * targetRotationDirection * movementSpeed);
    }

}
