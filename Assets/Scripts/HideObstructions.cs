using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObstructions : MonoBehaviour
{
    GameObject player;
    public LayerMask layerMask;

    // Data structure for objects that need to be hidden
    // THIS IS DUMB AND STUPID AND we probably totally aren't going to change it BUT WE SHOULD
    public class ObjectToHide
    {
        public ObjectToHide(GameObject obj, float t, bool i)
        {
            objectSelfReference = obj;
            time = t;
            invis = i;
        }

        public GameObject objectSelfReference { get; set; }
        public float time { get; set; }
        public bool invis { get; set; } // This checks whether the geometry is meant to be invisible

        public override string ToString() => $"({objectSelfReference.name}, {time})";
    }

    public List<ObjectToHide> hideList = new List<ObjectToHide>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //Ray ray = GetComponent<Camera>().ScreenPointToRay(Vector3.zero);
        float distanceToPlayer = Vector3.Distance(player.transform.position, this.transform.position);
        //Vector3 directionTowardPlayer = new Vector3(player.transform.position - this.transform.position);
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanceToPlayer, layerMask))
        {
            //Debug.Log("Hit?");
            if (hit.transform.gameObject.GetComponent<MeshRenderer>() != null)
            {
                // Add it to the list of things to toggle off if it isn't already there
                bool inList = false;
                foreach (ObjectToHide obj in hideList)
                {
                    if (obj.objectSelfReference == hit.transform.gameObject)
                    {
                        inList = true;
                        obj.time = 1.5f;
                    }
                }
                if (!inList)
                {
                    bool isInvis = false;
                    if (!hit.transform.gameObject.GetComponent<MeshRenderer>().enabled)
                    {
                        isInvis = true;
                    }
                    hit.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    hideList.Add(new ObjectToHide(hit.transform.gameObject, 1.5f, isInvis));
                }
            }
        }

        foreach (ObjectToHide obj in hideList)
        {
            obj.time -= Time.deltaTime;
            if (obj.time <= 0)
            {
                if (!obj.invis)
                {
                    obj.objectSelfReference.GetComponent<MeshRenderer>().enabled = true;
                }
                hideList.Remove(obj);
            }
        }
    }
}
