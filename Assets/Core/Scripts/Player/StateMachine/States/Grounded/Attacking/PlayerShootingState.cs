using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootingState : PlayerAttackingState
{
    GameObject antGO;

    public PlayerShootingState(PlayerStateMachine PlayerStateMachine) : base(PlayerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.MovementSpeedModifier = 1f;
        stateMachine.Player.Input.PlayerActions.Shoot.canceled += OnShootCanceled;


        StartAnimation(stateMachine.Player.AnimationData.ShootParameterHash);
        SetDirectionThreshold(stateMachine.Player.AnimationData.SpeedAttackParameterHash,
            stateMachine.Player.GetCurrentWeapon().BulletSpeed);

        RotateTowardsCursor(true);

        if (stateMachine.Player.GetCurrentWeapon().Type == GunType.Parabolic)
            antGO = AnticipationHandler.Instance.CircularAnticipation(stateMachine.ReusableData.RayCastPoint,
                stateMachine.Player.GetCurrentWeapon().Radius);
        else
            Shoot();
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.ShootParameterHash);

        stateMachine.Player.Input.PlayerActions.Shoot.canceled -= OnShootCanceled;

        base.Exit();

        AnticipationHandler.Instance.ReturnCircularAnticipation(antGO);
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();

        Move();
    }

    public override void Update()
    {
        base.Update();

        switch (stateMachine.Player.GetCurrentWeapon().Type)
        {
            case GunType.Straight:
                if (Time.time - stateMachine.Player.CooldownData.ShootStart > stateMachine.Player.GetCurrentWeapon().ReloadTime)
                {
                    stateMachine.Player.CooldownData.ShootStart = Time.time;

                    Shoot();
                }
                break;
            case GunType.Parabolic:
                AnticipationHandler.Instance.UpdatePosition(antGO, stateMachine.ReusableData.RayCastPoint);
                break;
        }

    }

    protected override void OnShootStarted(InputAction.CallbackContext obj)
    {
    }


    private void OnShootCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.Player.GetCurrentWeapon().Type == GunType.Parabolic)
        {
            Shoot();
        }

        if (stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);

            return;
        }

        OnMove();
    }

    
    private void Shoot()
    {
        GameObject bullet = PoolingManager.Instance.GetPool(stateMachine.Player.GetCurrentWeapon().BulletPrefab.ID)
            .Get();

        bullet.name = stateMachine.Player.GetCurrentWeapon().GetMetaString() + "_" +
            stateMachine.ReusableData.RayCastPoint.x + "_" + stateMachine.ReusableData.RayCastPoint.z;
        Vector3 pos = stateMachine.Player.PlayerRendering.transform.position +
                stateMachine.Player.PlayerRendering.transform.forward * 0.2f;
        pos.y = 1f;
        bullet.transform.position = pos;
        bullet.transform.localScale = Mathf.Clamp(stateMachine.Player.GetCurrentWeapon().Radius, 0.2f, 1f)
            * Vector3.one;

        bullet.transform.rotation = Quaternion.LookRotation(stateMachine.Player.PlayerRendering.transform.forward);
        bullet.SetActive(true);
    }
}
