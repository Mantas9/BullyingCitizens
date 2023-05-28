using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
    // Components
    private Rigidbody rb;

    // Movement
    private float moveSpeed; // speed variable
    // Speed variations (will be applied to moveSpeed when necessary)
    public float walkSpeed = 5;
    public float sprintSpeed = 15;
    public bool sprintUnlocked = false;
    public float maxSprintTime = 2;
    private Vector3 velocity;

    // Jump/Gravity
    public float jumpStrength = 10;
    public float gravity = 30;
    public float jumpTryTime;
    public float forgiveness = 0.2f;
    public Vector3 normal;
    public bool isGrounded;
    private bool wannaJump;

    // Camera
    public Transform head;
    private Camera cam;
    public float camSensitivity = 0.2f;
    private float horizontalAngle;
    private float verticalAngle;
    // Camera Zoom
    [Range(10, 160)]
    public float defaultFov = 60;
    [Range(10, 160)]
    public float sprintFov = 120;
    public float zoomMultiplier = 2;
    public float zoomDuration = 0.2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Sprinting();
        Rotation();

        if (Input.GetButton("Jump") && isGrounded)
        {
            wannaJump = true;
            jumpTryTime = Time.time;
        }

        if (Time.time > jumpTryTime + forgiveness)
            wannaJump = false;
    }

    private void FixedUpdate()
    {
        var ray = new Ray(transform.position, Vector3.down);
        isGrounded = Physics.Raycast(ray, out var hit, 1.3f);
        normal = hit.normal;

        if (normal == Vector3.zero)
            normal = Vector3.up;

        Moving();
        Gravity();
        Jumping();

        rb.velocity = velocity;
    }

    private void ZoomCamera(float target)
    {
        float angle = Mathf.Abs((defaultFov / zoomMultiplier) - defaultFov);
        cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, target, angle / zoomDuration * Time.deltaTime);
    }

    private void Jumping()
    {
        if (wannaJump && isGrounded)
        {
            velocity.y = jumpStrength;
            wannaJump = false;
        }
    }

    private void Sprinting()
    {
        if (!sprintUnlocked)
            return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = sprintSpeed;
            ZoomCamera(sprintFov);
        }
        else if (cam.fieldOfView != defaultFov)
        {
            ZoomCamera(defaultFov);
        }
        else
        {
            moveSpeed = walkSpeed;
        }

    }

    private void Gravity()
    {
        if (!isGrounded)
            velocity.y -= gravity * Time.deltaTime;
    }

    private void Rotation()
    {
        verticalAngle += Input.GetAxisRaw("Mouse Y") * -camSensitivity;
        horizontalAngle += Input.GetAxisRaw("Mouse X") * camSensitivity;

        // Dont allow camera backflip
        verticalAngle = Mathf.Clamp(verticalAngle, -90, 90);

        // Body rotation
        transform.eulerAngles = new Vector3(0, horizontalAngle, 0);

        // Head rotation
        head.localEulerAngles = new Vector3(verticalAngle, 0, 0);
    }


    private void Moving()
    {
        var input = transform.right * Input.GetAxisRaw("Horizontal") +
                    transform.forward * Input.GetAxisRaw("Vertical");
        input.Normalize();
        input *= moveSpeed;

        velocity.x = input.x;
        velocity.z = input.z;

        if (isGrounded)
            velocity.y = input.y;
    }
}
