using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObstructions : MonoBehaviour
{
    //GameObject player;
    public LayerMask layerMask;

    // Data structure for objects that need to be hidden
    // THIS IS DUMB AND STUPID AND we probably totally aren't going to change it BUT WE SHOULD
    public class ObjectToHide
    {
        public ObjectToHide(GameObject obj, float t)
        {
            objectSelfReference = obj;
            time = t;
        }

        public GameObject objectSelfReference { get; set; }
        public float time { get; set; }

        public override string ToString() => $"({objectSelfReference.name}, {time})";
    }

    public List<ObjectToHide> hideList = new List<ObjectToHide>();

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Vector3.zero);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log("Hit?");
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
                    hideList.Add(new ObjectToHide(hit.transform.gameObject, 1.5f));
                }
            }
        }

        foreach (ObjectToHide obj in hideList)
        {
            if (obj.objectSelfReference == hit.transform.gameObject)
            {
                obj.time -= Time.deltaTime;
                if (obj.time <= 0)
                {
                    hideList.Remove(obj);
                }
            }
        }
    }
}
