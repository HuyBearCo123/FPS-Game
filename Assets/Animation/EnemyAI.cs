using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float chaseSpeed = 5f;
    public float rotationSpeed = 120f;
    public float detectRange = 6f;
    public float fireRange = 4f; 
    public float obstacleRange = 1.5f;
    public GameObject bulletPrefab;
    public float fireRate = 1.5f;
    public float bulletSpeed = 10f;
    private float nextFireTime = 0f;
    public float stopChaseRange = 4.5f; 
    private bool isShooting = false;

    private Animator animator;
    private GameObject player;
    private bool isChasing = false;
    public float viewAngle = 90f;

    void Start()
    {
        player = GameObject.FindWithTag("Yuki");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        //Kiểm tra trong vùng nhìn và khoảng cách
        if (distanceToPlayer <= detectRange && angleToPlayer <= viewAngle / 2f)
        {
            isChasing = true;
        }
        else if (distanceToPlayer > detectRange + 2f)
        {
            isChasing = false;
        }

        if (isChasing)
        {
            if (isShooting)
            {
                // Nếu đang bắn, chỉ ngừng khi người chơi ra khỏi vùng đệm
                if (distanceToPlayer > stopChaseRange)
                {
                    isShooting = false;
                    ChasePlayer();
                }
                else
                {
                    StopAndShoot();
                }
            }
            else
            {
                // Nếu chưa bắn, chỉ bắt đầu bắn khi người chơi thật sự vào gần
                if (distanceToPlayer <= fireRange)
                {
                    isShooting = true;
                    StopAndShoot();
                }
                else
                {
                    ChasePlayer();
                }
            }
        }
        else
        {
            Patrol();
        }

    }


    void Patrol()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        if (animator != null) animator.SetFloat("Speed", moveSpeed);

        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, obstacleRange))
        {
            if (hit.collider.gameObject != player)
            {
                float turnAngle = Random.Range(90f, 180f) * (Random.value > 0.5f ? 1 : -1);
                transform.Rotate(Vector3.up, turnAngle);
            }
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        transform.Translate(Vector3.forward * chaseSpeed * Time.deltaTime);
        if (animator != null) animator.SetFloat("Speed", chaseSpeed);
    }

    void StopAndShoot()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        if (animator != null) animator.SetFloat("Speed", 0f);

        TryShootPlayer();
    }

    void TryShootPlayer()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            Vector3 origin = transform.position + Vector3.up * 1.5f;
            Vector3 direction = (player.transform.position + Vector3.up * 1.0f - origin).normalized;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, detectRange))
            {
                if (hit.collider.CompareTag("Yuki"))
                {
                    GameObject bullet = Instantiate(bulletPrefab, origin, Quaternion.LookRotation(direction));
                    Rigidbody rb = bullet.GetComponent<Rigidbody>();

                    rb.linearVelocity = direction * bulletSpeed;

                    Destroy(bullet, 3f);
                }
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        int rays = 30;
        float halfAngle = viewAngle / 2f;

        Vector3 prevPoint = transform.position + (Quaternion.Euler(0, -halfAngle, 0) * transform.forward * detectRange);
        for (int i = -rays / 2 + 1; i <= rays / 2; i++)
        {
            float angle = (i / (float)rays) * halfAngle * 2f;
            Vector3 nextPoint = transform.position + (Quaternion.Euler(0, angle, 0) * transform.forward * detectRange);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
