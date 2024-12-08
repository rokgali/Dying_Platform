using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : WeaponUser, IDamageable, IPlayerMoveable, IPicker, IPlayerAttackable, IInteractor
{
    [field: SerializeField] public string Name { get; }
    [field: SerializeField] private float MaxHealth;
    private float CurrentHealth;
    public Rigidbody RB { get; private set; }
    private Vector3 FacingDirection = Vector3.forward;
    public GameObject StandingOn { get; private set; }
    [SerializeField] private LayerMask GroundLayer;

    [SerializeField] float Height;
    [SerializeField] float Drag;
    [field: SerializeField] public Camera Camera { get; set; }
    [field: SerializeField] public float Strength { get; private set; }

    #region State Machine Variables

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerShootState ShootState { get; private set; }
    public PlayerPickableState PickableState { get; private set; }
    public PlayerCarryingState CarryingState { get; private set; }

    #endregion

    private InputSystem _playerInputActions;

    #region Movement Variables

    public bool IsGrounded { get; set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }
    public Vector3 MoveInput { get; private set; }
    public Vector3 JumpVelocity { get; set; }
    public Vector3 PlayerFacingDirection { get; private set; }

    #endregion

    #region Pickable
    [field: SerializeField] public float PickupSpeedMilliseconds { get; set; }
    public float ElapsedPickupTimeMilliseconds { get; set; }
    public bool IsPickingUp { get; set; }
    private GameObject _previousSelectedGameObject;
    public List<IPickable> PickedUpObjects { get; private set; }
    [field: SerializeField] public Transform CarryingPosition { get; private set; }
    [field: SerializeField] public Transform LaunchPosition { get; private set; }
    [SerializeField] private int _maxPickableObjects;

    #endregion
    #region Combat
    public WeaponMachine WeaponMachine { get; private set; }
    public Vector3 FiringDirection { get; private set; }
    public override List<Weapon> Weapons { get; }

    #endregion

    private void Awake()
    {
        _playerInputActions = new();
        StateMachine = new();
        WeaponMachine = new();

        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        ShootState = new PlayerShootState(this, StateMachine);
        PickableState = new PlayerPickableState(this, StateMachine);
        CarryingState = new PlayerCarryingState(this, StateMachine);
    }

    private void OnEnable()
    {
        _playerInputActions.Floor.Movement.performed += OnMove;
        _playerInputActions.Floor.Movement.canceled += OnMoveCancel;

        _playerInputActions.Floor.Shoot.performed += OnShoot;
        _playerInputActions.Floor.Shoot.canceled += OnShootCancel;

        _playerInputActions.Floor.PickUp.performed += OnPickUp;
        _playerInputActions.Floor.PickUp.canceled += OnPickUpCancel;

        _playerInputActions.Floor.Interact.performed += OnInteract;

        _playerInputActions.Floor.Jump.performed += OnJump;

        _playerInputActions.Floor.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Floor.Movement.performed -= OnMove;
        _playerInputActions.Floor.Movement.canceled -= OnMoveCancel;

        _playerInputActions.Floor.Shoot.performed -= OnShoot;
        _playerInputActions.Floor.Shoot.canceled -= OnShootCancel;

        _playerInputActions.Floor.PickUp.performed -= OnPickUp;
        _playerInputActions.Floor.PickUp.canceled -= OnPickUpCancel;

        _playerInputActions.Floor.Interact.performed -= OnInteract;

        _playerInputActions.Disable();
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;

        RB = GetComponent<Rigidbody>();

        StateMachine.Initialize(IdleState);

        _previousSelectedGameObject = new();
        PickedUpObjects = new();
    }

    private void Update()
    {
        if(IsPickingUp)
        {
            ElapsedPickupTimeMilliseconds += Time.deltaTime * 1000;
        }

        StateMachine.CurrentPlayerState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, Height * 0.5f + 0.2f, GroundLayer);
        Debug.DrawRay(transform.position, Vector3.down * (Height * 0.5f + 0.2f), Color.red);

        if (IsGrounded)
        {
            RB.drag = Drag;
        }
        else
        {
            RB.drag = 0;
        }

        StateMachine.CurrentPlayerState.PhysicsUpdate();
    }

    #region Health

    public void Damage(float damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0f)
            Die();
    }

    public void Die()
    {

    }

    #endregion

    #region Movement

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();

        MoveInput = new Vector3(inputValue.x, 0, inputValue.y);
    }

    public void OnMoveCancel(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        MoveInput = Vector3.zero;
    }

    public void OnPlayerIsGrounded()
    {
        
    }

    public void SetPlayerFacingDirection(Vector3 velocity)
    {
        Quaternion targetRotation = Quaternion.LookRotation(velocity);
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.5f);
        transform.rotation = targetRotation;
        PlayerFacingDirection = transform.forward;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        JumpVelocity = Vector3.up;
    }

    #endregion

    #region Animation Triggers

    public enum AnimationTriggerType
    {
        PlayerDamaged,
        PlayerWalking,
        PlayerDead
    }

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentPlayerState.AnimationTriggerEvent(triggerType);
    }

    #endregion

    #region Pickable
    public HashSet<IPickable> FindPickableObjectsInRange()
    {
        throw new System.NotImplementedException();
    }

    public void PickUpPickableObject()
    {
        RaycastHit hit;

        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            IPickable pickableObject = hitObject.GetComponent<IPickable>();

            if (pickableObject == null)
            {
                IsPickingUp = false;
            }

            if(ElapsedPickupTimeMilliseconds >= pickableObject.GetPickupTimeMilliseconds())
            {
                ElapsedPickupTimeMilliseconds = 0;

                PickedUpObjects.Add(pickableObject);

                IsPickingUp = false;
            }

            if(_previousSelectedGameObject != hitObject)
            {
                _previousSelectedGameObject = hitObject;

                ElapsedPickupTimeMilliseconds = 0;
            }
        }
    }

    public void OnPickUp(InputAction.CallbackContext ctx)
    {
        if (PickedUpObjects.Count >= _maxPickableObjects) return; 

        IsPickingUp = true;
    }

    public void OnPickUpCancel(InputAction.CallbackContext ctx)
    {
        ElapsedPickupTimeMilliseconds = 0;

        IsPickingUp = false;
    }

    public void Carry()
    {
        Debug.Log("I'm carrying thing");
    }

    #endregion

    #region Combat
    public void OnShoot(InputAction.CallbackContext ctx)
    {
        Vector3 mousePosition = Input.mousePosition;

        Ray ray = Camera.ScreenPointToRay(mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 worldPoint = hit.point;

            Vector3 direction = worldPoint - transform.position;
            FiringDirection = direction.normalized;
        }
    }

    public void OnShootCancel(InputAction.CallbackContext ctx)
    {
        FiringDirection = Vector3.zero;
    }

    public void LaunchPickableObject(Vector3 direction)
    {
        if(PickedUpObjects.Count > 0)
        {
            Debug.Log("Launching Pickable Object!");

            PickedUpObjects.RemoveAt(0);
        }
    }

    public override void PickUpWeapon(Weapon weapon)
    {
        base.PickUpWeapon(weapon);
    }

    public override void DropWeapon(Weapon weapon)
    {
        base.DropWeapon(weapon);
    }

    #endregion

    #region Interactions

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        GameObject interactableGameObject = new();

        Interact(interactableGameObject);
    }

    public void Interact(GameObject obj)
    {
        Debug.Log("Interacting with thingy! ");
    }

    #endregion

    public GameObject GetGameObject()
    {
        return transform.gameObject;
    }

}
