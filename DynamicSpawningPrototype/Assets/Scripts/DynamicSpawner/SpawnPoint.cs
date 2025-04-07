using System.Collections;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private float spawnPointCooldown = 4f;
    private float spawnPointCooldownTimer = 0f;
    public bool canSpawn = true;

    public IEnumerator IsChosen()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnPointCooldown);
        canSpawn = true;
    }

}
