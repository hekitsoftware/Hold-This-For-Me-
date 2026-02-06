using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("CAMERAS")]
    [SerializeField] private GameObject V_Cam1;
    [SerializeField] private GameObject V_Cam2;
    [SerializeField] private GameObject V_Cam3;
    [SerializeField] private GameObject V_Cam4;
    [SerializeField] private Transform cameraPivot; // The pivot to rotate for vertical look

    [Header("EXTRAS")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private Animator anim;
    [SerializeField] private bool isSprinting = false;

    [Header("MOVEMENTS")]
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float runSpeed = 18f;
    [SerializeField] private float jumpPower = 7f;
    [SerializeField] private float gravity = 8f;
    [SerializeField] private float lookSpeed = 2f; //sensitivity
    [SerializeField] private float lookXLimit = 45f;
    [SerializeField] private float defaultHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchSpeed = 3f;

    [HideInInspector] public bool canMove = true;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool isManualZoomed;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ZoomOut();
    }

    void Update()
    {
        #region Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = 0f;
        float curSpeedY = 0f;

        if (canMove)
        {
            float speed;

            if (isRunning)
            {
                speed = runSpeed;
                isSprinting = true;
            }
            else
            {
                speed = walkSpeed;
                isSprinting = false;
            }

            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");

            curSpeedX = speed * verticalInput;
            curSpeedY = speed * horizontalInput;

            //SPRINTING
            bool isMoving = Mathf.Abs(verticalInput) > 0.01f || Mathf.Abs(horizontalInput) > 0.01f;

            if (isSprinting && isMoving)
            {
                SpeedZoom();
            }
            else if (!isManualZoomed)   // don't override aim zoom
            {
                ZoomOut();
            }
        }

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 10f;
            runSpeed = 18f;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            // Vertical rotation - rotate the camera pivot (pitch)
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            cameraPivot.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Horizontal rotation - rotate the player (yaw)
            transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion

        if (Input.GetKeyDown(KeyCode.E)) PlayerInteract();

        // === GUN AIMING ===
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!isManualZoomed)
            {
                ZoomIn();
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (isManualZoomed)
            {
                ZoomOut();
            }
        }

        // === GUN FIRING ===
        if (Input.GetKeyDown(KeyCode.Mouse0) && isManualZoomed)
        {
            anim.SetTrigger("fire"); // Fire animation
        }
    }

    public void PlayerInteract()
    {
        int layerMask0 = 1 << 0;
        int layerMask3 = 1 << 3;
        int finalMask = layerMask0 | layerMask3;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, 5f, finalMask))
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

    public void ZoomInTalking()
    {
        isManualZoomed = false;
        V_Cam1.SetActive(false);
        V_Cam2.SetActive(true);
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
