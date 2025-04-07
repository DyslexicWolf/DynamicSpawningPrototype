using UnityEngine;

public class ZoneChangeDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player entered {this.name}.");
        }
    }
}
