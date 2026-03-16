using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HideObstructions;

public class Level5Encounter : MonoBehaviour
{
    public bool isActive = false;
    public bool isComplete = false;

    // Data structure for enemy/spawnpoint pairings
    // THIS IS DUMB AND STUPID AND we probably totally aren't going to change it BUT WE SHOULD
    public class EnemySpawnPair
    {
        public EnemySpawnPair(GameObject e, Transform s)
        {
            enemy = e;
            spawnPoint = s;
        }

        public GameObject enemy { get; set; }
        public Transform spawnPoint { get; set; }

        public override string ToString() => $"({enemy.name}, {spawnPoint})";
    }

    //public List<EnemySpawnPair> encounterList = new List<EnemySpawnPair>();
    List<GameObject> activeEnemies = new List<GameObject>();

    public GameObject enemyPrefab;
    public List<Transform> spawnPoints = new List<Transform>();

    void Start()
    {
        
    }

    void Update()
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

    public void StartEncounter()
    {
        foreach (Transform point in spawnPoints)
        {
            GameObject e = Instantiate(enemyPrefab, point.position, point.rotation);
            activeEnemies.Add(e);
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
            if (!isActive)
            {
                isActive = true;
                StartEncounter();
            }
        }
    }
}
