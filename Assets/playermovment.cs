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
    public Transform cameraTransform; // اربط كاميرا اللاعب هنا

    private CharacterController controller;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 🔁 تدوير اللاعب أفقيًا (يمين/يسار)
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.Rotate(0f, mouseX, 0f);

        // ⬆⬇ تدوير الكاميرا رأسيًا (أعلى/أسفل)
        float mouseY = Input.GetAxis("Mouse Y") * verticalSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // 🎮 التحكم بالحركة
        bool forward = Input.GetKey(KeyCode.W);
        bool backward = Input.GetKey(KeyCode.S);
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 move = Vector3.zero;
        if (forward) move += transform.forward;
        if (backward) move -= transform.forward;
        if (right) move += transform.right;
        if (left) move -= transform.right;

        float speed = isRunning ? runSpeed : walkSpeed;
        controller.Move(move.normalized * speed * Time.deltaTime);

        // ⚖️ الجاذبية
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // ✨ ممكن تضيف كود الأنميشن هنا
    }
}