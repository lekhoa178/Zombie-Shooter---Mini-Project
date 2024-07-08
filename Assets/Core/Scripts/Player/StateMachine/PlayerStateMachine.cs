public class PlayerStateMachine : StateMachine
{
    // Movement Staes
    public Player Player { get; }
    public ReusableData ReusableData { get; }
    public PlayerIdlingState IdlingState { get; }
    public PlayerRunningState RunningState { get; }
    public PlayerWalkingState WalkingState { get; }
    public PlayerDashingState DashingState { get; }
    public PlayerSprintingState SprintingState { get; }

    public PlayerHardStoppingState HardStoppingState { get; }


    public PlayerShootingState ShootingState { get; }
    public PlayerUltimateAttackingState UltimateAttackingState { get; }
    public PlayerSkillAttackingState SkillAttackingState { get; }

    public PlayerStateMachine(Player player)
    {
        Player = player;
        ReusableData = new ReusableData();

        IdlingState = new PlayerIdlingState(this);
        RunningState = new PlayerRunningState(this);
        WalkingState = new PlayerWalkingState(this);
        SprintingState = new PlayerSprintingState(this);
        HardStoppingState = new PlayerHardStoppingState(this);

        DashingState = new PlayerDashingState(this);

        ShootingState = new PlayerShootingState(this);
        UltimateAttackingState = new PlayerUltimateAttackingState(this);
        SkillAttackingState = new PlayerSkillAttackingState(this);
    }
}
