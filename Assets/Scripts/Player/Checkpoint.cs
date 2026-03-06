using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool reusable = false;
    bool triggered = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TopDownRigidbodyController>() != null)
        {
            if (reusable)
            {
                other.gameObject.GetComponent<TopDownRigidbodyController>().SetCheckpoint(this.transform);
            }
            else if (!triggered)
            {
                triggered = true;
                other.gameObject.GetComponent<TopDownRigidbodyController>().SetCheckpoint(this.transform);
            }
        }
    }
}
