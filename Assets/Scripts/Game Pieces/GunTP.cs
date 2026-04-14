using UnityEngine;

public class GunTP : MonoBehaviour
{
    public GameObject parentToBe;

    void Start()
    {
        
    }

    void Update()
    {
        this.transform.position = parentToBe.transform.position;
    }
}
