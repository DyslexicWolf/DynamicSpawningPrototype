using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("HealthVariables")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float healthRegen;

    private void Awake()
    {
        maxHealth = 20;
        currentHealth = maxHealth;
        healthRegen = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
    }
}
