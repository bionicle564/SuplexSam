using UnityEngine;

public class BarrelDispenser : MonoBehaviour
{
    public bool isOn;

    public Rigidbody barrelPrefab;
    public Transform spawnPoint;

    private GameObject activeInstance;

    void Start()
    {
        
    }

    void Update()
    {
        if (isOn)
        {
            if (activeInstance == null)
            {
                SpawnBarrel();
            }
        }
    }

    public void SpawnBarrel()
    {
        Rigidbody rb = Instantiate(barrelPrefab, spawnPoint.position, spawnPoint.rotation);
        activeInstance = rb.gameObject;
    }
}
