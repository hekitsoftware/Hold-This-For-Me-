using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("CAMERAS")]
    [SerializeField] private GameObject V_Cam1;
    [SerializeField] private GameObject V_Cam2;
    [SerializeField] private GameObject V_Cam3;
    [SerializeField] private GameObject V_Cam4;
    [SerializeField] private Transform cameraPivot;

    [Header("EXTRAS")]
    [SerializeField] private Animator anim;
    [SerializeField] private Weapon weapon;

    [Header("MOVEMENTS")]
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float runSpeed = 18f;
    [SerializeField] private float jumpPower = 7f;
    [SerializeField] private float gravity = 8f;
    [SerializeField] private float lookSpeed = 1f;
    [SerializeField] private float lookXLimit = 45f;
    [SerializeField] private float defaultHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchSpeed = 3f;

    private CharacterController characterController;
    private Vector3 moveDirection;
    private float rotationX;
    private bool isManualZoomed;
    private bool canMove = true;

    // input cache
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpPressed;
    private bool sprintHeld;
    private bool crouchHeld;

    private InputAction_Main actions;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        actions = new InputAction_Main();

        // Movement
        actions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        actions.Player.Move.canceled += _ => moveInput = Vector2.zero;

        actions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        actions.Player.Look.canceled += _ => lookInput = Vector2.zero;

        actions.Player.Jump.performed += _ => jumpPressed = true;
        actions.Player.Sprint.performed += _ => sprintHeld = true;
        actions.Player.Sprint.canceled += _ => sprintHeld = false;

        actions.Player.Crouch.performed += _ => crouchHeld = true;
        actions.Player.Crouch.canceled += _ => crouchHeld = false;

        actions.Player.Interact.performed += _ => PlayerInteract();

        actions.Player.Throw.performed += _ => ZoomIn();
        actions.Player.Throw.canceled += _ => ZoomOut();

        actions.Player.Attack.performed += _ =>
        {
            //fire anim
        };
    }

    private void OnEnable() => actions.Enable();
    private void OnDisable() => actions.Disable();

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ZoomOut();
    }

    private void Update()
    {
        Movement();
        HandleLook();
    }

    private void Movement()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float speed = sprintHeld ? runSpeed : walkSpeed;

        Vector3 desiredMove = (forward * moveInput.y + right * moveInput.x) * speed;

        if (characterController.isGrounded)
        {
            moveDirection.y = -1f;

            if (jumpPressed)
            {
                moveDirection.y = jumpPower;
                jumpPressed = false;
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        moveDirection.x = desiredMove.x;
        moveDirection.z = desiredMove.z;

        // Crouch
        if (crouchHeld)
        {
            characterController.height = crouchHeight;
            speed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        // Sprint camera zoom
        bool isMoving = moveInput.magnitude > 0.1f;
        if (sprintHeld && isMoving)
            SpeedZoom();
        else if (!isManualZoomed)
            ZoomOut();
    }

    void HandleLook()
    {
        if (!canMove) return;

        rotationX -= lookInput.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        cameraPivot.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.Rotate(Vector3.up * lookInput.x * lookSpeed);
    }

    public void PlayerInteract()
    {
        int layerMask = (1 << 0) | (1 << 3);
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 5f, layerMask))
        {
            Interact intScript = hit.transform.GetComponent<Interact>();
            if (intScript != null) intScript.CallInteract(this);
        }
    }

    public void ZoomIn()
    {
        isManualZoomed = true;
        V_Cam1.SetActive(false);
        V_Cam2.SetActive(false);
        V_Cam3.SetActive(true);
        V_Cam4.SetActive(false);
    }

    public void ZoomOut()
    {
        isManualZoomed = false;
        V_Cam1.SetActive(true);
        V_Cam2.SetActive(false);
        V_Cam3.SetActive(false);
        V_Cam4.SetActive(false);
    }

    public void SpeedZoom()
    {
        isManualZoomed = false;
        V_Cam1.SetActive(false);
        V_Cam2.SetActive(false);
        V_Cam3.SetActive(false);
        V_Cam4.SetActive(true);
    }
}
