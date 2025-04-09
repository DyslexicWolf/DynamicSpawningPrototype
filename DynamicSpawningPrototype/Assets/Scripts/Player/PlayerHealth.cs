using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float maxHealth = 20;
    private float currentHealth;
    private float healthRegeneration = 2f;
    private float healthRegenerationTimer = 1.5f;
    private bool canRegenerate = true;
    private bool isRecovering = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        healthRegenerationTimer -= Time.deltaTime;

        if(currentHealth < maxHealth)
        {
            if (canRegenerate && !isRecovering && healthRegenerationTimer <= 0)
            {
                Debug.Log("Regening health");
                currentHealth += healthRegeneration;
                healthRegenerationTimer = 1.5f;
            }
        }
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        canRegenerate = false;
        StartCoroutine(CanRegenerateCoroutine());
        Debug.Log(currentHealth);
    }

    private IEnumerator CanRegenerateCoroutine()
    {
        isRecovering = true;
        yield return new WaitForSeconds(3f);
        canRegenerate = true;
        isRecovering = false;
    }
}
