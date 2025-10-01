using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject zombiePrefab;
    private float timeToSpawnDelay = 10f;
    private float timeToSpawn;
    private float zombiesMax = 2;
    [SerializeField] private Transform[] pointsToSpawn;
    private int zombiesPerWave = 10;
    private float distance;

    void Start()
    {
        timeToSpawnDelay = Random.Range(1, 2);
        timeToSpawn = timeToSpawnDelay;
        distance = Random.Range(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < zombiesPerWave;)
        {
            int spawnPointIndex = Random.Range(0, pointsToSpawn.Length);
            Instantiate(zombiePrefab, pointsToSpawn[spawnPointIndex].position * distance, Quaternion.identity);
        }
    }
    
}
