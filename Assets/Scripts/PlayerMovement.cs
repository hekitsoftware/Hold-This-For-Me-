using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject V_Cam1;
    [SerializeField] private GameObject V_Cam2;
    [SerializeField] private GameObject V_Cam3;
    [SerializeField] private Transform cameraPivot; // The pivot to rotate for vertical look

    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float jumpPower = 7f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float lookSpeed = 2f;
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
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
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
            walkSpeed = 6f;
            runSpeed = 12f;
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

        if (Input.GetKeyDown(KeyCode.Mouse1) && isManualZoomed == false) { ZoomIn();}
        if (Input.GetKeyUp(KeyCode.Mouse1) && isManualZoomed == true) { ZoomOut(); }
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
    }

    public void ZoomOut()
    {
        isManualZoomed = false;
        V_Cam1.SetActive(true);
        V_Cam2.SetActive(false);
        V_Cam3.SetActive(false);
    }

    public void ZoomInTalking()
    {
        isManualZoomed = false;
        V_Cam1.SetActive(false);
        V_Cam2.SetActive(true);
        V_Cam3.SetActive(false);
    }
}
