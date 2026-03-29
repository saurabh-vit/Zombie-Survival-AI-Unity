using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 180f;

    [Header("Gravity")]
    public float gravity = -9.81f;

    private CharacterController controller;
    private Animator animator;

    private float verticalInput;
    private float horizontalInput;
    private float yVelocity;
    private bool isRunning;
    private bool isAttacking;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        ReadInput();
        HandleRotation();
        HandleMovement();
        HandleAnimation();
    }

    void ReadInput()
    {
        verticalInput = 0;
        horizontalInput = 0;

        // Attack input
        isAttacking = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        // 🚫 If attacking → do not allow movement input
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return;
        }

        // Forward
        if (Keyboard.current.wKey.isPressed)
            verticalInput = 1;

        // Rotate 180° on S press
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            transform.Rotate(0f, 180f, 0f);
            verticalInput = 1;
        }

        if (Keyboard.current.aKey.isPressed)
            horizontalInput = -1;

        if (Keyboard.current.dKey.isPressed)
            horizontalInput = 1;

        isRunning = Keyboard.current.leftShiftKey.isPressed;
    }

    void HandleRotation()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        if (horizontalInput != 0)
        {
            transform.Rotate(0f, horizontalInput * rotationSpeed * Time.deltaTime, 0f);
        }
    }

    void HandleMovement()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.forward * verticalInput * speed;

        if (controller.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 finalMove =
            move * Time.deltaTime +
            Vector3.up * yVelocity * Time.deltaTime;

        controller.Move(finalMove);
    }

    void HandleAnimation()
    {
        if (animator == null) return;

        if (isAttacking && controller.isGrounded)
        {
            animator.SetTrigger("Attack");
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        if (verticalInput == 0)
            animator.SetInteger("YBot", 0);  // Idle
        else if (isRunning)
            animator.SetInteger("YBot", 2);  // Run
        else
            animator.SetInteger("YBot", 1);  // Walk
    }
}