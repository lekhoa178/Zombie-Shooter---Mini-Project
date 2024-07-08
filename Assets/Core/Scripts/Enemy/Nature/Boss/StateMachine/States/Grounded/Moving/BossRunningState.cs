using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossRunningState : BossMovingState
{
    private RunData RunData;
    public BossRunningState(BossStateMachine BossStateMachine) : base(BossStateMachine)
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
