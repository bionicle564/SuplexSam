using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class FireHydrant : HoldActions
{
    public GameObject waterBall;

    private float timer;

    public float sprayTime;

    private void Start()
    {
        //timer = sprayTime;    
    }

    private void Update()
    {
        if (held)
        {
            timer -= Time.deltaTime;

            if(timer <= 0f)
            {
                GameObject temp = Instantiate(waterBall, this.transform.position + transform.forward *2f, transform.rotation);
                temp.GetComponent<Rigidbody>().AddForce(transform.forward * 10,ForceMode.Impulse);
                
                timer = sprayTime;
            }
        }
    }
}

