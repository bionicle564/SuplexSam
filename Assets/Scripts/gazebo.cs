using UnityEngine;

public class gazebo : BreakableWall
{
    public int health = 4;
    public GameObject enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DestroyedByExplosion()
    {

        health--;
        Debug.Log(health);

        if(health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Instantiate(enemy, transform.position + new Vector3(-15,0,0), transform.rotation);

        }


    }

}
