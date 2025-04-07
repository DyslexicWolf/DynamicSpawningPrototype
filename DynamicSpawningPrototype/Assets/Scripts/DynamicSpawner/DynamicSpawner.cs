using System;
using UnityEngine;

public class DynamicSpawner : MonoBehaviour
{
    [Header("Spawnpoints")]
    [SerializeField] private SpawnPoint[] activeSpawnPoints;

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

    public void ChangeActiveZone(int zoneID, SpawnPoint[] spawnPoints)
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
        //have to add logic that each spawnpoint gets chosen before a spawnpoint can be chosen again
        foreach(var spawnPoint in activeSpawnPoints)
        {
            if(spawnPoint.canSpawn)
            {
                StartCoroutine(spawnPoint.IsChosen());
                return spawnPoint;
            }
        }

        return null;
    }

    private void SpawnEnemy(SpawnPoint spawnPoint)
    {
        //have to add references for the enemyPrefab like navmesh navigation etc after instantiation, look up on how to do it efficiently
        //add spawnpoint orientation to the instantiate, turn the actual spawnpoints in the scene so the enemies will look forward (at the player) when they spawn
        Instantiate(enemyPrefab, spawnPoint.transform);
    }
}
