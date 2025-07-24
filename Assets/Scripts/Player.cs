using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimplePlayerMovement : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float gravity = -9.81f;

    public Animator animator;
    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool forward = Input.GetKey(KeyCode.W);
        bool backward = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 move = Vector3.zero;

        if (forward) move += transform.forward;
        if (backward) move -= transform.forward;
        if (right) move += transform.right;
        if (left) move -= transform.right;

        float speed = isRunning ? runSpeed : walkSpeed;
        controller.Move(move.normalized * speed * Time.deltaTime);

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Animation Control
        if (move.magnitude > 0.1f)
        {
            if (isRunning)
                animator.Play("Fast Run");
            else
                animator.Play("Nwalk");
        }
        else
        {
            animator.Play("Idle");
        }
    }
}