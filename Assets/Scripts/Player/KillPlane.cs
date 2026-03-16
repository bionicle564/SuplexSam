using UnityEngine;

public class KillPlane : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<TopDownRigidbodyController>().RespawnAtCheckpoint();
            // Need to maybe change this to not insta-kill?
        }
        else if (other.GetComponent<Rigidbody>() != null)
        {
            Destroy(other.gameObject);
        }
    }
}
