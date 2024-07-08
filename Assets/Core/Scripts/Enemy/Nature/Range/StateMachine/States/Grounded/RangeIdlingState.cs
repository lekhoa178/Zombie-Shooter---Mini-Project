using UnityEngine;
using UnityEngine.InputSystem;

public class RangeIdlingState : RangeGroundedState
{
    private IdleData IdleData;

    public RangeIdlingState(RangeStateMachine RangeStateMachine) : base(RangeStateMachine)
    {
        IdleData = groundedData.IdleData;
    }

    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeedModifier = 0f;
        base.Enter();

        StartAnimation(stateMachine.Enemy.AnimationData.IdleParameterHash);

        try
        {
            stateMachine.Enemy.Agent.SetDestination(stateMachine.Enemy.transform.position);
        }
        catch
        {
            Debug.Log("Warning from pooling init");
        }
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Enemy.AnimationData.IdleParameterHash);

        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        float sqrDst = GetSqrDstToPlayer();
        if (sqrDst < stateMachine.Enemy.Data.SqrDstToChase && sqrDst > stateMachine.Enemy.Data.SqrDstToAttack)
        {
            stateMachine.ChangeState(stateMachine.RunningState);
        }
        else if (sqrDst < stateMachine.Enemy.Data.SqrDstToAttack)
        {
                        if (Time.time > stateMachine.Enemy.CooldownData.TimeToBreakStart + stateMachine.Enemy.CooldownData.TimeToBreak)
            {
                stateMachine.ChangeState(stateMachine.StrikingState);

                stateMachine.Enemy.CooldownData.TimeToBreakStart = Time.time;
            }
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
