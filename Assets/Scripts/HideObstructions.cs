using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HideObstructions : MonoBehaviour
{
    GameObject player;
    public LayerMask layerMask;

    public List<MeshRenderer> hideList = new List<MeshRenderer>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Vector3.zero);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.gameObject.GetComponent<MeshRenderer>() != null)
            {
                // Add it to the list of things to toggle off if it isn't already there
            }
        }
    }
}
