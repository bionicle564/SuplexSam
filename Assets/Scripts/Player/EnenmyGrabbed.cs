using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class GrabbableEnemy : MonoBehaviour
{
    [Header("Stun Settings")]
    [SerializeField] private float stunDuration = 2f;

    private NavMeshAgent agent;
    private EnemyNavMeshAttack enemyAI;
    private Rigidbody rb;
    public Rigidbody RB { get { return rb; } }

    private Collider col;

    [SerializeField] private bool isStunned; // What is happening???

    [SerializeField] private float stunTimer;

    private bool isOnGround;
    public LayerMask layerMask;

    [Header("Voice Lines")]
    public List<AudioClip> hurtClips;
    public List<AudioClip> grabbedClips;
    public List<AudioClip> defeatClips;

    public float clipVolume = 0.5f;
    public float clipSpatial = 0.8f;

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

        // Sounds
        if (Random.Range(0, 5) == 0) // 20/80?
        {
            AudioClip clip = grabbedClips[Random.Range(0, grabbedClips.Count)];
            VoiceManager.Instance.VoiceTryGoon(clip, this.transform, clipVolume, clipSpatial);
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

        // Sounds
        if (Random.Range(0, 5) == 0) // 20/80?
        {
            AudioClip clip = defeatClips[Random.Range(0, defeatClips.Count)];
            VoiceManager.Instance.VoiceTryGoon(clip, this.transform, clipVolume, clipSpatial);
        }
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

    public void OnReleased()
    {
        //StartStunPublic();
        /*if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        StartStun(caller);*/
        StartStunPublic();
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
        //Debug.Log("Stun Snap");

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

        //Debug.Log("Drop Snap");
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
            if (colRB.linearVelocity.magnitude > 1.6f && collision.gameObject.tag != "Player")
            {
                StartStunPublic();
                // Sounds
                if (Random.Range(0, 5) == 0) // 20/80?
                {
                    AudioClip clip = hurtClips[Random.Range(0, hurtClips.Count)];
                    VoiceManager.Instance.VoiceTryGoon(clip, this.transform, clipVolume, clipSpatial);
                }
            }
        }
    }
}
