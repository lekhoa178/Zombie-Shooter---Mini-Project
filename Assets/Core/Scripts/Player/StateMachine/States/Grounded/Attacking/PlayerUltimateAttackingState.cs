using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

public class PlayerUltimateAttackingState : PlayerAttackingState
{
    public PlayerUltimateAttackingState(PlayerStateMachine PlayerStateMachine) : base(PlayerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.UltimateParameterHash);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.UltimateParameterHash);

        base.Exit();
    }

    public override void OnAnimationExitEvent()
    {
    }

    protected override void OnUltimateStarted(InputAction.CallbackContext obj)
    {
    }

    protected override void OnShootStarted(InputAction.CallbackContext obj)
    {
    }

    protected override void OnJumpStarted(InputAction.CallbackContext obj)
    {
    }

    protected override void OnDashStarted(InputAction.CallbackContext obj)
    {
    }
}
