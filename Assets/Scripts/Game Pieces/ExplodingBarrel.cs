using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    public float explosionRadius = 5.0f;
    public float explosionForce = 300.0f;
    public float upwardsModifier = 2.0f;
    public float timeDelay = 1.5f;
    public GameObject explosionParticles;

    Rigidbody rbMain;

    void Start()
    {
        rbMain = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    public void Explode()
    {
        // Spawn particles
        Instantiate(explosionParticles, transform.position, Quaternion.identity);

        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            BreakableWall wall = hit.GetComponent<BreakableWall>();

            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier);
            }
            if (wall != null)
            {
                if (wall.explosivesOnly)
                {
                    Debug.Log("hit");
                    wall.DestroyedByExplosion();
                }
            }
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rbMain.linearVelocity.magnitude > 1.5f && this.tag == "HEAVY")
        {
            Explode();
        }
    }
}
