using System.Collections;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab sẽ spawn lại

    private Animator animator;
    private EnemyAI enemyAI; 
    private bool isDead = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
    }

    private void OnEnable()
    {
        RayShooter shooter = Camera.main.GetComponent<RayShooter>();
        if (shooter != null)
        {
            shooter.OnRayHit += HandleRayHit;
        }
    }

    private void OnDisable()
    {
        if (Camera.main != null)
        {
            RayShooter shooter = Camera.main.GetComponent<RayShooter>();
            if (shooter != null)
            {
                shooter.OnRayHit -= HandleRayHit;
            }
        }
    }

    public void ReactToHit()
    {
        if (isDead) return;
        isDead = true;
        if (enemyAI != null)
            enemyAI.enabled = false;
        if (animator != null)
            animator.enabled = false;
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            transform.Rotate(Vector3.right * Time.deltaTime * 90f);
            transform.position += Vector3.down * Time.deltaTime * 0.5f;
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        Vector3[] spawnPoints = new Vector3[]
        {
        new Vector3(24.61f, 0.5f, -17.2f),
        new Vector3(25.85f, 0.5f, 22.93f),
        new Vector3(-26.1f, 0.5f, 22.93f)
        };

        int index = Random.Range(0, spawnPoints.Length);
        Vector3 randomPos = spawnPoints[index];
        if (enemyPrefab != null)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);

            // Đảm bảo AI và Animator bật lại
            EnemyAI ai = newEnemy.GetComponent<EnemyAI>();
            if (ai != null) ai.enabled = true;

            Animator anim = newEnemy.GetComponent<Animator>();
            if (anim != null) anim.enabled = true;
        }

        Destroy(gameObject);
    }


    private void HandleRayHit(GameObject hitObject, Vector3 hitPoint)
    {
        if (hitObject == this.gameObject)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(1);
            }
        }
    }
}
