using System;

public class MeleeStateMachine : StateMachine
{
    // Movement Staes
    public Enemy Enemy { get; }
    public MeleeIdlingState IdlingState { get; }
    public MeleeRunningState RunningState { get; }

    public MeleeStrikingState StrikingState { get; }

    public MeleeStateMachine(Enemy enemy)
    {
        Enemy = enemy;

        IdlingState = new MeleeIdlingState(this);
        RunningState = new MeleeRunningState(this);

        StrikingState = new MeleeStrikingState(this);
    }
}
