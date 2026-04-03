using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;


public class FireHydrant : HoldActions
{
    public GameObject waterBall;
    public Material waterPassMat;

    private float timer;
    private float killTimer;

    private GraphicsBuffer pelletsBuffer;
    private LinkedList<GameObject> activeWater;

    public float sprayTime;

    private void Start()
    {
        pelletsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 50, sizeof(float) * 3);
        activeWater = new LinkedList<GameObject>();
        waterPassMat.SetBuffer("_WaterPos", pelletsBuffer);
        //timer = sprayTime;    
    }

    private void OnDestroy()
    {
        pelletsBuffer.Release();
    }

    private void Update()
    {
        if (held)
        {
            timer -= Time.deltaTime;

            if(timer <= 0f)
            {
                GameObject temp = Instantiate(waterBall, this.transform.position + transform.forward *2f, transform.rotation);
                temp.GetComponent<Rigidbody>().AddForce(transform.forward * 5,ForceMode.Impulse);

                activeWater.AddFirst(temp);

                timer = sprayTime;
            }
        }

        List<Vector3> pos = new List<Vector3>();
        for (LinkedListNode<GameObject> node = activeWater.First; node != activeWater.Last; node = node.Next)
        {
            if (node.Value != null)
            {
                activeWater.Remove(node);

            }
            else
            {
                pos.Add(node.Value.transform.position);
            }
        }


        killTimer -= Time.deltaTime;
        if(killTimer <= 0f)
        {
            
            killTimer = 2f;
        }
        Debug.Log(activeWater.Count());
    }
}

