using UnityEngine;

public class wife : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Change()
    {
        GetComponent<Animator>().SetBool("change", true);
    }

}
