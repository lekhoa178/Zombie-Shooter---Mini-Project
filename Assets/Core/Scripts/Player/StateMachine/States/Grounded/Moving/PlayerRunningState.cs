using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunningState : PlayerMovingState
{
    private RunData RunData;
    private float _StartTime;
    public PlayerRunningState(PlayerStateMachine PlayerStateMachine) : base(PlayerStateMachine)
    {
        RunData = groundedData.RunData;
    }

    public override void Enter()
    {
        _StartTime = Time.time;

        stateMachine.ReusableData.MovementSpeedModifier = RunData.speedModifier;

        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);

        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (!stateMachine.ReusableData.ShouldWalk)
        {
            return;
        }

        if (Time.time < _StartTime + groundedData.SprintData.RunToWalkTime)
        {
            return;
        }

        StopRunning();
    }

    private void StopRunning()
    {
        if (stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            //stateMachine.ChangeState(stateMachine.MediumStoppingState);
            stateMachine.ChangeState(stateMachine.IdlingState);

            return;
        }

        stateMachine.ChangeState(stateMachine.WalkingState);
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext obj)
    {
        base.OnMovementCanceled(obj);

        //stateMachine.ChangeState(stateMachine.MediumStoppingState);
        stateMachine.ChangeState(stateMachine.IdlingState);
    }
    protected override void OnWalkToggleStarted(InputAction.CallbackContext obj)
    {
        base.OnWalkToggleStarted(obj);

        stateMachine.ChangeState(stateMachine.WalkingState);
    }
}
