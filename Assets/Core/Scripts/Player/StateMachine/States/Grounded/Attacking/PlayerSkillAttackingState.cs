using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

public class PlayerSkillAttackingState : PlayerAttackingState
{
    public PlayerSkillAttackingState(PlayerStateMachine PlayerStateMachine) : base(PlayerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.SkillParameterHash);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.SkillParameterHash);

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
