using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IEntity
{

    [field: Header("Reference")]
    [field: SerializeField] public EnemySO Data { get; private set; }

    [field: Header("Collisions")]
    [field: SerializeField] public ObjectCapsuleColliderUtility ColliderUtility { get; private set; }
    [field: SerializeField] public EnemyLayerData LayerData { get; private set; }
    [field: SerializeField] public List<Collider> HitBoxes {  get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public EnemyAnimationData AnimationData { get; private set; }

    [field: Header("Cooldown Data")]
    [field: SerializeField] public EnemyCooldownData CooldownData { get; private set; }

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public Transform MainCameraTransform { get; private set; }
    public Camera Camera { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    private StateMachine MovementStateMachine;
    public int CurrentWeapon { get; private set; }

    public Material Material => throw new NotImplementedException();

    public GunSO GetCurrentWeapon()
    {
        return Data.Weapons[CurrentWeapon];
    }

    private float maxHealth;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        Camera = Camera.main;

        AnimationData.Initialize();
        ColliderUtility.Initialize(gameObject);
        ColliderUtility.CalculateCapsuleColliderDimensions();

        MainCameraTransform = Camera.main.transform;

        switch (Data.EnemyType)
        {
            case EnemyType.Melee:
                MovementStateMachine = new MeleeStateMachine(this);
                break;

            case EnemyType.Range:
                MovementStateMachine = new RangeStateMachine(this);
                CurrentWeapon = 0;
                break;
        }
    }

    private void Start()
    {
        Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        Agent.acceleration = Data.Acceleration;
    }

    private void OnEnable()
    {
        switch (Data.EnemyType)
        {
            case EnemyType.Melee:
                MovementStateMachine.ChangeState(((MeleeStateMachine)MovementStateMachine).IdlingState);
                break;

            case EnemyType.Range:
                MovementStateMachine.ChangeState(((RangeStateMachine)MovementStateMachine).IdlingState);
                break;
        }

        MovementStateMachine.ReusableData.Health = Data.Health;
        maxHealth = Data.Health;
    }

    private void OnDisable()
    {
        if (gameObject.name[0] == '!')
            return;

        PoolingManager.Instance.GetPool(int.Parse(gameObject.name[0] + "")).Return(gameObject);
        LevelManager.Instance.EnemyDown();
    }

    private void OnValidate()
    {
        ColliderUtility.Initialize(gameObject);
        ColliderUtility.CalculateCapsuleColliderDimensions();
    }

    private void OnTriggerEnter(Collider collider)
    {
        MovementStateMachine.OnTriggetEnter(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        MovementStateMachine.OnTriggerExit(collider);
    }
    private void Update()
    {
        MovementStateMachine.HandleInput();

        MovementStateMachine.Update();
    }

    private void FixedUpdate()
    {
        MovementStateMachine.PhysicUpdate();
    }

    public void OnMovementStateAnimationEnterEvent()
    {
        MovementStateMachine.OnAnimationEnterEvent();
    }

    public void OnMovementStateAnimationExitEvent()
    {
        MovementStateMachine.OnAnimationExitEvent();
    }

    public void OnMovementStateAnimationTransitionEvent()
    {
        MovementStateMachine.OnAnimationTransitionEvent();
    }

    public void TakeDamage(float damage)
    {
        MovementStateMachine.ReusableData.Health = Mathf.Max(0, MovementStateMachine.ReusableData.Health - damage);

        if (MovementStateMachine.ReusableData.Health <= 0)
            gameObject.SetActive(false);
    }
}
