using UnityEngine;

public class ProxyPickup : MonoBehaviour
{
    MeshRenderer meshRenderer;
    public bool isPickedUp;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !isPickedUp)
        {
            isPickedUp = true;
            meshRenderer.enabled = false;
        }
    }
}
