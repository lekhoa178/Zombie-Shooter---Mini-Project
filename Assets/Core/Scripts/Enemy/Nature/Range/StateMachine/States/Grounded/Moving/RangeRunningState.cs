using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangeRunningState : RangeMovingState
{
    private RunData RunData;

    public RangeRunningState(RangeStateMachine RangeStateMachine) : base(RangeStateMachine)
    {
        RunData = groundedData.RunData;
    }

    public override void Enter()
    {

        stateMachine.ReusableData.MovementSpeedModifier = RunData.speedModifier;

        base.Enter();

        StartAnimation(stateMachine.Enemy.AnimationData.RunParameterHash);

    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Enemy.AnimationData.RunParameterHash);

        base.Exit();
    }

}
