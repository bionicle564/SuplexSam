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
        //Instantiate(explosionParticles, transform.position, Quaternion.identity);

        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier);
            }
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rbMain.linearVelocity.magnitude > 2)
        {
            Explode();
        }
    }
}
