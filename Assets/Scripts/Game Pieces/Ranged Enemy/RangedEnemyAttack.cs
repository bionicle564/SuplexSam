using System.Collections.Generic;
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

    [Header("Voice Lines")]
    public List<AudioClip> tauntClips;

    public float clipVolume = 0.5f;
    public float clipSpatial = 0.8f;

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
            // Sounds
            if (Random.Range(0, 10) == 0) // 10/90?
            {
                AudioClip clip = tauntClips[Random.Range(0, tauntClips.Count)];
                VoiceManager.Instance.VoiceTryGoon(clip, this.transform, clipVolume, clipSpatial);
            }

            attackTimer = attackCooldown;
            Rigidbody bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation); // Rotation needs to be changed
            Vector3 direction = target.position - shootPoint.transform.position;
            direction = Vector3.Normalize(direction);
            bullet.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            bullet.linearVelocity = direction * bulletSpeed;
        }
    }
}
