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
        }
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (MeshRenderer renderer in meshrenderers)
        {
            renderer.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (MeshRenderer renderer in meshrenderers)
        {
            renderer.enabled = true;
        }
    }
}
