using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintingState : PlayerMovingState
{
    private SprintData SprintData;

    private bool keepDiving;
    private bool shouldResetDiveData;

    private float _StateTime;
    public PlayerSprintingState(PlayerStateMachine PlayerStateMachine) : base(PlayerStateMachine)
    {
        SprintData = groundedData.SprintData;
    }

    public override void Enter()
    {
        _StateTime = Time.time;

        stateMachine.ReusableData.MovementSpeedModifier = SprintData.SpeedModifier;

        stateMachine.ReusableData.ShouldSprint = true;

        shouldResetDiveData = true;

        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.SprintParameterHash);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.SprintParameterHash);

        base.Exit();

        if (shouldResetDiveData)
        {
            keepDiving = false;

            stateMachine.ReusableData.ShouldSprint = false;
        }
    }

    public override void Update()
    {
        base.Update();

        if (keepDiving)
        {
            return;
        }

        if (Time.time - _StateTime < SprintData.SprintToRunTime)
        {
            return;
        }

        StopSprinting();
    }

    private void StopSprinting()
    {
        stateMachine.ChangeState(stateMachine.RunningState); // Temporary
    }

    protected override void RotateTowardsCursor(bool immediate)
    {
        stateMachine.Player.PlayerRendering.transform.localRotation = Quaternion.identity;
    }

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();

        stateMachine.Player.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
    }

    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();

        stateMachine.Player.Input.PlayerActions.Sprint.performed -= OnSprintPerformed;
    }

    private void OnSprintPerformed(InputAction.CallbackContext obj)
    {
        keepDiving = true;

        shouldResetDiveData = true;
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(stateMachine.HardStoppingState);
        //stateMachine.ChangeState(stateMachine.IdlingState);

        base.OnMovementCanceled(obj);
    }
    protected override void OnWalkToggleStarted(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(stateMachine.RunningState);

        base.OnWalkToggleStarted(obj);
    }
    protected override void OnJumpStarted(InputAction.CallbackContext obj)
    {
        shouldResetDiveData = false;

        base.OnJumpStarted(obj);
    }
    protected override void OnFall()
    {
        shouldResetDiveData = true;

        base.OnFall();
    }
}
