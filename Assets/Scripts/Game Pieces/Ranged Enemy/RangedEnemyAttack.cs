using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private Transform shootPoint;

    private bool isGrabbed = false;

    public void OnGrabbed()
    {
        isGrabbed = true;        // Enemy is now grabbed
        //isAttacking = false;     // Cancel any attack in progress
    }

    public void OnReleased()
    {
        isGrabbed = false;       // Enemy released, can attack again
    }

    private void Update()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
        if (attackCooldown <= 0f)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        Debug.Log("Shoot");
    }
}
