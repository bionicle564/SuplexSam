using UnityEngine;
using UnityEngine.AI;

public class GrabbableEnemy : MonoBehaviour
{
    [Header("Stun Settings")]
    [SerializeField] private float stunDuration = 2f;

    private NavMeshAgent agent;
    private EnemyNavMeshAttack enemyAI;
    private Rigidbody rb;
    private Collider col;

    private bool isStunned;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAI = GetComponent<EnemyNavMeshAttack>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void OnGrabbed()
    {
        Debug.Log("Called!");

        CancelInvoke();

        isStunned = false;

        if (agent != null)
            agent.enabled = false;

        if (enemyAI != null)
            enemyAI.enabled = false;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    // Called ONLY when thrown
    public void OnThrown(MonoBehaviour caller)
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        StartStun(caller);
    }

    public void OnDropped(MonoBehaviour caller)
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        ResumeAI(caller, 0f);
    }

    private void StartStun(MonoBehaviour caller)
    {
        if (isStunned) return;

        isStunned = true;
        caller.StartCoroutine(StunRoutine());
    }

    private System.Collections.IEnumerator StunRoutine()
    {
        yield return new WaitForSeconds(stunDuration);

        isStunned = false;

        if (agent != null)
            agent.enabled = true;

        if (enemyAI != null)
            enemyAI.enabled = true;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    private void ResumeAI(MonoBehaviour caller, float delay)
    {
        caller.StartCoroutine(ResumeAfterDelay(delay));
    }

    private System.Collections.IEnumerator ResumeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (agent != null)
            agent.enabled = true;

        if (enemyAI != null)
            enemyAI.enabled = true;
    }
}
