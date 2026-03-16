using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HideObstructions;

public class Level5Encounter : MonoBehaviour
{
    public bool triggersEvent;
    public GameObject objectToToggle;

    List<GameObject> activeEnemies = new List<GameObject>();

    public GameObject enemyPrefab;
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Logic Switches")]
    public bool isActive = false;
    public bool isComplete = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (isActive)
        {
            List<GameObject> removeList = new List<GameObject>();

            foreach (GameObject enemy in activeEnemies)
            {
                if (enemy == null)
                {
                    removeList.Add(enemy);
                }
            }

            foreach (GameObject enemy in removeList)
            {
                activeEnemies.Remove(enemy);
            }

            if (activeEnemies.Count == 0)
            {
                CompleteEncounter();
            }
        }
    }

    public void StartEncounter()
    {
        isActive = true;
        foreach (Transform point in spawnPoints)
        {
            GameObject e = Instantiate(enemyPrefab, point.position, point.rotation);
            activeEnemies.Add(e);
        }

        if (triggersEvent)
        {
            objectToToggle.SetActive(!objectToToggle.activeInHierarchy);
        }
    }

    public void CompleteEncounter()
    {
        isComplete = true;
        isActive = false;

        /*List<GameObject> removeList = new List<GameObject>();

        foreach (GameObject enemy in activeEnemies)
        {
            removeList.Add(enemy);
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!isActive && !isComplete)
            {
                isActive = true;
                StartEncounter();
            }
        }
    }
}
