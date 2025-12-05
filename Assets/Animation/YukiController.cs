using UnityEngine;

public class YukiController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float runSpeed = 3f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform; // Thêm: camera để lấy hướng nhìn

    private CharacterController characterController;
    private Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Lấy hướng di chuyển theo camera
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Bỏ trục Y để nhân vật không bị nghiêng
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Tính hướng di chuyển tương đối theo camera
        Vector3 moveDir = camForward * vertical + camRight * horizontal;

        bool isMoving = moveDir.magnitude > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentSpeed = isRunning ? runSpeed : moveSpeed;
        bool isEmote = Input.GetKey(KeyCode.L);

        if(isEmote)
        {
            animator.SetBool("isEmote", true);
        }
        else
        {
            animator.SetBool("isEmote", false);
        }

        // Di chuyển
        if (isMoving)
        {


            // Dùng CharacterController.Move để di chuyển
            characterController.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }

        // Animation
        if (!isMoving)
        {
            animator.SetFloat("Speed", 0f);
        }
        else
        {
            animator.SetFloat("Speed", isRunning ? 2f : 0.9f);
        }
    }
}
