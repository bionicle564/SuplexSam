using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float windUpTime = 0.1f;
    [SerializeField] private float attackCooldown = .2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] public LayerMask playerLayer;
    [SerializeField] public ShakeOff.ShakeOffDifficulty attackDifficulty = ShakeOff.ShakeOffDifficulty.Easy;

    private bool isAttacking;
    private float cooldownTimer;
    
    private bool isGrabbed = false;

	public void OnGrabbed()
	{
	    isGrabbed = true;        // Enemy is now grabbed
	    isAttacking = false;     // Cancel any attack in progress
	}
	
	public void OnReleased()
	{
	    isGrabbed = false;       // Enemy released, can attack again
	}

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    public void TryAttack()
	{
	    if (isAttacking || cooldownTimer > 0f || isGrabbed) return;
	    StartCoroutine(AttackRoutine());
	}
	
    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // WIND-UP (pause rotation visually)
        yield return new WaitForSeconds(windUpTime);

        if (!isGrabbed) // Bandaid solution
        {
            DoAttack();
        }

        cooldownTimer = attackCooldown;
        isAttacking = false;
    }

    private void DoAttack()
    {
        Collider[] hits = Physics.OverlapSphere(
            attackPoint.position,
            attackRange,
            playerLayer
        );

        foreach (Collider hit in hits)
        {
            var player = hit.GetComponent<TopDownRigidbodyController>();
            if (player != null)
            {
                if (player.shakeOff != null)
                {
                    if (!player.shakeOff.shakeOffInProgress)
                    {
                        player.SetStun(true);
                        player.shakeOff.StartShakeOff(player, attackDifficulty); // Edit to include ShakeOff type
                    }
                }
            }
        }
    }
}