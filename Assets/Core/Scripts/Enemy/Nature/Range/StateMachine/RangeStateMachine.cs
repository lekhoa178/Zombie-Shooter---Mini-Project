using System;

public class RangeStateMachine : StateMachine
{
    // Movement States
    public Enemy Enemy { get; }
    public RangeIdlingState IdlingState { get; }
    public RangeRunningState RunningState { get; }

    public RangeStrikingState StrikingState { get; }

    public RangeStateMachine(Enemy enemy)
    {
        Enemy = enemy;

        IdlingState = new RangeIdlingState(this);
        RunningState = new RangeRunningState(this);

        StrikingState = new RangeStrikingState(this);
    }
}
