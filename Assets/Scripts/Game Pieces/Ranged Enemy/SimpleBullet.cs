using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    public float lifeSpan = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<TopDownRigidbodyController>() != null)
            {
                collision.gameObject.GetComponent<TopDownRigidbodyController>().TakeDamage(1);
                BulletHit();
            }
        }
        else if (collision.gameObject.GetComponent<GrabbableObject>() != null)
        {
            // Code for breaking breakable objects will go here later
            BulletHit();
        }
        else
        {
            BulletHit();
        }
    }

    // Display any hit effects and particles when a bullet lands
    public void BulletHit()
    {
        Destroy(gameObject);
        // Add code for particles or w/e later
    }
}
