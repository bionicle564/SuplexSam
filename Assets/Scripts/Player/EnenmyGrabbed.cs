using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GrabbableEnemy : MonoBehaviour
{
    [Header("Stun Settings")]
    [SerializeField] private float stunDuration = 2f;

    private NavMeshAgent agent;
    private EnemyNavMeshAttack enemyAI;
    private Rigidbody rb;
    public Rigidbody RB { get { return rb; } }

    private Collider col;

    private bool isStunned;

    private float stunTimer;

    private bool isOnGround;
    public LayerMask layerMask;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAI = GetComponent<EnemyNavMeshAttack>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // Run the lil stun snap
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

    void Update()
    {
        /*
        if (stunTimer > 0f)
        {
            stunTimer -= Time.deltaTime;
        }
        if (stunTimer <= 0f && isStunned)
        {
            StunRoutine();
        }
        */
    }

    public void OnGrabbed()
    {
        CancelInvoke();

        isStunned = false; // ?

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
        //Debug.Log("Called OnThrown()");

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

        //ResumeAI(caller, 0.5f);
        StartStun(caller);
    }

    private void StartStun(MonoBehaviour caller)
    {
        if (isStunned) return;

        isStunned = true;
        stunTimer = stunDuration;
        //caller.StartCoroutine(StunRoutine());
    }

    public void StartStunPublic()
    {
        if (isStunned) return;

        agent.enabled = false;
        enemyAI.enabled = false;

        rb.isKinematic = false;
        rb.useGravity = true;

        isStunned = true;
        stunTimer = stunDuration / 2;
    }

    /*private System.Collections.IEnumerator StunRoutine()
    {
        yield return new WaitForSeconds(stunDuration);

        Debug.Log("Stun Snap");

        //isStunned = false;

        if (agent != null)
            agent.enabled = true;

        if (enemyAI != null)
            enemyAI.enabled = true;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }*/

    private void StunRoutine()
    {
        Debug.Log("Stun Snap");

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

        this.tag = "enemy";
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

        Debug.Log("Drop Snap");
    }

    private void OnCollisionStay(Collision collision)
    {
        // Yoinked something off the net marked as "bitwise-and (&) with bitwise shifting (<<)"
        int newMask = layerMask;
        if ((newMask & (1 << collision.gameObject.layer)) != 0)
        {
            // Touching ground (or something)
            if (stunTimer > 0f)
            {
                stunTimer -= Time.deltaTime;
            }
            if (stunTimer <= 0f && isStunned)
            {
                StunRoutine();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody colRB = collision.gameObject.GetComponent<Rigidbody>();
        if (colRB != null)
        {
            if (colRB.linearVelocity.magnitude > 2f && collision.gameObject.tag != "Player")
            {
                StartStunPublic();
            }
        }
    }
}
