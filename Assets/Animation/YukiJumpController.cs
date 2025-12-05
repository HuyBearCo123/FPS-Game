using UnityEngine;

public class YukiJumpController : MonoBehaviour
{
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Kiểm tra chạm đất
        isGrounded = controller.isGrounded;
        animator.SetBool("isGrounded", isGrounded);

        // Nếu chạm đất và đang rơi -> reset velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Nhấn phím nhảy
        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetTrigger("jumpTrigger"); // Gửi lệnh nhảy
        }

        // Áp dụng trọng lực
        velocity.y += gravity * Time.deltaTime;

        // Di chuyển theo trọng lực
        controller.Move(velocity * Time.deltaTime);

        // Gửi tốc độ rơi cho Animator
        animator.SetFloat("yVelocity", velocity.y);
    }
}
