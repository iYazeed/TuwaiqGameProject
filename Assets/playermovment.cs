using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class playermovment : MonoBehaviour
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
        // Key Inputs
        bool forward = Input.GetKey(KeyCode.W);
        bool backward = Input.GetKey(KeyCode.S);
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Movement Vector
        Vector3 move = Vector3.zero;
        if (forward) move += transform.forward;
        if (backward) move -= transform.forward;
        if (right) move += transform.right;
        if (left) move -= transform.right;

        // Movement Execution
        float speed = isRunning ? runSpeed : walkSpeed;
        controller.Move(move.normalized * speed * Time.deltaTime);

        // Apply gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 🎮 Animation Control (prioritized logic)
        
    }
}