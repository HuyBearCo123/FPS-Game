using UnityEngine;

public class YukiCamera : MonoBehaviour
{
    public Transform target;        // Nhân vật để camera theo
    public float distance = 4.0f;   // Khoảng cách camera - nhân vật
    public float height = 2.0f;     // Độ cao của camera so với nhân vật
    public float rotationSpeed = 120.0f; // Tốc độ xoay chuột
    public float smoothSpeed = 10.0f;    // Độ mượt khi camera di chuyển

    private float yaw = 0.0f;
    private float pitch = 10.0f; // Góc nhìn xuống mặc định

    void Start()
    {
        if (target == null)
        {
            target = transform; // Nếu chưa gán, camera sẽ theo chính object gắn script này
        }

        // Khởi tạo góc quay
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Lấy góc xoay từ chuột
        yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * 0.5f * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -30f, 60f); // Giới hạn góc nhìn lên/xuống

        // Tính vị trí mong muốn của camera
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance + Vector3.up * height;

        // Di chuyển mượt
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.5f); // Nhìn lên phía đầu nhân vật
    }
}