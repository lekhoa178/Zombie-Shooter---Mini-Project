using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerState
{
    private SlopeData SlopeData;

    private int startCount;
    private int delayToStartCast = 3;

    private RaycastHit hit;
    private Ray ray;

    public PlayerGroundedState(PlayerStateMachine PlayerStateMachine) : base(PlayerStateMachine)
    {
        SlopeData = stateMachine.Player.ColliderUtility.SlopeData;
    }

    public override void Enter()
    {
        base.Enter();

        SetBaseRotationData();
        StartAnimation(stateMachine.Player.AnimationData.GroundedParameterHash);

        startCount = Time.frameCount;
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.GroundedParameterHash);
    }


    public override void PhysicUpdate()
    {
        Float();
    }

    public override void Update()
    {
        base.Update();

        RotateTowardsCursor();
    }

    protected virtual void RotateTowardsCursor(bool immediate = false)
    {
        if (Time.frameCount > startCount + delayToStartCast || immediate)
        {
            ray = stateMachine.Player.Camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f, stateMachine.Player.LayerData.RaycastLayer))
            {
                stateMachine.ReusableData.RayCastPoint = hit.point;

                Vector2 dir;
                Vector2 movement = new Vector2(
                    stateMachine.ReusableData.MovementInput.x,
                    stateMachine.ReusableData.MovementInput.y);

                dir.x = -hit.point.x + stateMachine.Player.transform.position.x;
                dir.y = -hit.point.z + stateMachine.Player.transform.position.z;

                dir.Normalize();

                float angle = Mathf.Atan2(dir.y, dir.x);

                float cosAngle = Mathf.Cos(-angle);
                float sinAngle = Mathf.Sin(-angle);

                float velX = -(movement.x * sinAngle + movement.y * cosAngle);
                float velY = (movement.x * cosAngle - movement.y * sinAngle);

                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(-dir.x, 0, -dir.y));
                stateMachine.Player.PlayerRendering.transform.rotation = targetRotation;

                stateMachine.Player.Animator.SetFloat(stateMachine.Player.AnimationData.VelocityXParameterHash, velX);
                stateMachine.Player.Animator.SetFloat(stateMachine.Player.AnimationData.VelocityZParameterHash, velY);

            }

            startCount = Time.frameCount;

        }

    }

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();

        stateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;
        stateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;
        stateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
        stateMachine.Player.Input.PlayerActions.Walk.started += OnWalkToggleStarted;
        stateMachine.Player.Input.PlayerActions.Shoot.started += OnShootStarted;
        stateMachine.Player.Input.PlayerActions.ChangeWeapon.started += OnChangeWeaponStarted;
        //stateMachine.Player.Input.PlayerActions.Ultimate.started += OnUltimateStarted;
        //stateMachine.Player.Input.PlayerActions.Skill.started += OnSkillStarted;
    }

    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();

        stateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;
        stateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;
        stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
        stateMachine.Player.Input.PlayerActions.Walk.started -= OnWalkToggleStarted;
        stateMachine.Player.Input.PlayerActions.Shoot.started -= OnShootStarted;
        stateMachine.Player.Input.PlayerActions.ChangeWeapon.started -= OnChangeWeaponStarted;
        //stateMachine.Player.Input.PlayerActions.Skill.started -= OnSkillStarted;
    }

    private void Float()
    {
        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, SlopeData.FloatRayDistance, stateMachine.Player.LayerData.GroundLayer))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

            if (slopeSpeedModifier == 0f)
            {
                return;
            }

            float distanceToFloatingPoint = stateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y - hit.distance
                + stateMachine.Player.ColliderUtility.DefaultColliderData.Offset;

            if (distanceToFloatingPoint == 0f)
            {
                return;
            }

            float amountToLift = slopeSpeedModifier * distanceToFloatingPoint * SlopeData.StepReachForce - GetPlayerVerticalVelocity().y;

            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);
            stateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
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

    protected virtual void OnMove()
    {
        if (stateMachine.ReusableData.ShouldSprint)
        {
            stateMachine.ChangeState(stateMachine.SprintingState);

            return;
        }
        if (stateMachine.ReusableData.ShouldWalk)
        {
            stateMachine.ChangeState(stateMachine.WalkingState);

            return;
        }

        stateMachine.ChangeState(stateMachine.RunningState);
    }

    protected override void OnContactWithGroundExit(Collider collider)
    {
        base.OnContactWithGroundExit(collider);

        if (IsThereGroundUnderNearth())
        {
            return;
        }

        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

        Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - stateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderVerticalExtents, Vector3.down);

        if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, groundedData.GroundToFallRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
        {
            OnFall();
        }

    }

    private bool IsThereGroundUnderNearth()
    {
        BoxCollider groundCheckCollider = stateMachine.Player.ColliderUtility.TriggerColliderData.GroundCheckCollider;
        Vector3 groundColliderCenterInWorldSpace = groundCheckCollider.bounds.center;

        Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace, stateMachine.Player.ColliderUtility.TriggerColliderData.GroundCheckColliderExtents, groundCheckCollider.transform.rotation, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore);

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
        Vector3 playerHorizontalVelocity = stateMachine.Player.Rigidbody.velocity;

        playerHorizontalVelocity.y = 0f;

        return playerHorizontalVelocity;
    }


    protected virtual void OnJumpStarted(InputAction.CallbackContext obj)
    {
        //stateMachine.ChangeState(stateMachine.JumpingState);
    }

    protected virtual void OnDashStarted(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(stateMachine.DashingState);
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext obj)
    {
    }

    protected virtual void OnWalkToggleStarted(InputAction.CallbackContext obj)
    {
        stateMachine.ReusableData.ShouldWalk = !stateMachine.ReusableData.ShouldWalk;
    }

    protected virtual void OnShootStarted(InputAction.CallbackContext obj)
    {
        if (Time.time > stateMachine.Player.CooldownData.ShootStart + stateMachine.Player.GetCurrentWeapon().ReloadTime)
        {
            stateMachine.ChangeState(stateMachine.ShootingState);
            stateMachine.Player.CooldownData.ShootStart = Time.time;
        }
    }

    protected virtual void OnUltimateStarted(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(stateMachine.UltimateAttackingState);
    }

    protected virtual void OnSkillStarted(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(stateMachine.SkillAttackingState);
    }

    private void OnChangeWeaponStarted(InputAction.CallbackContext context)
    {
        stateMachine.Player.ChangeWeapon();
    }
}
