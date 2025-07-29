using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class playermovment : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float gravity = -9.81f;
    public float rotationSpeed = 5f;
    public float verticalSensitivity = 2f;
    public float maxVerticalAngle = 60f;

    public Animator animator;
    public Transform cameraTransform;       // Player's camera
    public GameObject torchInWorld;         // Torch on ground
    public GameObject torchInHand;          // Torch in player's hand
    public float interactDistance = 2f;     // Interaction range

    private CharacterController controller;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    private bool hasTorch = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null) animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Make sure the torch in hand is hidden initially
        if (torchInHand != null) torchInHand.SetActive(false);
    }

    void Update()
    {
        // 🎮 Rotate horizontally
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.Rotate(0f, mouseX, 0f);

        // ⬆⬇ Rotate camera vertically
        float mouseY = Input.GetAxis("Mouse Y") * verticalSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);
        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // 🕹️ Movement input
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move += transform.forward;
        if (Input.GetKey(KeyCode.S)) move -= transform.forward;
        if (Input.GetKey(KeyCode.D)) move += transform.right;
        if (Input.GetKey(KeyCode.A)) move -= transform.right;

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        controller.Move(move.normalized * speed * Time.deltaTime);

        // 🌍 Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 🎞️ Walk animation
        animator.SetFloat("Speed", move.magnitude);

        // 🕯️ Torch interaction
        if (torchInWorld != null && !hasTorch)
        {
            float distance = Vector3.Distance(transform.position, torchInWorld.transform.position);
            if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
            {
                PickUpTorch();
            }
        }
    }

    void PickUpTorch()
    {
        hasTorch = true;

        // Hide world torch and show held torch
        torchInWorld.SetActive(false);
        torchInHand.SetActive(true);

        // Trigger animation
        animator.SetTrigger("PickUpTorch"); // Make sure this trigger leads to "tw" animation
    }
}