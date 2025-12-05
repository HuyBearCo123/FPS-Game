using UnityEngine;

public class RagdollController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;

    void Awake()
    {
        animator = GetComponent<Animator>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        SetRagdollState(false); // tắt ragdoll ban đầu
    }

    public void SetRagdollState(bool active)
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            if (rb != null && rb.gameObject != gameObject)
            {
                rb.isKinematic = !active;
                rb.useGravity = active;
            }
        }

        foreach (Collider col in ragdollColliders)
        {
            if (col != null && col.gameObject != gameObject)
            {
                col.enabled = active;
            }
        }

        if (animator != null)
        {
            animator.enabled = !active;
        }

        // Tắt collider và rigidbody chính khi bật ragdoll
        Collider mainCol = GetComponent<Collider>();
        Rigidbody mainRb = GetComponent<Rigidbody>();
        if (mainCol != null) mainCol.enabled = !active;
        if (mainRb != null) mainRb.isKinematic = active;
    }
}
