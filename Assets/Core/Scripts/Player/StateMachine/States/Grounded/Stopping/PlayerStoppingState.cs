using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerStoppingState : PlayerGroundedState
{
    public PlayerStoppingState(PlayerStateMachine PlayerStateMachine) : base(PlayerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.StoppingParameterHash);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.StoppingParameterHash);

        base.Exit();
    }
}
