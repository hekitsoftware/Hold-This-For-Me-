using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public Camera playerCamera;
    [SerializeField] public float walkSpeed = 6f;
    [SerializeField] public float runSpeed = 12f;
    [SerializeField] public float jumpPower = 7f;
    [SerializeField] public float gravity = 10f;
    [SerializeField] public float lookSpeed = 2f;
    [SerializeField] public float lookXLimit = 45f;
    [SerializeField] public float defaultHeight = 2f;
    [SerializeField] public float crouchHeight = 1f;
    [SerializeField] public float crouchSpeed = 3f;

    [HideInInspector] public bool canMove = true;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion

        if (Input.GetKeyDown(KeyCode.E)) PlayerInteract();
    }

    public void PlayerInteract()
    {
        int layerMask0 = 1 << 0;
        int layerMask3 = 1 << 3;
        int finalMask = layerMask0 | layerMask3;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, 4f, finalMask))
        {
            Interact intScript = hit.transform.GetComponent<Interact>();
            if (intScript != null) intScript.CallInteract(this);
        }
    }
}
