using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private int maxHealth = 100; // Maximum health of the character
    [SerializeField] private int currentHealth; // Current health of the character
    [SerializeField] private PauseGame pauseGame;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        healthBar.fillAmount = currentHealth / 100f;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.fillAmount = currentHealth / 100f;
    }

    private void Die()
    {
        pauseGame.Death();
    }
}
