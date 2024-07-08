using UnityEngine;
using UnityEngine.InputSystem;

public class BossIdlingState : BossGroundedState
{
    private IdleData IdleData;

    private float startTime;
    private float timeToRandomMove = 5f;

    public BossIdlingState(BossStateMachine BossStateMachine) : base(BossStateMachine)
    {
        IdleData = groundedData.IdleData;
    }

    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeedModifier = 0f;
        base.Enter();

        StartAnimation(stateMachine.Enemy.AnimationData.IdleParameterHash);

        startTime = Time.time + Random.Range(2, 4f);

    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Enemy.AnimationData.IdleParameterHash);

        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (GetSqrDstToPlayer() < stateMachine.Enemy.Data.SqrDstToAttack &&
            Time.time > stateMachine.Enemy.CooldownData.JumpAttackStart +
            stateMachine.Enemy.CooldownData.TimeToJumpAttack)
        {
            stateMachine.ChangeState(stateMachine.EarlyAttackingState);

            stateMachine.Enemy.CooldownData.JumpAttackStart = Time.time;
        }
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

}
