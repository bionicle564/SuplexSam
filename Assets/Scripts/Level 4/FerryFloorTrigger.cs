using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class FerryFloorTrigger : MonoBehaviour
{
    public GameObject Level2Ground;
    public List<MeshRenderer> meshrenderers;

    void Start()
    {
        for (int i = 0; i < Level2Ground.transform.childCount; i++)
        {
            if (Level2Ground.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
            {
                meshrenderers.Add(Level2Ground.transform.GetChild(i).GetComponent<MeshRenderer>());
            }

            for(int j = 0; j < Level2Ground.transform.GetChild(i).transform.childCount; j++)
            {
                if (Level2Ground.transform.GetChild(i).GetChild(j).GetComponent<MeshRenderer>() != null)
                {
                    meshrenderers.Add(Level2Ground.transform.GetChild(i).GetChild(j).GetComponent<MeshRenderer>());
                }

                for (int k = 0; k < Level2Ground.transform.GetChild(i).GetChild(j).transform.childCount; k++)
                {
                    if (Level2Ground.transform.GetChild(i).GetChild(j).GetChild(k).GetComponent<MeshRenderer>() != null)
                    {
                        meshrenderers.Add(Level2Ground.transform.GetChild(i).GetChild(j).GetChild(k).GetComponent<MeshRenderer>());
                    }
                }
            }
        }
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player")
        {
            return;
        }
        foreach (MeshRenderer renderer in meshrenderers)
        {
            renderer.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player")
        {
            return;
        }
        foreach (MeshRenderer renderer in meshrenderers)
        {
            renderer.enabled = true;
        }
    }
}
