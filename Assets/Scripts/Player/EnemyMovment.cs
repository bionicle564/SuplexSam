using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMeshAttack : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Ranges")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;

    private NavMeshAgent agent;
    private EnemyAttack enemyAttack;
    private GrabbableEnemy grabbableEnemy;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAttack = GetComponent<EnemyAttack>();
        grabbableEnemy = GetComponent<GrabbableEnemy>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null || enemyAttack == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectionRange)
        {
            if (agent.enabled)
            {
                agent.ResetPath();
            }
            return;
        }

        if (distance > attackRange)
        {
            grabbableEnemy.RB.isKinematic = true;
            agent.enabled = true;
            //agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            //agent.isStopped = true;
            agent.enabled = false;
            grabbableEnemy.RB.isKinematic = false;
            enemyAttack.TryAttack(); // ← ONLY THIS
            FacePlayer();
        }
    }

    private void FacePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0f;

        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }
}