using UnityEngine;
using UnityEngine.AI;

public interface IEntity
{
    NavMeshAgent Agent { get; }
    Animator Animator { get; }
    Camera Camera { get; }
    ObjectCapsuleColliderUtility ColliderUtility { get; }
    Transform MainCameraTransform { get; }
    Rigidbody Rigidbody { get; }

    void OnMovementStateAnimationEnterEvent();
    void OnMovementStateAnimationExitEvent();
    void OnMovementStateAnimationTransitionEvent();
    void TakeDamage(float damage);
}