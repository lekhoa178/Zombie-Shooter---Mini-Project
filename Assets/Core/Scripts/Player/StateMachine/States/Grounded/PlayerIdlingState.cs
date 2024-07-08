using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdlingState : PlayerGroundedState
{
    private IdleData IdleData;

    public PlayerIdlingState(PlayerStateMachine PlayerStateMachine) : base(PlayerStateMachine)
    {
        IdleData = groundedData.IdleData;
    }

    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeedModifier = 0f;
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);

        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            return;
        }

        OnMove();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();

        if (!IsMovingHorizontally())
        {
            return;
        }

        ResetVelocity();
    }

    protected override void OnDashStarted(InputAction.CallbackContext obj)
    {
    }
}
