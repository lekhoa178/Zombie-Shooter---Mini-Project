using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class PlayerAnimationData : IAnimationData
{
    [Header("State Group Parameter Names")]
    [SerializeField] private string movementParamterName = "Movement";
    [SerializeField] private string groundedParameterName = "Grounded";
    [SerializeField] private string movingParameterName = "Moving";
    [SerializeField] private string stoppingParameterName = "Stopping";
    [SerializeField] private string attackParameterName = "Attacking";

    [Header("Grouded Parameter Names")]
    [SerializeField] private string idleParameterName = "IsIdling";
    [SerializeField] private string dashParameterName = "IsDashing";
    [SerializeField] private string walkParameterName = "IsWalking";
    [SerializeField] private string runParameterName = "IsRunning";
    [SerializeField] private string sprintParameterName = "IsSprinting";
    [SerializeField] private string mediumStopParameterName = "IsMediumStopping";
    [SerializeField] private string hardStopParameterName = "IsHardStopping";
    [SerializeField] private string rollParameterName = "IsRolling";
    [SerializeField] private string velocityXName = "Velocity X";
    [SerializeField] private string velocityZName = "Velocity Z";

    [Header("Attack Parameter Names")]
    [SerializeField] private string shootParameterName = "IsShooting";
    [SerializeField] private string ultimateParameterName = "IsUltimate";
    [SerializeField] private string skillParameterName = "IsSkill";
    [SerializeField] private string speedAttackParameterName = "SpeedAttack";


    //[Header("Direct Threshold Parameter Names")]

    public int MovementParameterHash { get; private set; }
    public int GroundedParameterHash { get; private set; }
    public int MovingParameterHash { get; private set; }
    public int StoppingParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }


    public int IdleParameterHash { get; private set; }
    public int DashParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int SprintParameterHash { get; private set; }
    public int MediumStopParameterHash { get; private set; }
    public int HardStopParameterHash { get; private set; }
    public int RollParameterHash { get; private set; }
    public int VelocityXParameterHash { get; private set; }
    public int VelocityZParameterHash { get; private set; }


    public int ShootParameterHash { get; private set; }
    public int UltimateParameterHash { get; private set; }
    public int SkillParameterHash { get; private set; }
    public int SpeedAttackParameterHash { get; private set; }



    public void Initialize()
    {
        MovementParameterHash = Animator.StringToHash(movementParamterName);
        GroundedParameterHash = Animator.StringToHash(groundedParameterName);
        MovingParameterHash = Animator.StringToHash(movingParameterName);
        StoppingParameterHash = Animator.StringToHash(stoppingParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);

        IdleParameterHash = Animator.StringToHash(idleParameterName);
        DashParameterHash = Animator.StringToHash(dashParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        SprintParameterHash = Animator.StringToHash(sprintParameterName);
        MediumStopParameterHash = Animator.StringToHash(mediumStopParameterName);
        HardStopParameterHash = Animator.StringToHash(hardStopParameterName);
        RollParameterHash = Animator.StringToHash(rollParameterName);
        VelocityXParameterHash = Animator.StringToHash(velocityXName);
        VelocityZParameterHash = Animator.StringToHash(velocityZName);

        ShootParameterHash = Animator.StringToHash(shootParameterName);
        UltimateParameterHash = Animator.StringToHash(ultimateParameterName);
        SkillParameterHash = Animator.StringToHash(skillParameterName);
        SpeedAttackParameterHash = Animator.StringToHash(speedAttackParameterName);
    }

}
