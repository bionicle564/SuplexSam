using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    public float explosionRadius = 5.0f;
    public float explosionForce = 300.0f;
    public float upwardsModifier = 2.0f;
    public float timeDelay = 1.5f;
    public GameObject explosionParticles;

    public bool hasExploded; // Need this to stop recursion. Whoops.

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
        hasExploded = true;

        // Spawn particles
        Instantiate(explosionParticles, transform.position, Quaternion.identity);

        // Apply force
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            BreakableWall wall = hit.GetComponent<BreakableWall>();
            GrabbableEnemy grabbableEnemy = hit.GetComponent<GrabbableEnemy>();
            ExplodingBarrel eb = hit.GetComponent<ExplodingBarrel>();

            if (grabbableEnemy != null)
            {
                grabbableEnemy.StartStunPublic();
            }
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier);
            }
            if (wall != null)
            {
                if (wall.explosivesOnly)
                {
                    //Debug.Log("hit");
                    wall.DestroyedByExplosion();
                }
            }
            if (eb != null)
            {
                if (!eb.hasExploded)
                {
                    eb.Explode();
                }
            }
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (rbMain.linearVelocity.magnitude > 1f && this.tag == "HEAVY")
        {
            Explode();
        }*/
        if (this.tag == "HEAVY")
        {
            Explode();
        }
    }
}
