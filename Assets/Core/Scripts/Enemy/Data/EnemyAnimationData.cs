using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class EnemyAnimationData : IAnimationData
{
    [Header("State Group Parameter Names")]
    [SerializeField] private string movementParamterName = "Movement";
    [SerializeField] private string groundedParameterName = "Grounded";
    [SerializeField] private string movingParameterName = "Moving";
    [SerializeField] private string attackParameterName = "Attacking";

    [Header("Grouded Parameter Names")]
    [SerializeField] private string idleParameterName = "IsIdling";
    [SerializeField] private string runParameterName = "IsRunning";

    [Header("Attack Parameter Names")]
    [SerializeField] private string strikeParameterName = "IsStrike";
    [SerializeField] private string strikeSpeedParameterName = "StrikeSpeed";


    //[Header("Direct Threshold Parameter Names")]

    public int MovementParameterHash { get; private set; }
    public int GroundedParameterHash { get; private set; }
    public int MovingParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }


    public int IdleParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }

    public int StrikeParameterHash { get; private set; }
    public int StrikeSpeedParameterHash { get; private set; }


    public void Initialize()
    {
        MovementParameterHash = Animator.StringToHash(movementParamterName);
        GroundedParameterHash = Animator.StringToHash(groundedParameterName);
        MovingParameterHash = Animator.StringToHash(movingParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);

        IdleParameterHash = Animator.StringToHash(idleParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);

        StrikeParameterHash = Animator.StringToHash(strikeParameterName);
        StrikeSpeedParameterHash = Animator.StringToHash(strikeSpeedParameterName);
    }

}
