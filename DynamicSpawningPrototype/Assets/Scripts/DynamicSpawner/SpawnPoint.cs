using System.Collections;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private float spawnPointCooldown = 4f;
    public bool canSpawn = true;

    public IEnumerator IsChosenCoroutine()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnPointCooldown);
        canSpawn = true;
    }

}
