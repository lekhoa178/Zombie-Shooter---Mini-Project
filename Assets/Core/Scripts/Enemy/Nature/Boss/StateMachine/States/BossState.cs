using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossState : IState
{
    protected BossStateMachine stateMachine;

    protected GroundedData groundedData;

    public BossState(BossStateMachine BossStateMachine)
    {
        stateMachine = BossStateMachine;

        groundedData = stateMachine.Enemy.Data.GroundedData;
    }
    public virtual void Enter()
    {
        //Debug.Log("State: " + GetType().Name);

        AddInputActionCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void OnAnimationEnterEvent()
    {

    }

    public virtual void OnAnimationExitEvent()
    {

    }

    public virtual void OnAnimationTransitionEvent()
    {
    }

    public virtual void OnTriggerEnter(Collider collider)
    {

        if (stateMachine.Enemy.LayerData.IsGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGround(collider);

            return;
        }
    }

    public virtual void OnTriggerExit(Collider collider)
    {

        if (stateMachine.Enemy.LayerData.IsGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGroundExit(collider);

            return;
        }
    }

    public virtual void PhysicUpdate()
    {

    }

    public virtual void Update()
    {
    }

    private void ReadMovementInput()
    {
    }
    protected virtual void AddInputActionCallbacks()
    {
    }

    protected virtual void RemoveInputActionCallbacks()
    {
    }

    protected virtual void OnContactWithGround(Collider collider)
    {
    }

    protected virtual void OnContactWithGroundExit(Collider collider)
    {
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Enemy.Animator.SetBool(animationHash, true);
    }
    protected void StopAnimation(int animationHash)
    {
        stateMachine.Enemy.Animator.SetBool(animationHash, false);
    }
    //protected void EnableLayer(int index)
    //{
    //    stateMachine.Player.EnableLayer(index);
    //}
    //protected void DisableLayer(int index)
    //{
    //    stateMachine.Player.DisableLayer(index);
    //}

    protected void SetDirectionThreshold(int animationHash, float directionThreshold)
    {
        stateMachine.Enemy.Animator.SetFloat(animationHash, directionThreshold);
    }
    private void UpdateTargetRotationData(float targetAngle)
    {
        stateMachine.ReusableData.CurrentTargetRotation.y = targetAngle;

        stateMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f;
    }

    protected void RotateTowardsTargetRotation()
    {
        float currentYAngle = stateMachine.Enemy.Rigidbody.rotation.eulerAngles.y;

        if (currentYAngle == stateMachine.ReusableData.CurrentTargetRotation.y)
        {
            return;
        }

        float smoothYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.ReusableData.CurrentTargetRotation.y,
            ref stateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y,
            stateMachine.ReusableData.TimeToReachTargetRotation.y - stateMachine.ReusableData.DampedTargetRotationPassedTime.y);

        stateMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

        Quaternion targetRotation = Quaternion.Euler(0f, smoothYAngle, 0f);

        stateMachine.Enemy.Rigidbody.MoveRotation(targetRotation);
    }

    protected bool IsMovingUp(float minimumVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y > minimumVelocity;
    }

    protected Vector3 GetPlayerVerticalVelocity()
    {
        return new Vector3(0f, stateMachine.Enemy.Rigidbody.velocity.y, 0f);
    }

    protected void ResetVelocity()
    {
        stateMachine.Enemy.Rigidbody.velocity = Vector3.zero;
    }
    protected void ResetVerticalVelocity()
    {
        Vector3 velocity = stateMachine.Enemy.Rigidbody.velocity;
        stateMachine.Enemy.Rigidbody.velocity = new Vector3(velocity.x, 0, velocity.z);
    }

    protected bool IsMovingDown(float minimumVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y < -minimumVelocity;
    }

    protected Vector3 GetMovementDirection()
    {
        return new Vector3(stateMachine.ReusableData.MovementInput.x, 0f, stateMachine.ReusableData.MovementInput.y);
    }

    protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
    {
        float directionAngle = GetDirectionAngle(direction);

        if (shouldConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }

        if (directionAngle != stateMachine.ReusableData.CurrentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }

    protected float GetDirectionAngle(Vector3 direction)
    {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        if (directionAngle < 0)
            directionAngle += 360f;

        return directionAngle;
    }

    protected float AddCameraRotationToAngle(float angle)
    {
        angle += stateMachine.Enemy.MainCameraTransform.eulerAngles.y;

        if (angle > 360f)
            angle -= 360f;

        return angle;
    }

    protected Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    protected float GetSqrDstToPlayer()
    {
        return (Player.Instance.transform.position - stateMachine.Enemy.transform.position).sqrMagnitude;
    }
}
