using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSpawner : MonoBehaviour
{
    [Header("SpawnpointsVariables")]
    [SerializeField] private List<SpawnPoint> activeSpawnPoints = new List<SpawnPoint>();
    [SerializeField] private List<SpawnPoint> chosenSpawnPoints = new List<SpawnPoint>();

    [Header("ActivezoneVariables")]
    [SerializeField] private int activeZone = 0;

    [Header("SpawningVariables")]
    [SerializeField] private float spawnDelay = 2.5f;
    private SpawnPoint chosenSpawnPoint;

    [Header("EnemiesVariables")]
    [SerializeField] private int totalEnemiesToSpawn;
    [SerializeField] private int totalDefeatedEnemies;
    [SerializeField] private int totalActiveEnemies;
    private int maxActiveEnemiesCap = 3;
    [SerializeField] private bool isBelowActiveEnemiesCap = true;
    [SerializeField] private int maxEnemiesCap = 300;
    public GameObject enemyPrefab;

    [Header("RoundVariables")]
    [SerializeField] private int roundIndex = 1;
    [SerializeField] private float roundTransitionDelay = 5f;
    [SerializeField] private bool roundFlipped = false;

    private void Awake()
    {
        UpdateTotalEnemies();
        StartCoroutine(RoundTransitionDelayCoroutine());
    }

    private void Update()
    {
        if (totalActiveEnemies < maxActiveEnemiesCap)
        {
            isBelowActiveEnemiesCap = true;
        }
        else
        {
            isBelowActiveEnemiesCap = false;
        }

        spawnDelay -= Time.deltaTime;
        if(spawnDelay <= 0 && totalEnemiesToSpawn > 0 && roundFlipped && isBelowActiveEnemiesCap)
        {
            totalEnemiesToSpawn--;
            totalActiveEnemies++;
            StartCoroutine(SpawnEnemyCoroutine());
        }

        
        //add roundtransition logic and other stuff AFTER the spawning system works for round 1
        //if(totalDefeatedEnemies == totalEnemies)
        //{
        //    roundIndex++;
        //    StartCorountine(RoundTransitionDelayCoroutine());
        //    roundTransitionDelay = 5f;
        //    UpdateTotalEnemies();
        //    totalDefeatedEnemies = 0;
        //    totalSpawnedEnemies = 0;
        //}
    }

    public void ChangeActiveZone(int zoneID, List<SpawnPoint> spawnPoints)
    {
        activeZone = zoneID;
        activeSpawnPoints = spawnPoints;
    }

    private void UpdateTotalEnemies()
    {
        totalEnemiesToSpawn = Mathf.FloorToInt(Mathf.Min(4 * Mathf.Pow(1.4f, roundIndex), maxEnemiesCap));
        Debug.Log(totalEnemiesToSpawn);
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
        StartCoroutine(chosenSpawnPoint.IsChosenCoroutine());

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

    private IEnumerator RoundTransitionDelayCoroutine()
    {
        roundFlipped = false;
        yield return new WaitForSeconds(roundTransitionDelay);
        roundFlipped = true;
    }
}
