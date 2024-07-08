using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerWalkingState : PlayerMovingState
{
    private WalkData WalkData;
    public PlayerWalkingState(PlayerStateMachine PlayerStateMachine) : base(PlayerStateMachine)
    {
        WalkData = groundedData.WalkData;
    }

    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeedModifier = WalkData.speedModifier;

        //stateMachine.ReusableData.BackwardsCameraRecenteringData = walkData.BackwardsCameraRecenteringData;

        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);

    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);

        base.Exit();
        //SetBaseCameraRecenteringData();
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext obj)
    {
        //stateMachine.ChangeState(stateMachine.LightStoppingState);
        stateMachine.ChangeState(stateMachine.IdlingState);

        base.OnMovementCanceled(obj);
    }
    protected override void OnWalkToggleStarted(InputAction.CallbackContext obj)
    {
        base.OnWalkToggleStarted(obj);

        stateMachine.ChangeState(stateMachine.RunningState);
    }
}
