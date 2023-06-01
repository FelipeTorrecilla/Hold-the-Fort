using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; // Maximum health of the character
    [SerializeField] private int currentHealth; // Current health of the character

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle death logic here (e.g., play death animation, end the game, etc.)
        Destroy(gameObject);
    }
}
