using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public PlayerHealth playerHealth;
    private int damage = 5;
    private bool canAttack = true;
    private float attackDelay = 1.5f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canAttack)
        {
            Debug.Log(other.gameObject.name);
            playerHealth.TakeDamage(damage);
            StartCoroutine(AttackDelayCoroutine());
        }
    }

    private IEnumerator AttackDelayCoroutine()
    {
        Debug.Log("in attacked delay");
        canAttack = false;
        yield return new WaitForSeconds(attackDelay);
        Debug.Log("attack delay done");
        canAttack = true;
    }
}
