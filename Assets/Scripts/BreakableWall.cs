using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public bool explosivesOnly = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void DestroyedByExplosion()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!explosivesOnly && collision.gameObject.tag == "HEAVY")
        {
            Destroy(gameObject);
        }
    }
}
