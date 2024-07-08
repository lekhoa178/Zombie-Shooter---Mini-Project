using System;

public class BossStateMachine : StateMachine
{
    // Movement Staes
    public BossEnemy Enemy { get; }
    public BossIdlingState IdlingState { get; }
    public BossRunningState RunningState { get; }

    public BossEarlyAttackingState EarlyAttackingState { get; }
    public BossJumpAttackingState JumpAttackingState { get; }

    public BossStateMachine(BossEnemy enemy)
    {
        Enemy = enemy;

        IdlingState = new BossIdlingState(this);
        RunningState = new BossRunningState(this);

        EarlyAttackingState = new BossEarlyAttackingState(this);
        JumpAttackingState = new BossJumpAttackingState(this);
    }
}
