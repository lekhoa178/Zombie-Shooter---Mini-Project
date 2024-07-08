using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IEntity
{
    public static Player Instance { get; private set; }

    [field: Header("Reference")]
    [field: SerializeField] public PlayerSO Data { get; private set; }

    [field: Header("Collisions")]
    [field: SerializeField] public ObjectCapsuleColliderUtility ColliderUtility { get; private set; }
    [field: SerializeField] public PlayerLayerData LayerData { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    [field: Header("Player")]
    [field: SerializeField] public GameObject PlayerRendering { get; private set; }

    [field: Header("Cooldown Data")]
    [field: SerializeField] public PlayerCooldownData CooldownData { get; private set; }

    public Rigidbody Rigidbody { get; private set; }
    public PlayerInput Input { get; private set; }
    public Animator Animator { get; private set; }
    public Material Material { get; private set; }
    public Transform MainCameraTransform { get; private set; }
    public Camera Camera { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public int CurrentWeapon { get; private set; }
    public GunSO GetCurrentWeapon()
    {
        return Data.Weapons[CurrentWeapon];
    }

    public float MaxHealth { get; private set; }


    private PlayerStateMachine MovementStateMachine;

    private Color originalColor;


    private void Awake()
    {
        Instance = this;

        MovementStateMachine = new PlayerStateMachine(this);

        Rigidbody = GetComponent<Rigidbody>();
        Input = GetComponent<PlayerInput>();
        Animator = GetComponentInChildren<Animator>();
        Material = GetComponentInChildren<SkinnedMeshRenderer>().material;
        Agent = GetComponent<NavMeshAgent>();
        Camera = Camera.main;

        AnimationData.Initialize();
        ColliderUtility.Initialize(gameObject);
        ColliderUtility.CalculateCapsuleColliderDimensions();

        MainCameraTransform = Camera.main.transform;
        CurrentWeapon = 0;

        MaxHealth = Data.Health;

        originalColor = Material.color;
    }

    private void Start()
    {
        Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        MovementStateMachine.ChangeState(MovementStateMachine.IdlingState);

        MovementStateMachine.ReusableData.Health = LevelManager.Instance.GetPlayerHealth(MaxHealth);
        WeaponController.Instance.Initialize(Data.Weapons.Select(p => p.Sprite).ToList());
    }

    private void OnDestroy()
    {
        LevelManager.Instance.RegisterPlayerHealth(MovementStateMachine.ReusableData.Health);
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

    public void TakeDamage(float damage)
    {
        if (MovementStateMachine.ReusableData.Immortal)
        {
            return;
        }

        CameraShaker.Instance.Shake();

        MovementStateMachine.ReusableData.Immortal = true;
        MovementStateMachine.ReusableData.Health = Mathf.Max(0, MovementStateMachine.ReusableData.Health - damage);

        HealthController.Instance.UpdateHealth(MovementStateMachine.ReusableData.Health, MaxHealth);

        if (MovementStateMachine.ReusableData.Health <= 0)
        {
            GameResultController.Instance.FinishGame(false);
        }

        StartCoroutine(BeImmortal());
    }

    public void ChangeWeapon()
    {
        CurrentWeapon = (CurrentWeapon + 1) % Data.Weapons.Count;
        CooldownData.ShootStart = 0;

        WeaponController.Instance.Select(CurrentWeapon);
    }

    IEnumerator BeImmortal()
    {
        float timeCount = 0;
        float lerp;

        while (timeCount < Data.TimeToImmortalAfterDamaged)
        {
            timeCount += Time.deltaTime;
            lerp = timeCount * 1f / Data.TimeToImmortalAfterDamaged;

            Material.color = originalColor * Data.EmissionCurve.Evaluate(lerp);

            yield return null;
        }

        Material.color = originalColor;

        MovementStateMachine.ReusableData.Immortal = false;
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

    //public void EnableLayer(int index)
    //{
    //    Animator.SetLayerWeight(index, 1);
    //}

    //public void DisableLayer(int index)
    //{
    //    StartCoroutine(SmoothDisableLayer(index));
    //}

    //private IEnumerator SmoothDisableLayer(int index)
    //{
    //    float t = 1;
    //    while (t > 0)
    //    {
    //        Animator.SetLayerWeight(index, t);

    //        t -= Time.deltaTime * 4f;

    //        yield return null;
    //    }
    //}
}
