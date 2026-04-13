using UnityEngine;

public class level1enemySig : MonoBehaviour
{
    public Animator controller;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    
    public void Walk()
    {
        controller.SetBool("moving", true);
    }


    public void Idle()
    {
        controller.SetBool("moving", false);
    }
}
