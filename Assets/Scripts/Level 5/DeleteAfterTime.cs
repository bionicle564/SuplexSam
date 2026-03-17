using UnityEngine;

public class DeleteAfterTime : MonoBehaviour
{
    public float lifetime;

    void Start()
    {
        
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
