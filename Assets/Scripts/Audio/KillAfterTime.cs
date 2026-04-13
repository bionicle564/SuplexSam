using UnityEngine;

public class KillAfterTime : MonoBehaviour
{
    public float lifetime = 1f;

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
