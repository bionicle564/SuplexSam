using UnityEngine;
using UnityEngine.ProBuilder;

public class RangedEnemyAttack : MonoBehaviour
{
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
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public void Shoot()
    {
        if (attackCooldown <= 0f && !isGrabbed)
        {
            Debug.Log("Shoot");
            Rigidbody bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.linearVelocity = transform.forward * bulletSpeed;
        }
    }
}
