using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangeGroundedState : RangeState
{
    private SlopeData SlopeData;

    public RangeGroundedState(RangeStateMachine RangeStateMachine) : base(RangeStateMachine)
    {
        SlopeData = stateMachine.Enemy.ColliderUtility.SlopeData;
    }

    public override void Enter()
    {
        base.Enter();

        SetBaseRotationData();
        StartAnimation(stateMachine.Enemy.AnimationData.GroundedParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Enemy.AnimationData.GroundedParameterHash);
    }

    public override void PhysicUpdate()
    {
        Float();
    }

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();
    }

    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();
    }

    private void Float()
    {
        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Enemy.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, SlopeData.FloatRayDistance, stateMachine.Enemy.LayerData.GroundLayer))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

            if (slopeSpeedModifier == 0f)
            {
                return;
            }

            float distanceToFloatingPoint = stateMachine.Enemy.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y - hit.distance
                + stateMachine.Enemy.ColliderUtility.DefaultColliderData.Offset;

            if (distanceToFloatingPoint == 0f)
            {
                return;
            }

            float amountToLift = slopeSpeedModifier * distanceToFloatingPoint * SlopeData.StepReachForce - GetPlayerVerticalVelocity().y;

            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);
            stateMachine.Enemy.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }

    private float SetSlopeSpeedModifierOnAngle(float angle)
    {
        float slopeSpeedModifier = groundedData.SlopeSpeedAngle.Evaluate(angle);

        if (stateMachine.ReusableData.MovementOnSlopesSpeedModifier != slopeSpeedModifier)
        {
            stateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;

            //UpdateCameraRecenteringState(stateMachine.ReusableData.MovementInput);
        }

        return slopeSpeedModifier;
    }

    protected float Rotate(Vector3 direction)
    {
        float directionAngle = UpdateTargetRotation(direction);

        RotateTowardsTargetRotation();

        return directionAngle;
    }

    protected void SetBaseRotationData()
    {
        stateMachine.ReusableData.RotationData = groundedData.BaseRotationData;

        stateMachine.ReusableData.TimeToReachTargetRotation = stateMachine.ReusableData.RotationData.TargetRotationReachTime;
    }


    protected override void OnContactWithGroundExit(Collider collider)
    {
        base.OnContactWithGroundExit(collider);

        if (IsThereGroundUnderNearth())
        {
            return;
        }

        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Enemy.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

        Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - stateMachine.Enemy.ColliderUtility.CapsuleColliderData.ColliderVerticalExtents, Vector3.down);

        if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, groundedData.GroundToFallRayDistance, stateMachine.Enemy.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
        {
            OnFall();
        }

    }

    private bool IsThereGroundUnderNearth()
    {
        BoxCollider groundCheckCollider = stateMachine.Enemy.ColliderUtility.TriggerColliderData.GroundCheckCollider;
        Vector3 groundColliderCenterInWorldSpace = groundCheckCollider.bounds.center;

        Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace, stateMachine.Enemy.ColliderUtility.TriggerColliderData.GroundCheckColliderExtents, groundCheckCollider.transform.rotation, stateMachine.Enemy.LayerData.GroundLayer, QueryTriggerInteraction.Ignore);

        return overlappedGroundColliders.Length > 0;
    }


    protected virtual void OnFall()
    {
        //stateMachine.ChangeState(stateMachine.FallingState);
    }

    protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

        Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);

        return playerHorizontalMovement.magnitude > minimumMagnitude;
    }

    protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
    {
        float movementSpeed = groundedData.BaseSpeed * stateMachine.ReusableData.MovementSpeedModifier;

        if (shouldConsiderSlopes)
        {
            movementSpeed *= stateMachine.ReusableData.MovementOnSlopesSpeedModifier;
        }

        return movementSpeed;
    }

    protected Vector3 GetPlayerHorizontalVelocity()
    {
        Vector3 playerHorizontalVelocity = stateMachine.Enemy.Rigidbody.velocity;

        playerHorizontalVelocity.y = 0f;

        return playerHorizontalVelocity;
    }

}
