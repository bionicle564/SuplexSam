using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Animation")]
    public Animator animController;

    [Header("Voice Lines")]
    public List<AudioClip> tauntClips;

    public float clipVolume = 0.5f;
    public float clipSpatial = 0.8f;

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

        animController.SetTrigger("punch");

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
                        player.shakeOff.StartShakeOff(player, attackDifficulty);
                    }
                    else
                    {
                        player.TakeDamage(1); // Currently doesn't do varied damage, can change if necessary
                        //Debug.Log("Enemy Damages Player");
                    }

                    // Sounds
                    if (Random.Range(0, 5) == 0) // 20/80?
                    {
                        AudioClip clip = tauntClips[Random.Range(0, tauntClips.Count)];
                        VoiceManager.Instance.VoiceTryGoon(clip, this.transform, clipVolume, clipSpatial);
                    }
                }
            }
        }
    }
}
