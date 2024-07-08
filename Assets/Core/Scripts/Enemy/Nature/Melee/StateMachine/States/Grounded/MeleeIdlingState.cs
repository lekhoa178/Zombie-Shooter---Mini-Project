using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeIdlingState : MeleeGroundedState
{
    private IdleData IdleData;

    private float startTime;
    private float timeToRandomMove = 5f;

    public MeleeIdlingState(MeleeStateMachine MeleeStateMachine) : base(MeleeStateMachine)
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

        float sqrDst = GetSqrDstToPlayer();
        if (sqrDst < stateMachine.Enemy.Data.SqrDstToChase && sqrDst > stateMachine.Enemy.Agent.stoppingDistance)
        {
            stateMachine.ChangeState(stateMachine.RunningState);
        }
        else if (Time.time > startTime + timeToRandomMove)
        {
            stateMachine.Enemy.Agent.speed = stateMachine.Enemy.Data.GroundedData.BaseSpeed *
                stateMachine.Enemy.Data.IdleMovementSpeedModifer;
            stateMachine.Enemy.Agent.SetDestination(stateMachine.Enemy.transform.position +
                3  * (new Vector3(Random.value, 0, Random.value) * 2 - Vector3.one));

            startTime = Time.time;
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
