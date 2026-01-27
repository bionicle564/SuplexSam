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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    private void Update()
    {
        if (player == null || enemyAttack == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectionRange)
        {
            agent.ResetPath();
            return;
        }

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
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