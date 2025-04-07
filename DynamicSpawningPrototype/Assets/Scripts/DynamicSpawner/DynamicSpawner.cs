using UnityEngine;

public class DynamicSpawner : MonoBehaviour
{
    [Header("Spawnpoints")]
    [SerializeField] private GameObject[] activeSpawnPoints;

    [Header("Activezone")]
    [SerializeField] private int activeZone = 0;

    void Start()
    {
        
    }


    public void ChangeActiveZone(int zoneID, GameObject[] spawnPoints)
    {
        activeZone = zoneID;
        activeSpawnPoints = spawnPoints;
    }
}
