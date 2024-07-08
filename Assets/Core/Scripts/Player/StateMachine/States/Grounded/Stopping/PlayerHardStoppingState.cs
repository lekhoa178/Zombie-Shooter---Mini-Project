using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerHardStoppingState : PlayerStoppingState
{
    private float startTime;
    private float timeToFree = 0.5f;

    public PlayerHardStoppingState(PlayerStateMachine PlayerStateMachine) : base(PlayerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeedModifier = 0;
        ResetVelocity();

        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.HardStopParameterHash);

        startTime = Time.time;
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.HardStopParameterHash);

        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Time.time > startTime + timeToFree)
        {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.IdlingState);

                return;
            }

            OnMove();
        }
    }

    public override void OnAnimationExitEvent()
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
    }
}
