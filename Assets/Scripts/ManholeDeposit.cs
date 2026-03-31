using UnityEngine;

public class ManholeDeposit : MonoBehaviour
{
    public float jumpHeight = 10f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HEAVY"))
        {
            Rigidbody objectRB = other.gameObject.GetComponent<Rigidbody>();
            objectRB.linearVelocity = new Vector3(objectRB.linearVelocity.x, objectRB.linearVelocity.y - 0.5f, objectRB.linearVelocity.z);
            Destroy(other);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            // Reset jump from held prop
            Rigidbody rb = player.GetComponent<Rigidbody>();
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            // Set to new height
            player.GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
        }
    }
}
