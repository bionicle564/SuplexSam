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
    List<Vector4> pos;

    public float sprayTime;

    private void Start()
    {
        pelletsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 50, sizeof(float) * 4);
        activeWater = new LinkedList<GameObject>();
        pos = new List<Vector4>();
        
        waterPassMat.SetInt("_Render", 0);
        //timer = sprayTime;    
    }

    private void OnDestroy()
    {
        pelletsBuffer.Release();
    }

    override public void Grab()
    {
        waterPassMat.SetBuffer("_WaterPos", pelletsBuffer);
        waterPassMat.SetInt("_Render", 1);
        held = true;
    }

    override public void LetGo()
    {
        waterPassMat.SetInt("_Render", 0);
        held = false;
    }


    private void Update()
    {
        if (held)
        {
            
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                GameObject temp = Instantiate(waterBall, this.transform.position + transform.forward * 2f, transform.rotation);
                temp.GetComponent<Rigidbody>().AddForce(transform.forward * 5, ForceMode.Impulse);

                activeWater.AddFirst(temp);

                timer = sprayTime;
            }

            pos.Clear();
            

            var node = activeWater.First;
            while (node != null)
            {
                var next = node.Next; // store before modifying

                if (node.Value == null)
                {
                    activeWater.Remove(node);
                }
                else
                {
                    pos.Add(node.Value.transform.position);
                }

                node = next;
            }
            pelletsBuffer.SetData(pos);

            killTimer -= Time.deltaTime;
            if (killTimer <= 0f)
            {
                //var node2 = activeWater.First;
                //while (node2 != null)
                //{
                //    var next = node2.Next; // store before modifying

                //    if (node2.Value == null)
                //    {
                //        activeWater.Remove(node2);
                //    }
                //    node2 = next;
                //}
                killTimer = 2f;
            }
        }
        else
        {
            pos.Clear();
            
        }
        //Debug.Log(activeWater.Count());
    }
}

