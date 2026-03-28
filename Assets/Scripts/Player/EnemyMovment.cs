using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMeshAttack : MonoBehaviour
{
    public bool isRanged = false;

    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Ranges")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;

    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;
    private EnemyAttack enemyAttack;
    private RangedEnemyAttack rangedEnemyAttack;
    private GrabbableEnemy grabbableEnemy;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        if (!isRanged)
        {
            enemyAttack = GetComponent<EnemyAttack>();
        }
        else
        {
            rangedEnemyAttack = GetComponent<RangedEnemyAttack>();
        }
        grabbableEnemy = GetComponent<GrabbableEnemy>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //agent.avoidancePriority = Random.Range(0, 50);
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectionRange)
        {
            if (agent.enabled)
            {
                if (agent.isOnNavMesh) // Should stop enemies from floating, but will also not reset them properly if there is no navmesh
                {
                    agent.ResetPath();
                }
            }
            return;
        }

        if (distance > attackRange)
        {
            grabbableEnemy.RB.isKinematic = true;
            obstacle.enabled = false;
            agent.enabled = true;
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(player.position);
            }

            // Shoot haphazardly while moving
            if (isRanged)
            {
                rangedEnemyAttack.Shoot(player.transform);
            }
        }
        else
        {
            agent.enabled = false;
            obstacle.enabled = true;
            grabbableEnemy.RB.isKinematic = false;
            if (!isRanged)
            {
                enemyAttack.TryAttack();
            }
            else
            {
                rangedEnemyAttack.Shoot(player.transform);
            }
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
