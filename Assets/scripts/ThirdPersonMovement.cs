using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{

    public AudioSource footstepSource;
    public AudioClip[] footstepClips;
    public float footstepInterval = 0.5f;
    public float runstepInterval = 0.3f;

    public float walkSpeed = 2f;

    public float runSpeed = 5f;

    public float turnSpeed = 10f;

    public float gravity = -20f;

    public Key runKey = Key.LeftShift;

    public Animator animator;
    public string speedParameter = "Speed";

    private CharacterController controller;
    private Transform cameraTransform;
    private float verticalVelocity;
    private int speedHash;

    private float stepTimer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        speedHash = Animator.StringToHash(speedParameter);
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        if (cameraTransform == null) return;

        float horizontal = (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1f : 0f)
                         - (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? 1f : 0f);
        float vertical = (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed ? 1f : 0f)
                       - (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed ? 1f : 0f);
        Vector3 input = Vector3.ClampMagnitude(new Vector3(horizontal, 0f, vertical), 1f);


        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();
        Vector3 moveDirection = cameraForward * input.z + cameraRight * input.x;

        if (animator != null)
        {
            bool attacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")
                        || animator.GetNextAnimatorStateInfo(0).IsTag("Attack");
            if (attacking) moveDirection = Vector3.zero;
        }

        bool isMoving = moveDirection.sqrMagnitude > 0.001f;

        bool isRunning = keyboard[runKey].isPressed;
        float targetSpeed = isMoving ? (isRunning ? runSpeed : walkSpeed) : 0f;

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = moveDirection * targetSpeed;
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        if (animator != null && !string.IsNullOrEmpty(speedParameter))
        {
            animator.SetFloat(speedHash, targetSpeed, 0.1f, Time.deltaTime);
        }

        HandleFootsteps(isMoving, isRunning);
    }

    private void HandleFootsteps(bool isMoving, bool isRunning)
    {
        if (controller.isGrounded && isMoving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstepSound();
                stepTimer = isRunning ? runstepInterval : footstepInterval;
            }
        }
        else
        {
            stepTimer = 0.15f;
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepSource == null || footstepClips == null || footstepClips.Length == 0) return;
        {
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            footstepSource.pitch = Random.Range(0.9f, 1.1f);
            footstepSource.PlayOneShot(clip);
        }
    }
}
