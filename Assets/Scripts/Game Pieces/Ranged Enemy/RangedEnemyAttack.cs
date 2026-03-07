using UnityEngine;
using UnityEngine.ProBuilder;

public class RangedEnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackTimer = 0f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private float bulletSpeed = 1f;

    private bool isGrabbed = false;

    public void OnGrabbed()
    {
        isGrabbed = true; // Enemy is now grabbed
    }

    public void OnReleased()
    {
        isGrabbed = false; // Enemy released, can attack again
    }

    private void Update()
    {
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public void Shoot(Transform target)
    {
        if (attackTimer <= 0f && !isGrabbed)
        {
            attackTimer = attackCooldown;
            Debug.Log("Shoot");
            Rigidbody bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            Vector3 direction = target.position - this.transform.position;
            direction = Vector3.Normalize(direction);
            bullet.linearVelocity = direction * bulletSpeed;
        }
    }
}
