using System.Collections.Generic;
using UnityEngine;

public class ZoneChangeDetection : MonoBehaviour
{
    public int zoneID;
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    private DynamicSpawner dynamicSpawner;

    private void Awake()
    {
        dynamicSpawner = FindAnyObjectByType<DynamicSpawner>();  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player entered {this.name}.");
            dynamicSpawner.ChangeActiveZone(zoneID, spawnPoints);
        }
    }
}
