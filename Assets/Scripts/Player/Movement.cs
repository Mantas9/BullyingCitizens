using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
    // Components
    private Rigidbody rb;

    // Movement
    [Header("Movement")]
    private float moveSpeed; // speed variable
    // Speed variations (will be applied to moveSpeed when necessary)
    public float walkSpeed = 5;
    private Vector3 velocity;
    // Sprinting
    [Header("Sprinting")]
    public float sprintSpeed = 15;
    public bool sprintUnlocked = false;
    public bool sprinting = false;
    public float maxSprintTime = 2;
    public float timeSprintingRemaining = 0;
    // Recovery
    public bool recovering = false;
    [Range(0, 2)] public float recoveryStrength = 0.5f;
    public float timeUntilRecoveryStarts = 5;
    public float timerUntilSprintRecover = 0;

    // Jump/Gravity
    [Header("Jumping")]
    public float jumpStrength = 10;
    public float gravity = 30;
    public float jumpTryTime;
    public float forgiveness = 0.2f;
    public Vector3 normal;
    public bool isGrounded;
    private bool wannaJump;

    // Camera
    [Header("Camera")]
    public Transform head;
    private Camera cam;
    public float camSensitivity = 0.2f;
    private float horizontalAngle;
    private float verticalAngle;
    // Camera Zoom
    [Header("Cam Zoom")]
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
        timeSprintingRemaining = maxSprintTime;
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
        if (cam.fieldOfView != defaultFov)
            ZoomCamera(defaultFov);


        if (Input.GetKey(KeyCode.LeftShift) && sprintUnlocked)
        {
            moveSpeed = sprintSpeed;
            ZoomCamera(sprintFov);

            timeSprintingRemaining -= Time.deltaTime;
            sprinting = true;
            recovering = false;
            timerUntilSprintRecover = 0;
        }
        else
        {
            moveSpeed = walkSpeed;
            sprinting = false;
        }

        if (timeSprintingRemaining < 0 && sprinting)
        {
            moveSpeed = walkSpeed;
            timeSprintingRemaining = 0;
            timerUntilSprintRecover = 0;
            sprintUnlocked = false;
            sprinting = false;
        }

        if (timeSprintingRemaining < maxSprintTime && !sprinting)
        {
            SprintRecovery();
        }
    }

    private void SprintRecovery()
    {
        if (timerUntilSprintRecover < timeUntilRecoveryStarts && !recovering)
        {
            timerUntilSprintRecover += Time.deltaTime;
            sprintUnlocked = false;

            if (timerUntilSprintRecover >= timeUntilRecoveryStarts)
            {
                recovering = true;
                timerUntilSprintRecover = 0;
            }
        }

        if (recovering)
        {
            timeSprintingRemaining = Mathf.Lerp(timeSprintingRemaining, maxSprintTime, Time.deltaTime * recoveryStrength);
            sprintUnlocked = true;

            if (timeSprintingRemaining >= maxSprintTime - 0.05f)
            {
                timeSprintingRemaining = maxSprintTime;
                recovering = false;
            }
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

    public void UpgradeStats(float sprintingSpeed = 0, float sprintingEndurance = 0, float sprintingRecoveryTime = 0)
    {
        sprintSpeed += sprintingSpeed;
        maxSprintTime += sprintingEndurance;
        timeUntilRecoveryStarts -= sprintingRecoveryTime;
    }
}
