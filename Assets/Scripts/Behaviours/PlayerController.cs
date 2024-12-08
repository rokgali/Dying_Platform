using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] 
    private float MoveSpeed = 5f;
    [SerializeField]
    private float RotationSpeed = 10f;
    [SerializeField]
    private float JumpForce = 10f;
    private float GroundCheckDistance = 1.5f;

    private GameObject PreviousGroundObject;
    private GameObject CurrentGroundObject;

    private bool IsAiming;
    private bool IsGrounded;

    [Header("Combat")]
    [SerializeField]
    private float ThrowableObjectMaxCarryTimeMilliseconds = 5000f;
    private float ThrowableObjectCarryLoadTimeMilliseconds = 0;
    [SerializeField]
    private float ThrowableObjectPickupSpeedMilliseconds = 1000f;
    private float ThrowableObjectPickupLoadTimeMilliseconds = 0f;
    [SerializeField]
    private float PickUpRange = 2f;
    [SerializeField]
    private float LaunchPower = 25f;

    private bool PickupInProgress = false;
    private bool CarryInProgress = false;

    public IPickable PickedUpObject { get; private set; }
    private IPickable PickedUpObjectBuffer;

    private HashSet<IPickable> PreviousThrowableObjectsInRange;
    private HashSet<IPickable> ThrowableObjectsInRange;

    private bool HighlightInRangeObjects = false;

    private float MarkInRangeObjectsTimeMilliseconds = 0;
    private float MarkInRangeObjectsMaxTimeMilliseconds = 2000;

    [SerializeField]
    private Transform HoldPosition;
    [SerializeField]
    private Transform LaunchPosition;

    private Vector2 MoveInput;
    private Vector2 ShootInput;
    public Vector3 Direction { get; private set; }

    private InputSystem PlayerInputActions;

    [field: SerializeField] public Camera Camera { get; set; }

    private bool PickupButtonHeldDown;

    private void Awake()
    {
        PlayerInputActions = new InputSystem();
        Direction = transform.forward;
        IsAiming = false;
        PickedUpObject = null;
    }

    private void OnEnable()
    {
        PlayerInputActions.Enable();

        PlayerInputActions.Floor.Movement.performed += OnMove;
        PlayerInputActions.Floor.Movement.canceled += context => OnMoveCancel();

        PlayerInputActions.Floor.Shoot.performed += OnShoot;
        PlayerInputActions.Floor.Shoot.canceled += context => OnShootCancel();

        PlayerInputActions.Floor.PickUp.performed += OnPickUp;
        PlayerInputActions.Floor.PickUp.canceled += OnPickUpCancel;

        PlayerInputActions.Floor.Jump.performed += OnJump;

        PreviousThrowableObjectsInRange = new HashSet<IPickable>();
        ThrowableObjectsInRange = new HashSet<IPickable>();

    }

    private void OnDisable()
    {
        PlayerInputActions.Floor.Movement.performed -= OnMove;
        PlayerInputActions.Floor.Movement.canceled -= context => OnMoveCancel();

        PlayerInputActions.Floor.Shoot.performed -= OnShoot;
        PlayerInputActions.Floor.Shoot.canceled -= context => OnShootCancel();

        PlayerInputActions.Floor.PickUp.performed -= OnPickUp;
        PlayerInputActions.Floor.PickUp.canceled -= OnPickUpCancel;

        PlayerInputActions.Disable();
    }

    private void Update()
    {
        ThrowableObjectsInRange = FindThrowableObjectsInRange();
        ThrowableObjectsInRange.Remove(CurrentGroundObject?CurrentGroundObject.GetComponent<IPickable>():null);

        MovePlayer();
        SetIsGrounded();
        Aim();

        if (PickupInProgress)
        {
            ThrowableObjectPickupLoadTimeMilliseconds += Time.deltaTime * 1000;
            AssignPickedUpObject();
        }

        if(HighlightInRangeObjects)
        {
            MarkInRangeObjectsTimeMilliseconds += Time.deltaTime * 1000;

            HighlightNewThrowableObjects();
            RemoveOldHighlightOnThrowableObjects();
        }

        UnhighlightInRangeObjectsTimerIfTimeExceeded();

        CarryPickedUpObject();

        UpdatePreviousGroundObject();
        UpdatePreviousThrowableObjects();
    }

    private void MovePlayer()
    {
        Vector3 cameraFowardDirection = Camera.transform.forward;
        Vector3 cameraRightDirection = Camera.transform.right;

        cameraFowardDirection.y = 0;
        cameraRightDirection.y = 0;

        cameraFowardDirection.Normalize();
        cameraRightDirection.Normalize();

        Vector3 moveDirection = (cameraFowardDirection * MoveInput.y + cameraRightDirection * MoveInput.x).normalized;
        Vector3 move = moveDirection * MoveSpeed * Time.deltaTime;

        transform.Translate(move, Space.World);

        if (moveDirection != Vector3.zero && IsAiming == false)
        {
            SetPlayerDirection(moveDirection);
        }
    }

    private void UnhighlightInRangeObjectsTimerIfTimeExceeded()
    {
        if(MarkInRangeObjectsMaxTimeMilliseconds <= MarkInRangeObjectsTimeMilliseconds)
        {
            HighlightInRangeObjects = false;
            MarkInRangeObjectsTimeMilliseconds = 0;
            RemoveHighlightsOnThrowableObjects();
        }
    }

    private void UpdatePreviousGroundObject()
    {
        // PreviousGroundObject = new GameObject();

        if(PreviousGroundObject != CurrentGroundObject)
        {
            PreviousGroundObject = CurrentGroundObject;
        }
    }

    private void UpdatePreviousThrowableObjects()
    {
        if(PreviousThrowableObjectsInRange != ThrowableObjectsInRange)
        {
            PreviousThrowableObjectsInRange = ThrowableObjectsInRange;
        }
    }

    private void Aim()
    {
        Vector3 aimDirection;

        if(Mathf.Abs(ShootInput.x) > Mathf.Abs(ShootInput.y))
        {
            aimDirection = new Vector3(Mathf.Sign(ShootInput.x), 0, 0);
        }
        else
        {
            aimDirection = new Vector3(0, 0, Mathf.Sign(ShootInput.y));
        }

        if(aimDirection != Vector3.zero && IsAiming == true)
        {
            SetPlayerDirection(aimDirection);
        }
    }

    private void SetPlayerDirection(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.5f);
        transform.rotation = targetRotation;
        Direction = transform.forward;
    }

    private HashSet<IPickable> FindThrowableObjectsInRange()
    {
        var throwableObjects = new HashSet<IPickable>();
        var overlapSpherePosition = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);

        Collider[] colliders = Physics.OverlapSphere(overlapSpherePosition, PickUpRange / 2);

        foreach (Collider collider in colliders)
        {
            IPickable throwableObject = collider.gameObject.GetComponent<IPickable>();

            if (throwableObject != null)
            {
                throwableObjects.Add(throwableObject);
            }
        }

        return throwableObjects;
    }

    private IEnumerable<IPickable> ThrowableObjectsToUnhighlight() => PreviousThrowableObjectsInRange.Except(ThrowableObjectsInRange);

    private void RemoveOldHighlightOnThrowableObjects()
    {
        foreach(var throwableObject in ThrowableObjectsToUnhighlight())
        {
            if (UnhighlightThrowableObjectCheck(throwableObject)) { throwableObject.Highlight(false); }
        }
    }
    private void HighlightNewThrowableObjects()
    {
        foreach (var throwableObject in ThrowableObjectsInRange)
        {
            if(HighlightThrowableObjectCheck(throwableObject)) { throwableObject.Highlight(true); }
        }
    }

    private void RemoveHighlightsOnThrowableObjects()
    {
        foreach (var throwableObject in ThrowableObjectsInRange)
        {
            if (UnhighlightThrowableObjectCheck(throwableObject)) { throwableObject.Highlight(false); }
        }
    }

    private bool UnhighlightThrowableObjectCheck(IPickable tobj)
    {
        return tobj != null && tobj.GetIsHighlighted()
            //&& !(tobj.GetGameObject() == CurrentGroundObject) 
            ? true : false;
    }

    private bool HighlightThrowableObjectCheck(IPickable tobj)
    {
        return tobj != null && !tobj.GetIsHighlighted()
            //&& !(tobj.GetGameObject() == CurrentGroundObject) 
            ? true : false;
    }

    private void AssignPickedUpObject()
    {
        if (ThrowableObjectPickupLoadTimeMilliseconds >= ThrowableObjectPickupSpeedMilliseconds)
        {
            PickedUpObject = PickedUpObject == null ? PickedUpObjectBuffer : PickedUpObject;

            ThrowableObjectPickupLoadTimeMilliseconds = 0;
            PickupInProgress = false;
            CarryInProgress = true;
        }
    }

    private void CarryPickedUpObject()
    {
        if (PickedUpObject == null)
        {
            CarryInProgress = false;

            return;
        }

        if (ThrowableObjectCarryLoadTimeMilliseconds >= ThrowableObjectMaxCarryTimeMilliseconds)
        {
            PickedUpObject = null;
            CarryInProgress = false;

            ThrowableObjectCarryLoadTimeMilliseconds = 0;
        }

        UnityEngine.Debug.Log("Not carrying thingy! :/");

        if (CarryInProgress)
        {
            UnityEngine.Debug.Log("Carrying thingy! :D");
            // throwableObjectCarryLoadTimeMilliseconds = 0;
            ThrowableObjectCarryLoadTimeMilliseconds += Time.deltaTime * 1000;
            PickedUpObject.Move(HoldPosition.position, ThrowableObjectCarryLoadTimeMilliseconds, ThrowableObjectMaxCarryTimeMilliseconds);
        }
    }

    private void OnPickUp(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        Ray ray = Camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit))
        {
            IPickable hitObject = hit.collider.gameObject.GetComponent<IPickable>();

            if (hitObject == null) return;

            hitObject.Highlight(!hitObject.GetIsHighlighted());

            float distance = Mathf.Abs(Vector3.Distance(transform.position, hitObject.GetPosition()));

            if(distance > PickUpRange)
            {
                HighlightInRangeObjects = true;

                return;
            }

            PickupInProgress = true;
            PickedUpObjectBuffer = hitObject;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if(IsGrounded)
        {
            transform.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    private void SetIsGrounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, GroundCheckDistance))
        {
            CurrentGroundObject = hit.collider.gameObject;

            IsGrounded = true;
        }
        else
        {
            CurrentGroundObject = null;

            IsGrounded = false;
        }
    }

    private void OnPickUpCancel(InputAction.CallbackContext context)
    {
        PickupInProgress = false;
        ThrowableObjectPickupLoadTimeMilliseconds = 0;
        PickupButtonHeldDown = false;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCancel()
    {
        MoveInput = Vector2.zero;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        IsAiming = true;
        ShootInput = context.ReadValue<Vector2>();

        SetPlayerDirection(new Vector3(ShootInput.x, 0, ShootInput.y));

        if (PickedUpObject != null)
        {
            LaunchPickedUpObject();
        }
    }
    private void OnShootCancel()
    {
        IsAiming = false;
        ShootInput = Vector2.zero;
    }

    private void LaunchPickedUpObject()
    {
        CarryInProgress = false;
        ThrowableObjectCarryLoadTimeMilliseconds = 0;

        PickedUpObject.GetGameObject().transform.position = LaunchPosition.position;
        PickedUpObject.Launch(Direction, LaunchPower);

        PickedUpObject = null;
    }
}
