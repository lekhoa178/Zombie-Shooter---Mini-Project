using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashingState : PlayerGroundedState
{
    private DashData DashData;

    private float startTime;
    private bool shouldKeepRotating;
    public PlayerDashingState(PlayerStateMachine PlayerStateMachine) : base(PlayerStateMachine)
    {
        DashData = groundedData.DashData;
    }

    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeedModifier = DashData.SpeedModifier;

        shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

        stateMachine.ReusableData.RotationData = DashData.RotationData;
        stateMachine.ReusableData.TimeToReachTargetRotation = DashData.RotationData.TargetRotationReachTime;

        startTime = Time.time;

        Dash();

        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.DashParameterHash);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.DashParameterHash);

        base.Exit();

        SetBaseRotationData();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();

        Dash();

        if (!shouldKeepRotating)
        {
            return;
        }

        RotateTowardsTargetRotation();
    }

    public override void Update()
    {
        base.Update();

        if (Time.time > startTime + DashData.TimeToSprint)
        {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                //stateMachine.ChangeState(stateMachine.HardStoppingState);
                stateMachine.ChangeState(stateMachine.IdlingState);

                return;
            }
            stateMachine.ChangeState(stateMachine.SprintingState);
        }

    }

    protected override void RotateTowardsCursor(bool immediate)
    {
        stateMachine.Player.PlayerRendering.transform.localRotation = Quaternion.identity;
    }

    private void Dash()
    {
        Vector3 dashDirection = stateMachine.Player.transform.forward;

        dashDirection.y = 0f;

        UpdateTargetRotation(dashDirection, false);

        if (shouldKeepRotating)
        {
            UpdateTargetRotation(GetMovementDirection());

            dashDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
        }

        stateMachine.Player.Agent.Move(Time.deltaTime * dashDirection * GetMovementSpeed(false));

    }

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();

        stateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;
    }

    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();

        stateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
    }

    private void OnMovementPerformed(InputAction.CallbackContext obj)
    {
        shouldKeepRotating = true;
    }

    public override void OnAnimationTransitionEvent()
    {
        if (stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            //stateMachine.ChangeState(stateMachine.HardStoppingState);
            stateMachine.ChangeState(stateMachine.IdlingState);

            return;
        }
        stateMachine.ChangeState(stateMachine.SprintingState);
    }

    protected override void OnDashStarted(InputAction.CallbackContext obj)
    {
    }

    protected override void OnShootStarted(InputAction.CallbackContext obj)
    {
    }
}
