using System;
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
    [SerializeField] private int currentSpawnPointIndex = 0;
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
            spawnDelay = 2.5f;
            chosenSpawnPoint = GetNextSpawnPoint();
            SpawnEnemy(chosenSpawnPoint);
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
        //add randomization so the spawnpoints is randomly chosen (from the spawnpoints that are left)

        //check if every spawnpoint has been used already, if so, reset the list
        if(chosenSpawnPoints.Count == activeSpawnPoints.Count)
        {
            chosenSpawnPoints.Clear();
        }

        foreach(SpawnPoint spawnPoint in activeSpawnPoints)
        {
            //check if the spawnpoint is not on CD and if the spawnpoint hasnt been chosen before yet
            if(spawnPoint.canSpawn && !chosenSpawnPoints.Contains(spawnPoint))
            {
                chosenSpawnPoints.Add(spawnPoint);
                StartCoroutine(spawnPoint.IsChosen());
                return spawnPoint;
            }
        }

        return null;
    }

    private void SpawnEnemy(SpawnPoint spawnPoint)
    {
        //have to add references for the enemyPrefab like navmesh navigation etc after instantiation, look up on how to do it efficiently

        //Turn the actual spawnpoints in the scene so the enemies will look forward (at the player) when they spawn
        Instantiate(enemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }
}
