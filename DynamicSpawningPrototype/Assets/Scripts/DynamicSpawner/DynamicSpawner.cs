using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSpawner : MonoBehaviour
{
    [Header("Spawnpoints")]
    [SerializeField] private List<SpawnPoint> activeSpawnPoints = new List<SpawnPoint>();
    [SerializeField] private List<SpawnPoint> chosenSpawnPoints = new List<SpawnPoint>();

    [Header("Activezone")]
    [SerializeField] private int activeZone = 0;

    [Header("SpawningVariables")]
    [SerializeField] private float spawnDelay = 2.5f;
    private SpawnPoint chosenSpawnPoint;

    [Header("EnemiesAndRound")]
    [SerializeField] private int totalEnemies;
    [SerializeField] private int totalDefeatedEnemies;
    [SerializeField] private int maxEnemiesCap = 300;
    [SerializeField] private int totalActiveEnemies;
    [SerializeField] private int roundIndex = 1;
    public GameObject enemyPrefab;

    private void Awake()
    {
        UpdateTotalEnemies();
    }

    private void Update()
    {
        spawnDelay -= Time.deltaTime;
        if(spawnDelay <= 0)
        {
            StartCoroutine(SpawnEnemyCoroutine());
        }

        //add roundtransition logic and other stuff AFTER the spawning system works for round 1
        //if(totalDefeatedEnemies == totalEnemies)
        //{
        //    totalDefeatedEnemies = 0;
        //    roundIndex++;
        //    UpdateTotalEnemies();
        //}
    }

    public void ChangeActiveZone(int zoneID, List<SpawnPoint> spawnPoints)
    {
        activeZone = zoneID;
        activeSpawnPoints = spawnPoints;
    }

    private void UpdateTotalEnemies()
    {
        totalEnemies = Mathf.FloorToInt(Mathf.Min(4 * Mathf.Pow(1.4f, roundIndex), maxEnemiesCap));
        Debug.Log(totalEnemies);
    }

    private SpawnPoint GetNextSpawnPoint()
    {
        //check if every spawnpoint has been used already, if so, reset the list
        if(chosenSpawnPoints.Count == activeSpawnPoints.Count)
        {
            chosenSpawnPoints.Clear();
        }

        //get a list of the availablespawnpoints
        List<SpawnPoint> availableSpawnPoints = new List<SpawnPoint>();
        foreach (SpawnPoint spawnPoint in activeSpawnPoints)
        {
            if (spawnPoint.canSpawn && !chosenSpawnPoints.Contains(spawnPoint))
            {
                availableSpawnPoints.Add(spawnPoint);
            }
        }

        // If there are no available spawn points, return null
        if (availableSpawnPoints.Count == 0)
        {
            return null;
        }

        // Randomly select a spawnpoint from available spawnpoints
        int randomIndex = Random.Range(0, availableSpawnPoints.Count);
        SpawnPoint chosenSpawnPoint = availableSpawnPoints[randomIndex];

        chosenSpawnPoints.Add(chosenSpawnPoint);
        StartCoroutine(chosenSpawnPoint.IsChosen());

        return chosenSpawnPoint;
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        while ((chosenSpawnPoint = GetNextSpawnPoint()) == null)
        {
            //ADD THIS TO PREVENT A TIGHT LOOP (performance optimization)
            yield return null;
        }
        spawnDelay = 2.5f;
        SpawnEnemy(chosenSpawnPoint);
    }

    private void SpawnEnemy(SpawnPoint spawnPoint)
    {
        //have to add references for the enemyPrefab like navmesh navigation etc after instantiation, look up on how to do it efficiently

        //Turn the actual spawnpoints in the scene so the enemies will look forward (at the player) when they spawn
        Instantiate(enemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }
}
