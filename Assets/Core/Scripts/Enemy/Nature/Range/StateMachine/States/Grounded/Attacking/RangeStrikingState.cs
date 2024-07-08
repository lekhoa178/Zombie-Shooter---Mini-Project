using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangeStrikingState : RangeAttackingState
{

    public RangeStrikingState(RangeStateMachine RangeStateMachine) : base(RangeStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.MovementSpeedModifier = 0f;

        StartAnimation(stateMachine.Enemy.AnimationData.StrikeParameterHash);
        SetDirectionThreshold(stateMachine.Enemy.AnimationData.StrikeSpeedParameterHash, 1f / stateMachine.Enemy.GetCurrentWeapon().ReloadTime);

        stateMachine.Enemy.Agent.updateRotation = false;
        stateMachine.Enemy.Agent.isStopped = true;

        stateMachine.Enemy.CooldownData.TimeToAttackStart = Time.time;
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Enemy.AnimationData.StrikeParameterHash);

        stateMachine.Enemy.Agent.isStopped = false;
        stateMachine.Enemy.Agent.updateRotation = true;

        stateMachine.Enemy.CooldownData.TimeToBreakStart = Time.time;

        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Time.time > stateMachine.Enemy.CooldownData.StrikeStart  + stateMachine.Enemy.GetCurrentWeapon().ReloadTime)
        {
            Fire();

            stateMachine.Enemy.CooldownData.StrikeStart = Time.time;
        }

        if (Time.time > stateMachine.Enemy.CooldownData.TimeToAttackStart + stateMachine.Enemy.CooldownData.TimeToAttack)
        {
            if (GetSqrDstToPlayer() > stateMachine.Enemy.Data.SqrDstToAttack)
                stateMachine.ChangeState(stateMachine.RunningState);
            else
                stateMachine.ChangeState(stateMachine.IdlingState);

        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();

        Vector3 dir = Player.Instance.transform.position - stateMachine.Enemy.transform.position;
        dir.y = 0;

        stateMachine.Enemy.transform.rotation = Quaternion.LookRotation(dir);
    }

    public override void OnAnimationExitEvent()
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
    }

    private void Fire()
    {
        GameObject bullet = PoolingManager.Instance.GetPool(stateMachine.Enemy.GetCurrentWeapon().BulletPrefab.ID)
            .Get();
        bullet.name = stateMachine.Enemy.GetCurrentWeapon().GetMetaString() +
            "_" + Player.Instance.transform.position.x + "_" + Player.Instance.transform.position.z;
        bullet.transform.position = stateMachine.Enemy.transform.position +
                stateMachine.Enemy.transform.forward * 0.2f + Vector3.up;
        bullet.transform.localScale = Mathf.Clamp(stateMachine.Enemy.GetCurrentWeapon().Radius, 0.2f, 1f)
                * Vector3.one;

        bullet.transform.rotation = Quaternion.LookRotation(stateMachine.Enemy.transform.forward);
        bullet.SetActive(true);
    }
}
